namespace InfinityZombies.Domain
{
    public interface IJoinExistingGame
    {
        public void Invoke();
    }

    public class JoinExistingGame : IJoinExistingGame
    {
        public void Invoke()
        {
            throw new System.NotImplementedException();
        }
    }
}