using System.ComponentModel.DataAnnotations;
using AspNetMvc_Infrastructure;

namespace GuestBook_WebRole.Models
{
    public class GuestBookEntryModel
    {
        public GuestBookEntryModel()
        {
            Name = string.Empty;
            Comment = string.Empty;
        }
        
        [Required(ErrorMessage = "Please enter your name.")]
        [MapToModelProperty("GuestName")]
        public string Name { set; get; }

        [DataType(DataType.MultilineText)]
        [MapToModelProperty("Comment")]
        public string Comment { set; get; }

        //public byte[] PhotoData { set; get; }

        //public byte[] ThumbnailData { set; get; }
    }
}