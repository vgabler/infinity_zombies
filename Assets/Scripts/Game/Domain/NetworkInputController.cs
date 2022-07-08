using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using Game.Domain;


namespace Game
{
    /// <summary>
    /// Essa classe precisa estar dentro do personagem
    /// </summary>
    public class NetworkInputController : NetworkBehaviour, INetworkRunnerCallbacks
    {
        //TODO camera deveria vir por injection
        public Camera cam;
        public LayerMask groundMask;

        Vector3 dir;

        public override void Spawned()
        {
            base.Spawned();

            Runner.AddCallbacks(this);
        }
        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var playerObj = Runner.GetPlayerObject(Runner.LocalPlayer);

            if (playerObj == null)
            {
                Debug.LogWarning("Não tem playerobj para controlar");
                return;
            }

            var (success, position) = GetMouseWorldPosition();
            if (success)
            {
                dir = (position - playerObj.transform.position).normalized;

                dir.y = 0;
            }

            var data = new PlayerNetworkInput()
            {
                HorizontalMovement = Input.GetAxis("Horizontal"),
                VerticalMovement = Input.GetAxis("Vertical"),
                //Sempre atira para frente, então ignora o Y
                HorizontalFire = dir.x,
                VerticalFire = dir.z
            };

            data.Buttons.Set(PlayerButtons.Attack, Input.GetButton("Fire1"));

            input.Set(data);
        }

        (bool success, Vector3 position) GetMouseWorldPosition()
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
            {
                // The Raycast hit something, return with the position.
                return (success: true, position: hitInfo.point);
            }
            else
            {
                // The Raycast did not hit anything.
                return (success: false, position: Vector3.zero);
            }
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