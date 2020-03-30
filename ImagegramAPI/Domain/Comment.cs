using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImagegramAPI.Domain
{
    public class Comment
    {
        public long Id { get; private set; }

        public string Content { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public virtual long CreatorId { get; private set; }
        [ForeignKey("CreatorId")]
        public Account Creator { get; set; }

        public virtual long PostId { get; private set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }

        public Comment(string content, long creatorId, long postId)
        {
            Content = content;
            CreatorId = creatorId;
            PostId = postId;
            CreatedAt = DateTime.Now;
        }
    }
}
