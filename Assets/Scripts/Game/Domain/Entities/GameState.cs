namespace Game.Domain
{
    public interface IGameState { }

    public struct GameStateRunning : IGameState
    {
        public float SecondsRemaining { get; }
        public GameStateRunning(float secondsRemaining)
        {
            SecondsRemaining = secondsRemaining;
        }
    }

    public struct GameStateStarting : IGameState
    {
        public float SecondsRemaining { get; }
        public GameStateStarting(float secondsRemaining)
        {
            SecondsRemaining = secondsRemaining;
        }
    }
    public struct GameStateEnded : IGameState { }
}