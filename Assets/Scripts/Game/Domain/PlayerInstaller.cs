using Game.Domain;
using Zenject;

namespace Game
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInstance(GetComponentInChildren<IScorer>()).AsSingle();
            Container.BindInstance(GetComponentInChildren<INicknamed>()).AsSingle();
        }
    }
}