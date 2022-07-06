using System.Threading.Tasks;

namespace InfinityZombies.Domain
{
    public interface IStartNewGame
    {
        public Task Invoke();
    }

    public class StartNewGame : IStartNewGame
    {
        IGameService service;
        public StartNewGame(IGameService service)
        {
            this.service = service;
        }

        public Task Invoke()
        {
            return service.StartNewGame();
        }
    }
}