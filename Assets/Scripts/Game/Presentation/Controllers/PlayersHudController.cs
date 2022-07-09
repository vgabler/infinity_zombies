using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Domain;
using Utils;

namespace Game.Presentation
{
    /// <summary>
    /// Habilita / desabilita huds de acordo com os jogadores entrando ou saindo
    /// </summary>
    public class PlayersHudController : ReactiveMonoBehaviour
    {
        public Transform container;
        public PlayerHud prefab;

        //Lista de entradas já instanciadas
        readonly List<PlayerHud> entries = new List<PlayerHud>();

        IEntityManager<PlayerEntity> entityManager;

        [Inject]
        public void Setup(IEntityManager<PlayerEntity> entityManager)
        {
            this.entityManager = entityManager;
            Subscribe(entityManager.Entities.ObserveCountChanged(true), (count) => UpdateEntries(entityManager.Entities));
        }

        void UpdateEntries(IEnumerable<PlayerEntity> players)
        {
            var i = 0;

            foreach (var p in players)
            {
                if (entries.Count <= i)
                {
                    entries.Add(Instantiate(prefab, container));
                }

                entries[i].Setup(p);
                i++;
            }

            //Desabilita os que não estão em uso
            while (i < entries.Count)
            {
                entries[i].Setup(null);
                i++;
            }
        }
    }
}