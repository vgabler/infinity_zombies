using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InfinityZombies.Prototype
{
    public class GameStarter : MonoBehaviour
    {
        public string startingScene = "Game";
        public NetworkRunner runner;

        async void StartGame(GameMode mode)
        {
            // Create the Fusion runner and let it know that we will be providing user input
            runner.ProvideInput = true;

            // Start or join (depends on gamemode) a session with a specific name
            await runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "TestRoom",
                //Scene = scene,
            });

            Debug.Log("Entrando na cena");

            runner.SetActiveScene(1);
        }

        public void Host()
        {
            StartGame(GameMode.Host);
        }
        public void Join()
        {
            StartGame(GameMode.Client);
        }
    }
}