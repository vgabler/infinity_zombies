using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Game.Domain;

namespace Game
{
    public interface IEntityManager<T>
    {
        public IReadOnlyReactiveCollection<T> Entities { get; }
        public void EntitySpawned(T entity);
        public void EntityDespawned(T entity);
    }

    public class BasicEntityManager<T> : IEntityManager<T>, IDisposable
    {
        public IReadOnlyReactiveCollection<T> Entities => entities;

        readonly ReactiveCollection<T> entities;

        public BasicEntityManager()
        {
            entities = new ReactiveCollection<T>();
        }

        public void EntitySpawned(T entity)
        {
            entities.Add(entity);
        }

        public void EntityDespawned(T entity)
        {
            entities.Remove(entity);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}