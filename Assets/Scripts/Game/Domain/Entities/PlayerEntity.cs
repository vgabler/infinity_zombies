using Auth.Domain;
using Fusion;
using UniRx;
using Zenject;

namespace Game.Domain
{
    public class PlayerEntity : NetworkBehaviour, IScorer, INicknamed
    {
        IAuthController authController;
        IEntityManager<PlayerEntity> entityManager;
        public Context Context { get; private set; }

        //TODO tanto o score quanto o nickname deveriam estar em classes próprias
        [Networked(OnChanged = nameof(OnPropertyChanged))] int _score { get; set; }
        [Networked(OnChanged = nameof(OnPropertyChanged))] NetworkString<_16> _nickname { get; set; }

        public IReadOnlyReactiveProperty<int> Score => score;
        public IReadOnlyReactiveProperty<string> Nickname => nickname;
        readonly ReactiveProperty<int> score = new ReactiveProperty<int>();
        readonly ReactiveProperty<string> nickname = new ReactiveProperty<string>();

        [Inject]
        public void Setup(IEntityManager<PlayerEntity> entityManager, Context context, IAuthController authController)
        {
            this.entityManager = entityManager;
            this.Context = context;
            this.authController = authController;
        }

        public override void Spawned()
        {
            // --- Client
            // Find the local non-networked PlayerData to read the data and communicate it to the Host via a single RPC 
            if (Object.HasInputAuthority)
            {
                var nickname = authController.CurrentUser.Value.Nickname;
                RpcSetNickName(nickname);
            }


            base.Spawned();
            entityManager.EntitySpawned(this);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            entityManager.EntityDespawned(this);
        }

        public void AddScore(int scoreValue)
        {
            _score += scoreValue;
        }

        static void OnPropertyChanged(Changed<PlayerEntity> info)
        {
            info.Behaviour.score.Value = info.Behaviour._score;
            info.Behaviour.nickname.Value = info.Behaviour._nickname.Value;
        }

        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        private void RpcSetNickName(string nickname)
        {
            if (string.IsNullOrEmpty(nickname)) return;
            this._nickname = nickname;
        }

        void OnDestroy() { score.Dispose(); nickname.Dispose(); }
    }
}