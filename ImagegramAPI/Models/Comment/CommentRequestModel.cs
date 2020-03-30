using ImagegramAPI.Models.Common;

namespace ImagegramAPI.Models.Comment
{
    public class CommentRequestModel : Pagination
    {
        public long PostId { get; set; }
    }
}
