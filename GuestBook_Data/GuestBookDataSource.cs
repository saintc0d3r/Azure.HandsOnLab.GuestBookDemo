using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace GuestBook_Data
{
    // TODO: Refactor this as GuestBookEntryRepository
    public class GuestBookDataSource
    {
        private static readonly CloudStorageAccount storageAccount;
        private readonly GuestBookDataContext context;

        static GuestBookDataSource()
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            initialiseGuestBookEntryCloudTable();
        }

        public GuestBookDataSource()
        {
            context = new GuestBookDataContext(storageAccount.CreateCloudTableClient()); // TODO: Inject the instance using constructor injection
        }

        public IEnumerable<GuestBookEntry> GetCurrentGuestBookEntries()
        {
            #region // Original Hand lab's code:
            //CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            //CloudTable table = tableClient.GetTableReference("GuestBookEntry");

            //TableQuery<GuestBookEntry> query = new TableQuery<GuestBookEntry>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, DateTime.UtcNow.ToString("MMddyyyy")));

            //return table.ExecuteQuery(query);
            #endregion

            // Improvised code
            //return context.GuestBookEntries.Where(guestBookEntry => guestBookEntry.PartitionKey.Equals(DateTime.UtcNow.ToString("MMddyyyy"))).ToArray();
            return context.GuestBookEntries.ToArray().OrderByDescending(guestBookEntry => guestBookEntry.PartitionKey);
        }

        public void AddGuestBookEntry(GuestBookEntry newItem)
        {
            TableOperation operation = TableOperation.Insert(newItem);
            context.GuestBookTable.Execute(operation);
        }

        public void UpdateImageThumbnail(string partitionKey, string rowKey, string thumbUrl)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<GuestBookEntry>(partitionKey, rowKey);
            TableResult retrievedResult = context.GuestBookTable.Execute(retrieveOperation);
            GuestBookEntry updateEntity = retrievedResult.Result as GuestBookEntry;

            if (updateEntity != null)
            {
                updateEntity.ThumbnailUrl = thumbUrl;
                TableOperation replaceOperation = TableOperation.Replace(updateEntity);
                context.GuestBookTable.Execute(replaceOperation);
            }
        }

        private static void initialiseGuestBookEntryCloudTable()
        {
            CloudTableClient cloudTableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = cloudTableClient.GetTableReference(GuestBookEntry.ENTITY_NAME);
            table.CreateIfNotExists();
        }
    }
}
