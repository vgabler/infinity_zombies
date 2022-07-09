using Fusion;
using UnityEngine;
using Zenject;

namespace Game.Domain
{
    public class PlayerSpawner : SimulationBehaviour, ISpawned, IPlayerJoined, IPlayerLeft
    {
        [SerializeField] NetworkPrefabRef playerPrefab;

        IGameStateController gameStateController;

        [Inject]
        public void Setup(IGameStateController gameStateController)
        {
            this.gameStateController = gameStateController;
        }

        /// <summary>
        /// Ao ser instanciado, já instancia os personagens dos jogadores. Podem ficar andando por ai enquanto o jogo não começa.
        /// </summary>
        public void Spawned()
        {
            if (Object.HasStateAuthority == false) return;

            //TODO Collect all spawn points in the scene.
            //_spawnPoints = FindObjectsOfType<SpawnPoint>();

            foreach (var player in Runner.ActivePlayers)
            {
                SpawnPlayer(player);
            }
        }

        public void PlayerJoined(PlayerRef player) { SpawnPlayer(player); }

        public void PlayerLeft(PlayerRef player) { DespawnPlayer(player); }

        /// <summary>
        /// Instancia um novo personagem, a não ser que o jogo já tenha terminado
        /// </summary>
        /// <param name="player"></param>
        private void SpawnPlayer(PlayerRef player)
        {
            if (Object.HasStateAuthority == false || gameStateController.GameState.Value is GameStateEnded)
            {
                return;
            }

            // Create a unique position for the player
            var spawnPosition = new Vector3(player.RawEncoded % Runner.Config.Simulation.DefaultPlayers * 3, 1, 0);

            var playerObj = Runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
            Runner.SetPlayerObject(player, playerObj);

        }

        /// <summary>
        /// Remove o objeto de um jogador (chamado on player left)
        /// </summary>
        /// <param name="player"></param>
        private void DespawnPlayer(PlayerRef player)
        {
            if (Object.HasStateAuthority == false) return;

            if (Runner.TryGetPlayerObject(player, out var networkObject))
            {
                Runner.Despawn(networkObject);
            }
        }
    }
}