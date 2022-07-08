using Game.Domain;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;
using UniRx;

namespace Game.Presentation
{
    public class PlayerHud : ReactiveMonoBehaviour
    {
        public Slider healthBar;
        public Text score;
        public Text nickname;
        public GameObject aliveAvatar;
        public GameObject deadAvatar;
        public GameObject isMineIndicator;

        public void Setup(PlayerEntity player)
        {
            //Remove qualquer listener que possuia antes
            Dispose();

            gameObject.SetActive(player != null);

            if (player == null)
            {
                return;
            }

            var health = player.Context.Container.Resolve<IHealth>();
            SubscribePropertyUpdateNow
            (
                //Combina o valor de Health com MaxHealth, retornando um valor de 0 a 1
                health.Health.CombineLatest(health.MaxHealth, (h, mh) => mh > 0 ? Mathf.Clamp01((float)h / mh) : 1.0f).ToReactiveProperty(),
                OnHealthPercentageChanged
            );

            SubscribePropertyUpdateNow(health.IsDead, OnIsDeadChanged);

            //Se for "meu" input authority, indicar
            var isMine = player.Object.HasInputAuthority;

            isMineIndicator.SetActive(isMine);
            if (isMine)
                transform.SetAsFirstSibling();

            //TODO
            //var scorer = playerContext.Container.Resolve<IScorer>();
            //SubscribePropertyUpdateNow(scorer.Score, OnScoreChanged);

            //TODO
            //var nicknamed = playerContext.Container.Resolve<INicknamed>();
            //SubscribePropertyUpdateNow(nicknamed.Nickname, OnNicknameChanged);
        }

        private void OnIsDeadChanged(bool isDead)
        {
            aliveAvatar.SetActive(!isDead);
            deadAvatar.SetActive(isDead);
        }

        private void OnHealthPercentageChanged(float healthPerc)
        {
            healthBar.value = healthPerc;
        }
    }
}