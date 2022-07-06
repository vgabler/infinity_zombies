namespace InfinityZombies.Domain
{
    public interface IStartNewGame
    {
        public void Invoke();
    }

    public class StartNewGame : IStartNewGame
    {
        public void Invoke()
        {
            throw new System.NotImplementedException();
        }
    }
}