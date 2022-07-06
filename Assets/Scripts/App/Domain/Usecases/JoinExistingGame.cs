using System.Threading.Tasks;

namespace InfinityZombies.Domain
{
    public interface IJoinExistingGame
    {
        public Task Invoke();
    }

    public class JoinExistingGame : IJoinExistingGame
    {
        IGameService service;
        public JoinExistingGame(IGameService service)
        {
            this.service = service;
        }

        public Task Invoke()
        {
            return service.JoinExistingGame();
        }
    }
}