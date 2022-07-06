using Fusion;
using InfinityZombies.Domain;
using InfinityZombies.Presentation;
using System.Threading.Tasks;
using UnityEngine;

namespace InfinityZombies.External
{
    public class PhotonGameService : IGameService
    {
        readonly NetworkRunner runner;
        readonly ISceneController sceneController;

        public PhotonGameService(NetworkRunner runner, ISceneController sceneController)
        {
            this.runner = runner;
            this.sceneController = sceneController;
        }

        public Task JoinExistingGame()
        {
            return StartGame(GameMode.Client);
        }

        public Task StartNewGame()
        {
            return StartGame(GameMode.Host);
        }

        async Task StartGame(GameMode mode)
        {
            //TODO precisa levar isso pra ouro lugar
            runner.ProvideInput = true;

            await runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "TestRoom", //TODO pegar o nome do quarto
 
                //Scene = scene,
            });

            runner.SetActiveScene(1);

            await Task.Delay(200);
            //await sceneController.ChangePage(Constants.Pages.Game);
        }
    }
}
