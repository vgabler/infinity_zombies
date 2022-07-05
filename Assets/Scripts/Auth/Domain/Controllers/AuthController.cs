using Auth.Domain.Entities;
using Auth.Domain.UseCases;
using System;
using UniRx;

namespace Auth.Controllers
{
    public interface IAuthController : IDisposable
    {
        public IReadOnlyReactiveProperty<bool> Initialized { get; }
        public IReadOnlyReactiveProperty<UserInfo> CurrentUser { get; }

        public void UserChanged(UserInfo userInfo);
    }

    public class AuthControllerImpl : IAuthController
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
            Initialize();
        }

        async void Initialize()
        {
            _currentUser.Value = await getCurrentUser.Invoke();
            _initialized.Value = true;
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
    }
}