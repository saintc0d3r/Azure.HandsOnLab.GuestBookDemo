using AspNetMvc_Infrastructure;

namespace GuestBook_WebRole.Models
{
    public class GuestBookItemModel : GuestBookEntryModel
    {
        [MapToModelProperty("ThumbnailUrl")]
        public string ThumbnailUrl { set; get; }

        [MapToModelProperty("Timestamp")]
        public System.DateTimeOffset MessageTimestamp { set; get; }
    }
}