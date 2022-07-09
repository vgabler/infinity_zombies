using UniRx;

namespace Game.Domain
{
    internal interface IScorer
    {
        IReadOnlyReactiveProperty<int> Score { get; }
        void AddScore(int scoreValue);
    }
}
