using System.Threading.Tasks;

namespace Auth.Domain
{
    public interface ISignIn
    {
        public Task<UserInfo> Invoke(string email, string password);
    }

    public class SignInImpl : ISignIn
    {
        readonly IAuthRepository repository;
        public SignInImpl(IAuthRepository repository)
        {
            this.repository = repository;
        }

        public Task<UserInfo> Invoke(string email, string password)
        {
            return repository.SignIn(email, password);
        }
    }
}
