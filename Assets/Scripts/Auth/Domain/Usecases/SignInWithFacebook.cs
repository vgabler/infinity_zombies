using System.Threading.Tasks;

namespace Auth.Domain
{
    public interface ISignInWithFacebook
    {
        public Task<UserInfo> Invoke();
    }

    public class SignInWithFacebookImpl : ISignInWithFacebook
    {
        readonly IAuthRepository repository;
        public SignInWithFacebookImpl(IAuthRepository repository)
        {
            this.repository = repository;
        }

        public Task<UserInfo> Invoke()
        {
            return repository.SignInWithFacebook();
        }
    }
}