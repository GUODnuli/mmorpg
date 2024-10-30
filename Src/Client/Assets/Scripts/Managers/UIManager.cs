using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        class UIElement
        {
            public string Resource;
            public bool Cache;
            public GameObject Instance;
        }

        private Dictionary<Type, UIElement> UIResources = new Dictionary<Type, UIElement>();

        public UIManager()
        {
            //this.UIResources.Add(typeof(UITest)), new UIElement() { Resource = ""}
        }

        ~UIManager()
        {

        }

        /// <summary>
        /// Show UI
        /// </summary>
        public T Show<T>()
        {
            // SoundManager.Instance.PlaySound("ui_open");
            Type type = typeof(T);
            if (!this.UIResources.ContainsKey(type))
            {
                return default;
            }

            UIElement info = this.UIResources[type];
            if (info.Instance != null)
            {
                info.Instance.SetActive(true);
            }
            else
            {
                UnityEngine.Object prefab = Resources.Load(info.Resource);
                if (prefab == null)
                {
                    return default;
                }
                info.Instance = (GameObject)GameObject.Instantiate(prefab);
            }
            return info.Instance.GetComponent<T>();
        }

        public void Close(Type type)
        {
            // SoundManager.Instance.PlaySound("");
            if (!this.UIResources.ContainsKey(type))
            {
                return;
            }

            UIElement info = this.UIResources[type];
            if (info.Cache)
            {
                info.Instance.SetActive(false);
            }
            else
            {
                GameObject.Destroy(info.Instance);
                info.Instance = null;
            }
        }
    }
}
