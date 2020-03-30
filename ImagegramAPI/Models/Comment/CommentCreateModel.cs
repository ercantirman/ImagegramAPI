namespace ImagegramAPI.Models.Comment
{
    public class CommentCreateModel
    {
        public string Content { get; set; }

        public long PostId { get; set; }
    }
}
