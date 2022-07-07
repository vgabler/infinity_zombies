using Fusion;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Domain
{
    public class PlayersController : SimulationBehaviour, ISpawned, IPlayerJoined, IPlayerLeft
    {
        [SerializeField] NetworkPrefabRef _playerPrefab;
        readonly Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

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

            // Collect all spawn points in the scene.
            //_spawnPoints = FindObjectsOfType<SpawnPoint>();

            foreach (var player in Runner.ActivePlayers)
            {
                SpawnPlayer(player);
            }
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

            if (Object.HasStateAuthority == false || !(gameStateController.GameState.Value is GameStateRunning))
            {
                return;
            }

            //Verificar se todos os jogadores estão vivos

            var alive = 0;

            foreach (var c in _spawnedCharacters.Values)
            {
                var health = c.GetComponent<IHealth>();

                if (health == null || health.CurrentHealth <= 0)
                {
                    continue;
                }

                alive++;
            }

            //Se pelo menos 1 estiver vivo, o jogo continua
            if (alive > 0)
            {
                return;
            }

            //TODO reason?
            gameStateController.EndGame();
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

            var playerObj = Runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            Runner.SetPlayerObject(player, playerObj);

            // Keep track of the player avatars so we can remove it when they disconnect
            _spawnedCharacters.Add(player, playerObj);
        }

        /// <summary>
        /// Remove o objeto de um jogador (chamado on player left)
        /// </summary>
        /// <param name="player"></param>
        private void DespawnPlayer(PlayerRef player)
        {
            if (Object.HasStateAuthority == false) return;

            if (_spawnedCharacters.TryGetValue(player, out var networkObject))
            {
                Runner.Despawn(networkObject);
                _spawnedCharacters.Remove(player);
            }
        }

    }
}