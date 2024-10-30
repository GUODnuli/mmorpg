using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using UnityEngine.UI;

namespace Managers
{
    public class MiniMapManager : MonoSingleton<MiniMapManager>
    {
        public UIMiniMap miniMap;
        private Collider miniMapBoundingBox;
        public Collider MiniMapBoundingBox
        {
            get { return miniMapBoundingBox; }
        }

        public Transform PlayerTransform
        {
            get
            {
                if (User.Instance.CurrentCharacterObject == null)
                {
                    return null;
                }
                return User.Instance.CurrentCharacterObject.transform;
            }
        }

        public Sprite LoadCurrentMiniMap()
        {
            return Resloader.Load<Sprite>("UI/MiniMap/" + User.Instance.CurrentMap.MiniMap);
        }

        public void UpdateMiniMap(Collider miniMapBoundingBox)
        {
            this.miniMapBoundingBox = miniMapBoundingBox;
            if (this.miniMapBoundingBox == null)
            {
                Debug.LogError("Error: The MiniMapBoundingBox is null.");
                return;
            }
            if (this.miniMap != null)
            {
                miniMap.UpdateMap();
            }
        }
    }
}