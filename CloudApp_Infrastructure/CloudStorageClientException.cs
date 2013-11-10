using System;

namespace GuestBook_WebRole.Infrastructure.Cloud
{
    public class CloudStorageClientException : Exception
    {
        private const string ERROR_MESSAGE = 
            "Storage services initialization failure. " 
            + "Check your storage account configuration settings. If running locally, " 
            + "ensure that the Development Storage service is running.";

        public CloudStorageClientException(Exception innerException) : base(ERROR_MESSAGE, innerException) {}
    }
}