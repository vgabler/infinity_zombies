using Fusion;

namespace Game.Domain
{
    public enum PlayerButtons
    {
        Attack
    }
    public struct PlayerNetworkInput : INetworkInput
    {
        public float HorizontalMovement;
        public float VerticalMovement;
        public float HorizontalFire;
        public float VerticalFire;
        public NetworkButtons Buttons;
    }
}