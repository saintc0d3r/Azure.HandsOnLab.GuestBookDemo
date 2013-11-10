using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Mvc;

using AspNetMvc_Infrastructure;
using CloudApp_Infrastructure;
using GuestBook_Data;
using GuestBook_WebRole.Models;
using GuestBook_WebRole.PostResponses;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;

namespace GuestBook_WebRole.Controllers
{
    public class HomeController : Controller
    {
        private static readonly BlobStorage _blobStorage;
        private static readonly QueueStorage _queueStorage;
        private const string CLOUD_STORAGE_ACCOUNT_CONFIG_SETTING = "DataConnectionString";
        private const string CLOUD_BLOB_REFERENCE = "guestbookpics";
        private const string CLOUD_QUEUE_REFERENCE = "guestthumbs";
        private const string GUEST_BOOK_ENTRY_UNIQUE_BLOB_NAME_FORMAT = "guestbookpics/image_{0}{1}";

        static HomeController()
        {
            var cloudStorageConfigSetting = CloudConfigurationManager.GetSetting(CLOUD_STORAGE_ACCOUNT_CONFIG_SETTING);
            var cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConfigSetting);
            _blobStorage = BlobStorage.Create(cloudStorageAccount, CLOUD_BLOB_REFERENCE);
            _queueStorage = QueueStorage.Create(cloudStorageAccount, CLOUD_QUEUE_REFERENCE);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            return View();
        }

        [HttpGet]
        [RestoreModelStateFromTempData]
        public ActionResult Index()
        {
            ViewBag.SubmitResponse = TempData["SubmitResponse"];
            ViewBag.GuestBookEntries = retrieveGuestBookEntries();
            return View();
        }

        [HttpPost]
        [SetTempDataModelState]
        public ActionResult Submit(GuestBookEntryModel submittedGuestBookEntry, HttpPostedFileBase imageFileUpload)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            try
            {
                // Upload the image to blob storage
                string uniqueBlobName;
                var uploadedFileBlobUri = uploadImageFileToCloudBlobStorage(imageFileUpload, out uniqueBlobName);

                // Create a new entry in table storage
                string newEntryPartitionKey, newEntryRowKey;
                createNewEntryInCloudTableStorage(submittedGuestBookEntry.Name, submittedGuestBookEntry.Comment,
                                                  uploadedFileBlobUri, out newEntryPartitionKey, out newEntryRowKey);

                // Queue a message to process the stored image
                queueMessageToProcessStoredImage(uniqueBlobName, newEntryPartitionKey, newEntryRowKey);

                ModelState.Clear();
                TempData["SubmitResponse"] = new SubmitResponse
                    {
                        Message = "Data submission is success.",
                        IsSuccess = true
                    };
            }
            catch (Exception exception)
            {
                Trace.TraceError("Error message:'{0}'. Stack trace: '{1}'", exception.Message, exception.StackTrace);
                ModelState.AddModelError(string.Empty,
                                         string.Format("Submission Data is Failed. Reason: '{0}'", exception.Message));
                TempData["SubmitResponse"] = new SubmitResponse {Message = "Data submission is Failed."};
            }

            return RedirectToAction("Index");
        }

        #region --Helpers

        private static void queueMessageToProcessStoredImage(string uniqueBlobName, string newEntryPartitionKey,
                                                             string newEntryRowKey)
        {
            _queueStorage.QueueMessage(string.Format("{0},{1},{2}", uniqueBlobName, newEntryPartitionKey, newEntryRowKey));
            Trace.TraceInformation("Queued message to process blob '{0}'", uniqueBlobName);
        }

        private static void createNewEntryInCloudTableStorage(string name, string message, string uploadedFileBlobUri,
                                                              out string newEntryPartitionKey, out string newEntryRowKey)
        {
            var newGuestBookEntry = new GuestBookEntry
                {
                    GuestName = name,
                    Comment = message,
                    PhotoUrl = uploadedFileBlobUri,
                    ThumbnailUrl = uploadedFileBlobUri
                };
            var guestBookDataSource = new GuestBookDataSource();
            guestBookDataSource.AddGuestBookEntry(newGuestBookEntry);
            newEntryPartitionKey = newGuestBookEntry.PartitionKey;
            newEntryRowKey = newGuestBookEntry.RowKey;
            Trace.TraceInformation("Added entry {0}-{1} in table storage for guest '{2}'", newEntryPartitionKey,
                                   newEntryRowKey, newGuestBookEntry.GuestName);
        }

        private static string uploadImageFileToCloudBlobStorage(HttpPostedFileBase uploadedImageFile,
                                                                out string uniqueBlobName)
        {
            // Generate a unique blob name
            uniqueBlobName = string.Format(GUEST_BOOK_ENTRY_UNIQUE_BLOB_NAME_FORMAT, Guid.NewGuid(),
                                           Path.GetExtension(uploadedImageFile.FileName));

            // Call Upload from stream
            var blobUri = _blobStorage.UploadFromStream(uniqueBlobName, uploadedImageFile.ContentType,
                                                        uploadedImageFile.InputStream);

            Trace.TraceInformation("Uploaded image '{0}' to blob storage as '{1}'", uploadedImageFile.FileName,
                                   uniqueBlobName);

            return blobUri;
        }

        private IEnumerable<GuestBookEntryModel> retrieveGuestBookEntries()
        {
            var guestBookDataSource = new GuestBookDataSource();
            return guestBookDataSource.GetCurrentGuestBookEntries().ToMvcModels<GuestBookEntry, GuestBookItemModel>();
        }

        #endregion
    }
}
