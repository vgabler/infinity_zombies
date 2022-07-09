using Fusion;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkinPicker : NetworkBehaviour
    {
        public List<GameObject> skins = new List<GameObject>();

        [Networked(OnChanged = nameof(OnPropertyChanged))] int _skin { get; set; }
        public override void Spawned()
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            }

            _skin = Random.Range(0, skins.Count);
        }

        void PickSkin(int skin)
        {
            for (int i = 0; i < skins.Count; i++)
            {
                skins[i].SetActive(i == skin);
            }
        }

        static void OnPropertyChanged(Changed<SkinPicker> info)
        {
            info.Behaviour.PickSkin(info.Behaviour._skin);
        }
    }
}