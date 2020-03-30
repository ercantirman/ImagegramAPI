using Microsoft.AspNetCore.Http;

namespace ImagegramAPI.Services
{
    public interface IAccountDomainService
    {
        bool CheckIfUserExistsFromHeader(HttpRequest request);

        long GetCurrentAccountIdFromHeader(HttpRequest request);
    }
}
