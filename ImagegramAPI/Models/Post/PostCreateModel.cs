using Microsoft.AspNetCore.Http;

namespace ImagegramAPI.Models.Post
{
    public class PostCreateModel
    {
        public IFormFile File { get; set; }
    }
}
