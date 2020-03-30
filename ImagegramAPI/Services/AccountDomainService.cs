using ImagegramAPI.Domain;
using ImagegramAPI.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;

namespace ImagegramAPI.Services
{
    public class AccountDomainService :  IAccountDomainService
    {
        private readonly IRepository<Account> _accountRepository;

        public AccountDomainService(IRepository<Account> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public bool CheckIfUserExistsFromHeader(HttpRequest request)
        {
            var accountId = GetCurrentAccountIdFromHeader(request);

            return _accountRepository.Get(accountId) != null;
        }

        public long GetCurrentAccountIdFromHeader(HttpRequest request)
        {
            var accountId = long.Parse(request.Headers["X-Account-Id"]);

            return accountId;
        }
    }
}
