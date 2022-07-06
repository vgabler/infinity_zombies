using Fusion;
using InfinityZombies.Domain;
using System.Threading.Tasks;

namespace InfinityZombies.External
{
    public class PhotonMatchService : IMatchService
    {
        readonly NetworkRunner runner;

        public PhotonMatchService(NetworkRunner runner)
        {
            this.runner = runner;
        }

        public Task ExitCurrentMatch()
        {
            return runner.Shutdown();
        }

        public Task JoinExistingMatch()
        {
            return StartGame(GameMode.Client);
        }

        public Task StartNewMatch()
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

            //TODO deveria esperar carregar a cena direitinho
            //Deveria passar a cena certa em vez de número
            runner.SetActiveScene(1);

            await Task.Delay(200);
            //await sceneController.ChangePage(Constants.Pages.Game);
        }
    }
}
