using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace App.Presentation
{
    public interface ISceneController
    {
        public Task ChangePage(string pageName);
    }

    public class SceneController : ISceneController
    {
        public async Task ChangePage(string pageName)
        {
            SceneManager.LoadSceneAsync(pageName);
            //TODO esperar a cena estar realmente carregada
            await Task.Delay(200);
        }
    }
}