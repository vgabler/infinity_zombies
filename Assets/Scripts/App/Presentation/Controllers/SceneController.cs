using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace InfinityZombies.Presentation.Controllers
{
    public interface ISceneController
    {
        public void ChangePage(string pageName);
    }

    public class SceneController : ISceneController
    {
        public void ChangePage(string pageName)
        {
            SceneManager.LoadSceneAsync(pageName);
        }
    }
}