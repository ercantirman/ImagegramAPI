using ImagegramAPI.Domain;
using ImagegramAPI.Infrastructure.Repository;
using ImagegramAPI.Models.Comment;
using ImagegramAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ImagegramAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    public class CommentController : ControllerBase
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IAccountDomainService _accountDomainService;

        public CommentController(IRepository<Comment> commentRepository, IAccountDomainService accountDomainService)
        {
            _commentRepository = commentRepository;
            _accountDomainService = accountDomainService;
        }

        [HttpPost]
        public IActionResult GetByPostId([FromBody]CommentRequestModel model)
        {
            if (!_accountDomainService.CheckIfUserExistsFromHeader(HttpContext.Request))
            {
                return Unauthorized();
            }
            else
            {
                var comments = _commentRepository.GetAll()
                .Where(p => p.PostId == model.PostId)
                .Include(q => q.Creator)
                .OrderBy(p => p.CreatedAt)
                .Skip((model.Page - 1) * model.Size)
                .Take(model.Size)
                .ToList();

                var outputModel = comments.Select(p => new CommentOutputModel
                {
                    Id = p.Id,
                    Content = p.Content,
                    PostId = p.PostId,
                    CreatorName = p.Creator.Name,
                    CreatorId = p.CreatorId,
                    CreatedAt = p.CreatedAt
                }).ToList();

                return Ok(outputModel);
            }
        }

                

        [HttpPost]
        public IActionResult Create([FromBody]CommentCreateModel model)
        {
            if (!_accountDomainService.CheckIfUserExistsFromHeader(HttpContext.Request))
            {
                return Unauthorized();
            }
            else
            {
                long creatorId = long.Parse(HttpContext.Request.Headers["X-Account-Id"]);
                var comment = new Comment(model.Content, creatorId, model.PostId);
                _commentRepository.Create(comment);
                return Ok();
            }
        }

        [HttpDelete]
        public IActionResult Delete(long commentId)
        {
            if (!_accountDomainService.CheckIfUserExistsFromHeader(HttpContext.Request))
            {
                return Unauthorized();
            }
            else
            {
                var currentAccountId = _accountDomainService.GetCurrentAccountIdFromHeader(HttpContext.Request);

                var commentCreatorId = _commentRepository.Get(commentId).CreatorId;

                if (currentAccountId == commentCreatorId)
                {
                    _commentRepository.Delete(commentId);

                    return Ok();
                }
                else
                {
                    return Forbid();
                }
            }
        }

        
    }
}
