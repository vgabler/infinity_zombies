namespace InfinityZombies.Domain
{
    public interface IJoinExistingGame
    {
        public void Invoke();
    }

    public class JoinExistingGame : IJoinExistingGame
    {
        IGameService service;
        public JoinExistingGame(IGameService service)
        {
            this.service = service;
        }

        public void Invoke()
        {
            service.JoinExistingGame();
        }
    }
}