using System.Threading.Tasks;

namespace Auth.Domain
{
    public interface ISignOut
    {
        public Task Invoke();
    }

    public class SignOutImpl : ISignOut
    {
        readonly IAuthRepository repository;
        public SignOutImpl(IAuthRepository repository)
        {
            this.repository = repository;
        }

        public Task Invoke()
        {
            return repository.SignOut();
        }
    }
}
