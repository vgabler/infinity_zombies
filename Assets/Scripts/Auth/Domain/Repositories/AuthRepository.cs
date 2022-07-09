using System.Threading.Tasks;

namespace Auth.Domain
{
    public interface IAuthRepository
    {
        public Task<UserInfo> GetCurrentUser();
        Task<UserInfo> SignIn(string email, string password);
        Task<UserInfo> SignUp(string email, string password, string nickname);
        Task SignOut();
    }
}
