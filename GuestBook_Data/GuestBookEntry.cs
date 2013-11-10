using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace GuestBook_Data
{
    public class GuestBookEntry : TableEntity
    {
        internal const string ENTITY_NAME = "GuestBookEntry";

        public GuestBookEntry()
        {
            PartitionKey = DateTime.UtcNow.ToString("MMddyyyy");
            RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid());
        }

        public string Comment {get;set;}

        public string GuestName { get; set; }

        public string PhotoUrl { set; get; }

        public string ThumbnailUrl { set; get; }

    }
}
