using System.Threading.Tasks;

namespace InfinityZombies.Domain
{
    public interface IStartNewMatch
    {
        public Task Invoke();
    }

    public class StartNewMatchImpl : IStartNewMatch
    {
        IMatchService service;
        public StartNewMatchImpl(IMatchService service)
        {
            this.service = service;
        }

        public Task Invoke()
        {
            return service.StartNewMatch();
        }
    }
}