using System.Threading.Tasks;

namespace InfinityZombies.Domain
{
    public interface IExitCurrentMatch
    {
        public Task Invoke();
    }

    public class ExitCurrentMatchImpl : IExitCurrentMatch
    {
        IMatchService service;
        public ExitCurrentMatchImpl(IMatchService service)
        {
            this.service = service;
        }

        public Task Invoke()
        {
            return service.ExitCurrentMatch();
        }
    }
}