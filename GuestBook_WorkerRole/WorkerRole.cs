using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;

using CloudApp_Infrastructure;
using GuestBook_Data;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace GuestBook_WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private BlobStorage _blobStorage;
        private QueueStorage _queueStorage;
        private const string CLOUD_STORAGE_ACCOUNT_CONFIG_SETTING = "DataConnectionString";
        private const string CLOUD_BLOB_REFERENCE = "guestbookpics";
        private const string CLOUD_QUEUE_REFERENCE = "guestthumbs";
        private const int THUMBNAIL_SIZE = 128;

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.TraceInformation("GuestBook_WorkerRole entry point is called", "Information");

            while (true)
            {
                try
                {
                    // TODO: Retrieve a new message from the queue
                    CloudQueueMessage newMessage =  _queueStorage.GetNewMessage();


                    if (newMessage != null)
                    {
                        var newMessageParts = newMessage.AsString.Split(new [] {','});
                        var imageBlobName = newMessageParts[0];
                        var partitionKey = newMessageParts[1];
                        var rowKey = newMessageParts[2];
                        Trace.TraceInformation("Processing image in blob '{0}'", imageBlobName);

                        string thumbnailName = System.Text.RegularExpressions.Regex.Replace(imageBlobName, "([^\\.]+)(\\.[^\\.]+)?$", "$1-thumb$2");
                        Trace.TraceInformation("Generated Thumbnail name = '{0}'", thumbnailName);

                        CloudBlockBlob inputBlob = _blobStorage.GetBlockBlobReference(imageBlobName);
                        CloudBlockBlob outputBlob = _blobStorage.GetBlockBlobReference(thumbnailName);

                        using(Stream inputStream = inputBlob.OpenRead())
                        using (Stream outputStream = outputBlob.OpenWrite())
                        {
                            processImage(inputStream, outputStream);

                            outputBlob.Properties.ContentType = inputBlob.Properties.ContentType;
                            string outputBlobUri = outputBlob.Uri.ToString();

                            // Update related Guestbook entry in the table storage to point to the thumbnail's Uri
                            GuestBookDataSource guestBookDataSource = new GuestBookDataSource();
                            guestBookDataSource.UpdateImageThumbnail(partitionKey, rowKey, outputBlobUri);

                            // Remove the message from queue
                            _queueStorage.DeleteMessage(newMessage);

                            Trace.TraceInformation("Generated thumbnail in blob '{0}'.", outputBlobUri);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Trace.TraceError("Exception when processing queue item. Message: '{0}'", exception.Message);
                    Thread.Sleep(5000);
                }

            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            bool storageInitialised = false;
            while (!storageInitialised)
            {
                try
                {
                    // Initialise Cloud Blob & Queue clients
                    string cloudStorageConfigSetting = CloudConfigurationManager.GetSetting(CLOUD_STORAGE_ACCOUNT_CONFIG_SETTING);
                    CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConfigSetting);
                    _blobStorage = BlobStorage.Create(cloudStorageAccount, CLOUD_BLOB_REFERENCE);
                    _queueStorage = QueueStorage.Create(cloudStorageAccount, CLOUD_QUEUE_REFERENCE);
                    storageInitialised = true;
                }
                catch (Exception exception)
                {
                    Trace.TraceError(exception.Message);
                    Thread.Sleep(5000);
                }
            }

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        private void processImage(Stream inputStream, Stream outputStream)
        {
            var originalImage = new Bitmap(inputStream);
            int thumbnailHeight;
            int thumbnailWidth; 

            // Define the thumbnail's dimension
            if (originalImage.Width > originalImage.Height)
            {
                thumbnailWidth = THUMBNAIL_SIZE;
                thumbnailHeight = THUMBNAIL_SIZE * originalImage.Height/originalImage.Width;
            }
            else
            {
                thumbnailWidth = THUMBNAIL_SIZE*originalImage.Width/originalImage.Height;
                thumbnailHeight = THUMBNAIL_SIZE;
            }

            // Create the Thumbnail's bitmap
            Bitmap thumbnailBitmap = null;
            try
            {
                thumbnailBitmap = new Bitmap(thumbnailWidth, thumbnailHeight);

                using (Graphics graphics = Graphics.FromImage(thumbnailBitmap))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(originalImage, 0, 0, thumbnailWidth, thumbnailHeight);
                }

                thumbnailBitmap.Save(outputStream, ImageFormat.Jpeg);
            }
            finally
            {
                if (thumbnailBitmap != null)
                {
                    thumbnailBitmap.Dispose();
                }
            }
        }
    }
}
