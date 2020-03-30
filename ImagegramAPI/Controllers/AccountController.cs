using ImagegramAPI.Domain;
using ImagegramAPI.Infrastructure.Repository;
using ImagegramAPI.Models.Account;
using ImagegramAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ImagegramAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<Comment> _commentRepository;
        private readonly IAccountDomainService _accountDomainService;
        public AccountController(IRepository<Account> accountRepository, IRepository<Post> postRepository, IRepository<Comment> commentRepository, IAccountDomainService accountDomainService)
        {
            _accountRepository = accountRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _accountDomainService = accountDomainService;
        }

        [HttpPost]
        public IActionResult GetAll([FromBody]AccountRequestModel model)
        {
            if (!_accountDomainService.CheckIfUserExistsFromHeader(HttpContext.Request))
            {
                return Unauthorized();
            }
            else
            {
                var accounts = _accountRepository.GetAll()
               .Skip((model.Page - 1) * model.Size)
               .Take(model.Size)
               .ToList();

                var outputModel = accounts.Select(p => new AccountOutputModel
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList();

                return Ok(outputModel);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody]AccountCreateModel model)
        {
            if (!_accountDomainService.CheckIfUserExistsFromHeader(HttpContext.Request))
            {
                return Unauthorized();
            }
            else
            {
                var account = new Account(model.Name);

                _accountRepository.Create(account);

                return Ok();
            }
        }

        [HttpDelete]
        public IActionResult DeleteMe()
        {
            if (!_accountDomainService.CheckIfUserExistsFromHeader(HttpContext.Request))
            {
                return Unauthorized();
            }
            else
            {
                var currentAccountId = _accountDomainService.GetCurrentAccountIdFromHeader(HttpContext.Request);

                var commentsRelatedToAccount = _commentRepository.GetAll().Where(p => p.CreatorId == currentAccountId).ToList();

                foreach (var comment in commentsRelatedToAccount)
                {
                    _commentRepository.Delete(comment.Id);
                }

                var postsRelatedToAccount = _postRepository.GetAll().Where(p => p.CreatorId == currentAccountId).ToList();

                foreach (var post in postsRelatedToAccount)
                {
                    _postRepository.Delete(post.Id);
                }

                _accountRepository.Delete(currentAccountId);

                return Ok();
            }
        }
    }
}
