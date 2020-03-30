using ImagegramAPI.Domain;
using ImagegramAPI.Infrastructure.Repository;
using ImagegramAPI.Models.Comment;
using ImagegramAPI.Models.Post;
using ImagegramAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImagegramAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    public class PostController : ControllerBase
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IAccountDomainService _accountDomainService;
        public IConfiguration _configuration { get; }

        public PostController(IRepository<Post> postRepository, IAccountDomainService accountDomainService, IConfiguration configuration)
        {
            _postRepository = postRepository;
            _accountDomainService = accountDomainService;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult GetAll([FromBody]PostRequestModel model)
        {
            if (!_accountDomainService.CheckIfUserExistsFromHeader(HttpContext.Request))
            {
                return Unauthorized();
            }
            else
            {
                var posts = _postRepository.GetAll()
                .Include(p => p.Comments)
                .Include(p => p.Creator)
                .OrderBy(p => p.Comments.Count)
                .Skip((model.Page - 1) * model.Size)
                .Take(model.Size)
                .ToList();

                var outputModel = posts.Select(p => new PostOutputModel
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    CreatorName = p.Creator.Name,
                    CreatorId = p.CreatorId,
                    CreatedAt = p.CreatedAt,
                    LastThreeComments = p.Comments
                                        .OrderBy(p => p.CreatedAt)
                                        .Take(3)
                                        .Select(q => new CommentOutputModel
                                        {
                                            Id = q.Id,
                                            Content = q.Content,
                                            PostId = q.PostId,
                                            CreatorName = q.Creator.Name,
                                            CreatorId = q.CreatorId,
                                            CreatedAt = q.CreatedAt
                                        }).ToList()
                }).ToList();

                return Ok(outputModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm]PostCreateModel model)
        {
            if (!_accountDomainService.CheckIfUserExistsFromHeader(HttpContext.Request))
            {
                return Unauthorized();
            }
            else
            {
                long creatorId = _accountDomainService.GetCurrentAccountIdFromHeader(HttpContext.Request);

                try
                {
                    string filePath = await SaveImage(model.File);
                    ResizeImage(filePath);
                    string appRootAddress = _configuration.GetSection("AppRootAddress").Value;
                    string imageUrl = Path.Combine(appRootAddress, filePath);
                    var post = new Post(imageUrl, creatorId);
                    _postRepository.Create(post);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                return Ok();
            }
                
        }


        private async Task<string> SaveImage(IFormFile file)
        {
            var targetFolder = "Images";
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            List<string> acceptedExtensions = new List<string> { ".png", ".jpg", ".bmp" };

            if (!acceptedExtensions.Contains(fileExtension))
            {
                throw new Exception("File extension is not valid!");
            }

            var newFileName = Guid.NewGuid() + ".jpg";
            string targetPath = Path.Combine(targetFolder, newFileName);
            using (var fileStream = new FileStream(targetPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            string filePath = Path.Combine(targetFolder, newFileName);
            return filePath;
        }

        private void ResizeImage(string imgUrl)
        {
            using (Image image = Image.Load(imgUrl))
            {
                var resizeRatio = (decimal)image.Width / 640;
                int newHeight = (int)(image.Height / resizeRatio);
                image.Mutate(x => x.Resize(640, newHeight));
                image.Save(imgUrl);
            }
        }

    }
}
