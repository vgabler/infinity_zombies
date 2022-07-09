using Auth.Domain;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Zenject;

namespace InfinityZombies.Presentation
{
    public class RegisterScreenController : ReactiveMonoBehaviour
    {
        IAuthController authController;
        ISignUp signUp;

        public GameObject loadingIndicator;

        [Inject]
        public void Setup(IAuthController authController, ISignUp signUp)
        {
            this.authController = authController;
            this.signUp = signUp;
        }

        public async void TestRegister()
        {
            loadingIndicator.SetActive(true);
            var result = await Task.Run(() => signUp.Invoke("testMail@gmail.com", "123qwe", "Meu Nick"));
            loadingIndicator.SetActive(false);

            authController.UserChanged(result);

            if (result == null)
            {
                Debug.LogError("Falhou!");
                return;
            }

            Debug.LogError($"Deu certo! {result.Id} | {result.Nickname}");

            //SceneManager.LoadScene(Constants.Pages.Home);
        }
    }
}