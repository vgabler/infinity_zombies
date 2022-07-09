using System.Threading.Tasks;

namespace Auth.Domain
{
    public interface ISignUp
    {
        public Task<UserInfo> Invoke(string email, string password, string nickname);
    }

    public class SignUpImpl : ISignUp
    {
        readonly IAuthRepository repository;
        public SignUpImpl(IAuthRepository repository)
        {
            this.repository = repository;
        }

        public Task<UserInfo> Invoke(string email, string password, string nickname)
        {
            return repository.SignUp(email, password, nickname);
        }
    }
}
