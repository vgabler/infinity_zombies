using Fusion;
using UnityEngine;

namespace InfinityZombies.Prototype
{
    public enum PlayerButtons
    {
        Attack
    }
    public struct PlayerNetworkInput : INetworkInput
    {
        public float Horizontal;
        public float Vertical;
        public NetworkButtons Buttons;
    }
}