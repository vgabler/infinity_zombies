using UniRx;

namespace Game.Domain
{
    internal interface INicknamed
    {
        IReadOnlyReactiveProperty<string> Nickname { get; }
    }
}
