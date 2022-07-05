using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityZombies.Prototype
{

    public class BasicSpawner : SimulationBehaviour, IPlayerJoined, ISpawned, IPlayerLeft
    {
        [SerializeField] private NetworkPrefabRef _playerPrefab;
        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

        private bool _gameIsReady = false;

        GameStateController _gameStateController;

        // Spawns a new spaceship if a client joined after the game already started
        public void PlayerJoined(PlayerRef player)
        {
            if (_gameIsReady == false) return;
            SpawnSpaceship(player);
        }

        // Despawns the spaceship associated with a player when their client leaves the game session.
        public void PlayerLeft(PlayerRef player)
        {
            DespawnSpaceship(player);
        }

        private void DespawnSpaceship(PlayerRef player)
        {
            // Find and remove the players avatar
            if (_spawnedCharacters.TryGetValue(player, out var networkObject))
            {
                Runner.Despawn(networkObject);
                _spawnedCharacters.Remove(player);
            }
        }

        private void SpawnSpaceship(PlayerRef player)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3(player.RawEncoded % Runner.Config.Simulation.DefaultPlayers * 3, 1, 0);
            var networkPlayerObject = Runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);

            // Keep track of the player avatars so we can remove it when they disconnect
            _spawnedCharacters.Add(player, networkPlayerObject);

            //// Modulo is used in case there are more players than spawn points.
            //int index = player % _spawnPoints.Length;
            //var spawnPosition = _spawnPoints[index].transform.position;

            //var playerObject = Runner.Spawn(_spaceshipNetworkPrefab, spawnPosition, Quaternion.identity, player);
            //// Set Player Object to facilitate access across systems.
            //Runner.SetPlayerObject(player, playerObject);

            //// Add the new spaceship to the players to be tracked for the game end check.
            //_gameStateController.TrackNewPlayer(playerObject.GetComponent<PlayerDataNetworked>().Id);
        }

        public void Spawned()
        {
            if (Object.HasStateAuthority == false) return;

            // Collect all spawn points in the scene.
            //_spawnPoints = FindObjectsOfType<SpawnPoint>();
        }

        // The spawner is started when the GameStateController switches to GameState.Running.
        public void StartSpawner(GameStateController gameStateController)
        {
            _gameIsReady = true;
            _gameStateController = gameStateController;
            foreach (var player in Runner.ActivePlayers)
            {
                SpawnSpaceship(player);
            }
        }
    }
}