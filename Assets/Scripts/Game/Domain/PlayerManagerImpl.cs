using Fusion;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Domain
{
    public interface IPlayerManager
    {
        public IReadOnlyDictionary<int, GameObjectContext> Players { get; }
    }

    public class PlayerManagerImpl : SimulationBehaviour, ISpawned, IPlayerJoined, IPlayerLeft, IPlayerManager
    {
        [SerializeField] NetworkPrefabRef _playerPrefab;

        IGameStateController _gameStateController;

        public IReadOnlyDictionary<int, GameObjectContext> Players => _players;

        readonly Dictionary<int, GameObjectContext> _players = new Dictionary<int, GameObjectContext>();

        [Inject]
        public void Setup(IGameStateController gameStateController)
        {
            _gameStateController = gameStateController;
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

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

            if (Object.HasStateAuthority == false || !(_gameStateController.GameState.Value is GameStateRunning))
            {
                return;
            }

            //Verificar se todos os jogadores estão vivos

            var alive = 0;

            foreach (var c in _players.Values)
            {
                var health = c.Container.Resolve<IHealth>();

                if (health == null || health.IsDead.Value == true)
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
            _gameStateController.EndGame();
        }

        public void PlayerJoined(PlayerRef player) { SpawnPlayer(player); }

        public void PlayerLeft(PlayerRef player) { DespawnPlayer(player); }

        /// <summary>
        /// Instancia um novo personagem, a não ser que o jogo já tenha terminado
        /// </summary>
        /// <param name="player"></param>
        private void SpawnPlayer(PlayerRef player)
        {
            if (Object.HasStateAuthority == false || _gameStateController.GameState.Value is GameStateEnded)
            {
                return;
            }

            if (Players.ContainsKey(player))
            {
                Debug.LogError("Tentando spawn um player que já existe!");
                return;
            }

            // Create a unique position for the player
            var spawnPosition = new Vector3(player.RawEncoded % Runner.Config.Simulation.DefaultPlayers * 3, 1, 0);

            var playerObj = Runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            Runner.SetPlayerObject(player, playerObj);

            //Sempre deve ter o mesmo indice nas duas listas
            _players.Add(player, playerObj.GetComponentInChildren<GameObjectContext>());
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
                _players.Remove(player);
            }
        }
    }
}