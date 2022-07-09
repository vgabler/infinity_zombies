using System.Threading.Tasks;

namespace Auth.Domain
{
    public interface IGetCurrentUser
    {
        public Task<UserInfo> Invoke();
    }
    public class GetCurrentUserImpl : IGetCurrentUser
    {
        readonly IAuthRepository repository;
        public GetCurrentUserImpl(IAuthRepository repository)
        {
            this.repository = repository;
        }

        async public Task<UserInfo> Invoke()
        {
            return await repository.GetCurrentUser();
        }
    }
}