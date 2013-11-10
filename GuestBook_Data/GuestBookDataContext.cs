using System.Linq;

using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;

namespace GuestBook_Data
{
    public class GuestBookDataContext : TableServiceContext
    {
        public GuestBookDataContext(CloudTableClient client) : base(client) { }

        public IQueryable<GuestBookEntry> GuestBookEntries
        {
            get { return CreateQuery<GuestBookEntry>(GuestBookEntry.ENTITY_NAME); }
        }

        public CloudTable GuestBookTable
        {
            get { return ServiceClient.GetTableReference(GuestBookEntry.ENTITY_NAME); }
        }

    }
}

