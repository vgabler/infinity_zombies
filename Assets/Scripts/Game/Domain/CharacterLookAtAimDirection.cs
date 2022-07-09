using Fusion;
using Game.Domain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CharacterLookAtAimDirection : NetworkBehaviour
    {
        public override void FixedUpdateNetwork()
        {
            if (GetInput(out PlayerNetworkInput data))
            {
                var dir = new Vector3(data.HorizontalFire, 0, data.VerticalFire).normalized;
                //TODO deveria pegar o transform por injection
                if (dir != Vector3.zero)
                    transform.forward = dir;
            }
        }
    }
}