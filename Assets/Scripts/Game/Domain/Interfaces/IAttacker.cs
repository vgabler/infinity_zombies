using UnityEngine;

namespace Game
{
    public interface IAttacker
    {
        public void Attack();
        public void Attack(Vector3 direction);
    }
}