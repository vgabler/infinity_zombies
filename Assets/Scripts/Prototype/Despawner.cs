using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityZombies.Prototype
{
    public class Despawner : NetworkBehaviour
    {
        public void Despawn()
        {
            Runner.Despawn(Object);
        }
    }
}