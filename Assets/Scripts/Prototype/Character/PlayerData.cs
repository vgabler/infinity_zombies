using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityZombies.Prototype
{
    public class PlayerNetworkedData : NetworkBehaviour
    {

        [HideInInspector][Networked(OnChanged = nameof(OnLivesChanged))] public int Lives { get; private set; }

        static void OnLivesChanged(Changed<PlayerNetworkedData> playerInfo)
        {
            //playerInfo.Behaviour._overviewPanel.UpdateLives(playerInfo.Behaviour.Object.InputAuthority, playerInfo.Behaviour.Lives);
        }


    }
}