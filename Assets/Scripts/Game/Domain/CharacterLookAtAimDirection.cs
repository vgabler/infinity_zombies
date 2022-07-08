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
                //TODO deveria pegar o transform por injection
                transform.forward = new Vector3(data.HorizontalFire, 0, data.VerticalFire).normalized;
            }
        }
    }
}