using ImagegramAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagegramAPI.Models.Comment
{
    public class CommentOutputModel
    {
        public long Id { get; set; }

        public string Content { get; set; }

        public long PostId { get; set; }

        public string CreatorName { get; set; }

        public long CreatorId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
