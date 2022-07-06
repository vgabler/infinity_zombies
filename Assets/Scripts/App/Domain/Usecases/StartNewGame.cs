namespace InfinityZombies.Domain
{
    public interface IStartNewGame
    {
        public void Invoke();
    }

    public class StartNewGame : IStartNewGame
    {
        IGameService service;
        public StartNewGame(IGameService service)
        {
            this.service = service;
        }

        public void Invoke()
        {
            service.StartNewGame();
        }
    }
}