using System;
using System.Threading.Tasks;
using UniRx;
using Zenject;

namespace Auth.Domain
{
    public interface IAuthController
    {
        public IReadOnlyReactiveProperty<bool> Initialized { get; }
        public IReadOnlyReactiveProperty<UserInfo> CurrentUser { get; }
        public void UserChanged(UserInfo userInfo);
    }

    public class AuthControllerImpl : IAuthController, IInitializable, IDisposable
    {
        public IReadOnlyReactiveProperty<bool> Initialized => _initialized;
        public IReadOnlyReactiveProperty<UserInfo> CurrentUser => _currentUser;

        readonly ReactiveProperty<UserInfo> _currentUser;

        readonly ReactiveProperty<bool> _initialized;
        IGetCurrentUser getCurrentUser;

        public AuthControllerImpl(IGetCurrentUser getCurrentUser)
        {
            _currentUser = new ReactiveProperty<UserInfo>(null);
            _initialized = new ReactiveProperty<bool>(false);
            this.getCurrentUser = getCurrentUser;
        }

        public void UserChanged(UserInfo userInfo)
        {
            _currentUser.Value = userInfo;
        }

        public void Dispose()
        {
            _currentUser.Dispose();
            _initialized.Dispose();
        }

        public void Initialize()
        {
            //Inicializa em background
            Task.Run(Initialization);
        }

        public async Task Initialization()
        {
            var result = await getCurrentUser.Invoke();

            //Pra atualizar as variáveis precisa ser no main thread
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                _currentUser.Value = result;
                _initialized.Value = true;
            });
        }
    }
}