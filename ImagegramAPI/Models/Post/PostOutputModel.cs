using ImagegramAPI.Models.Comment;
using ImagegramAPI.Models.Common;
using System;
using System.Collections.Generic;

namespace ImagegramAPI.Models.Post
{
    public class PostOutputModel
    {
        public long Id { get; set; }

        public string ImageUrl { get; set; }

        public string CreatorName { get; set; }

        public long CreatorId { get; set; }

        public List<CommentOutputModel> LastThreeComments { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
