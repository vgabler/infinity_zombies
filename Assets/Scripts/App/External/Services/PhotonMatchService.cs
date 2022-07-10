using Fusion;
using Fusion.Sockets;
using App.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace App.External
{
    public class PhotonMatchService : IMatchService, INetworkRunnerCallbacks
    {
        NetworkRunner runner;
        public PhotonMatchService() { }

        public Task ExitCurrentMatch()
        {
            LeaveSession();
            return Task.CompletedTask;
        }
        public Task StartNewMatch()
        {
            var seed = Guid.NewGuid().ToString();
            var sessionName = $"Game-{seed}";
            return StartGame(sessionName);
        }

        public Task JoinExistingMatch()
        {
            return StartGame(null);
        }

        async Task StartGame(string sessionName)
        {
            if (runner != null)
                LeaveSession();

            var go = new GameObject("Session");
            GameObject.DontDestroyOnLoad(go);

            runner = go.AddComponent<NetworkRunner>();
            runner.ProvideInput = true;
            runner.AddCallbacks(this);

            //Inicia no shared mode
            await runner.StartGame(new StartGameArgs { GameMode = GameMode.AutoHostOrClient, SessionName = sessionName, });

            //TODO deveria esperar carregar a cena direitinho
            //Deveria passar a cena certa em vez de número
            runner.SetActiveScene(1);

            await Task.Delay(200);
        }

        public Task RetryCurrentMatch()
        {
            return StartGame(runner.SessionInfo.Name);
        }

        void LeaveSession()
        {
            if (runner != null)
                runner.Shutdown();
            //else
            //    SetConnectionStatus(ConnectionStatus.Disconnected);
        }

        public void OnConnectedToServer(NetworkRunner runner) { }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

        public void OnDisconnectedFromServer(NetworkRunner runner) { }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

        public void OnInput(NetworkRunner runner, NetworkInput input) { }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
            //TODO chamar esse
        }

        public void OnSceneLoadStart(NetworkRunner runner) { }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    }
}
