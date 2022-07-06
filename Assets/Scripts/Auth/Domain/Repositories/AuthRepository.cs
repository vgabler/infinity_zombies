using System.Threading.Tasks;

namespace Auth.Domain
{
    public interface IAuthRepository
    {
        public Task<UserInfo> GetCurrentUser();
    }
}
