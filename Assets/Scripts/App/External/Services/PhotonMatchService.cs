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
            return StartGame(GameMode.Host);
        }

        public Task JoinExistingMatch()
        {
            return StartGame(GameMode.Client);
        }

        async Task StartGame(GameMode mode)
        {
            if (runner != null)
                LeaveSession();

            GameObject go = new GameObject("Session");
            GameObject.DontDestroyOnLoad(go);

            runner = go.AddComponent<NetworkRunner>();
            runner.ProvideInput = true;
            runner.AddCallbacks(this);

            await runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "TestRoom", //TODO pegar o nome da session
            });

            //TODO deveria esperar carregar a cena direitinho
            //Deveria passar a cena certa em vez de número
            runner.SetActiveScene(1);

            await Task.Delay(200);
            //await sceneController.ChangePage(Constants.Pages.Game);
        }

        public Task RetryCurrentMatch()
        {
            //TODO só passar as mesmas configurações
            return StartGame(GameMode.AutoHostOrClient);
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

        public void OnSceneLoadDone(NetworkRunner runner) { }

        public void OnSceneLoadStart(NetworkRunner runner) { }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    }
}
