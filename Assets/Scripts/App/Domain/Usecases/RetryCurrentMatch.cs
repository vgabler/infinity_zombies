using System.Threading.Tasks;

namespace InfinityZombies.Domain
{
    public interface IRetryCurrentMatch
    {
        public Task Invoke();
    }

    public class RetryCurrentMatchImpl : IRetryCurrentMatch
    {
        IMatchService service;
        public RetryCurrentMatchImpl(IMatchService service)
        {
            this.service = service;
        }

        public Task Invoke()
        {
            return service.RetryCurrentMatch();
        }
    }
}