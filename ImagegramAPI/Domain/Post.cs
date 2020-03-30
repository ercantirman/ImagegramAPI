using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImagegramAPI.Domain
{
    public class Post
    {
        public long Id { get; private set; }

        public string ImageUrl { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public long CreatorId { get; private set; }
        [ForeignKey("CreatorId")]
        public virtual Account Creator { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public Post(string imageUrl, long creatorId)
        {
            ImageUrl = imageUrl;
            CreatorId = creatorId;
            CreatedAt = DateTime.Now;
        }
    }
}
