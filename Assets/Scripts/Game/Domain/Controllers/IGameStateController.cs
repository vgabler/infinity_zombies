using UniRx;

namespace Game.Domain
{
    public interface IGameStateController
    {
        public IReadOnlyReactiveProperty<IGameState> GameState { get; }
    }
}
