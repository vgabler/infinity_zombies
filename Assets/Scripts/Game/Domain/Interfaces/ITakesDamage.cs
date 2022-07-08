using UniRx;

namespace Game.Domain
{
    public interface ITakesDamage
    {
        public IReadOnlyReactiveProperty<int> LastAttackerId { get; }
        public void TakeDamage(int value, int attackerId);
    }
}
