using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

namespace InfinityZombies.Prototype
{
    public class InputHandler : MonoBehaviour, INetworkRunnerCallbacks
    {
        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new PlayerNetworkInput()
            {
                Horizontal = Input.GetAxis("Horizontal"),
                Vertical = Input.GetAxis("Vertical")
            };

            data.Buttons.Set(PlayerButtons.Attack, Input.GetButton("Fire1"));

            input.Set(data);
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnConnectedToServer(NetworkRunner runner) { }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

        public void OnDisconnectedFromServer(NetworkRunner runner) { }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

        public void OnSceneLoadDone(NetworkRunner runner) { }

        public void OnSceneLoadStart(NetworkRunner runner) { }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    }
}