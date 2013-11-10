using System;
using System.IO;

using GuestBook_WebRole.Infrastructure.Cloud;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CloudApp_Infrastructure
{
    public class BlobStorage
    {
        private readonly CloudBlobClient _blobStorage;
        private readonly CloudBlobContainer _containerReference;

        private BlobStorage(CloudBlobClient blobStorage, CloudBlobContainer containerReference)
        {
            _blobStorage = blobStorage;
            _containerReference = containerReference;
        }

        public static BlobStorage Create(CloudStorageAccount storageAccount, string containerReference, 
                                         BlobContainerPublicAccessType publicAccessPermission = BlobContainerPublicAccessType.Container)
        {
            try
            {
                // Get blob container
                var blobStorage = storageAccount.CreateCloudBlobClient();
                var container = blobStorage.GetContainerReference(containerReference);
                container.CreateIfNotExists();

                // Set blob container access permission
                var permission = container.GetPermissions();
                permission.PublicAccess = publicAccessPermission;
                container.SetPermissions(permission);

                return new BlobStorage(blobStorage, container);

            }
            catch (Exception exception)
            {
                throw new CloudStorageClientException(exception);
            }
        }

        public string UploadFromStream(string uniqueBlobName, string contentType, Stream inputStream)
        {
            CloudBlockBlob blockBlob = GetBlockBlobReference(uniqueBlobName);

            blockBlob.Properties.ContentType = contentType;
            blockBlob.UploadFromStream(inputStream);

            return blockBlob.Uri.ToString();
        }

        public CloudBlockBlob GetBlockBlobReference(string blobName)
        {
            return _containerReference.GetBlockBlobReference(blobName);
        }
    }
}