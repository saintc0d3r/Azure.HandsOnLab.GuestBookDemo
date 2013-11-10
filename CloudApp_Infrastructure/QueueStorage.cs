using System;

using GuestBook_WebRole.Infrastructure.Cloud;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudApp_Infrastructure
{
    public class QueueStorage
    {
        private readonly CloudQueueClient _queueStorage;
        private readonly CloudQueue _queueReference;

        private QueueStorage(CloudQueueClient queueStorage, CloudQueue queueReference)
        {
            _queueStorage = queueStorage;
            _queueReference = queueReference;
        }

        public static QueueStorage Create(CloudStorageAccount storageAccount, string queueReference )
        {
            try
            {
                var queueStorage = storageAccount.CreateCloudQueueClient();
                CloudQueue queue = queueStorage.GetQueueReference(queueReference);
                queue.CreateIfNotExists();

                return new QueueStorage(queueStorage, queue);
            }
            catch (Exception exception)
            {
                throw new CloudStorageClientException(exception);
            }
        }

        public void QueueMessage(string message)
        {
            var messageToQueue = new CloudQueueMessage(message);
            _queueReference.AddMessage(messageToQueue);
        }

        public CloudQueueMessage GetNewMessage()
        {
            return _queueReference.GetMessage();
        }

        public void DeleteMessage(CloudQueueMessage messageToDelete)
        {
            _queueReference.DeleteMessage(messageToDelete);
        }
    }
}