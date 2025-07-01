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
            this.UIResources.Add(typeof(UISetting), new UIElement() { Resource = "UI/UISetting", Cache = true });
            this.UIResources.Add(typeof(UIBag), new UIElement() { Resource = "UI/UIBag", Cache = false});
            this.UIResources.Add(typeof(UIShop), new UIElement() { Resource = "UI/UIShop", Cache = false });
            this.UIResources.Add(typeof(UICharEquip), new UIElement() { Resource = "UI/UICharEquip", Cache = false });
            this.UIResources.Add(typeof(UIQuestSystem), new UIElement() { Resource = "UI/UIQuestSystem", Cache = false });
            this.UIResources.Add(typeof(UIQuestDialog), new UIElement() { Resource = "UI/UIQuestDialog", Cache = false });
            this.UIResources.Add(typeof(UIFriends), new UIElement() { Resource = "UI/UIFriends",Cache = false });
            this.UIResources.Add(typeof(UIGuild), new UIElement() { Resource = "UI/Guild/UIGuild", Cache = false });
            this.UIResources.Add(typeof(UIGuildList), new UIElement() { Resource = "UI/Guild/UIGuildList", Cache = false });
            this.UIResources.Add(typeof(UIGuildPopNoGuild), new UIElement() { Resource = "UI/Guild/UIGuildPopNoGuild", Cache = false });
            this.UIResources.Add(typeof(UIGuildPopCreate), new UIElement() { Resource = "UI/Guild/UIGuildPopCreate", Cache = false });
            this.UIResources.Add(typeof(UIGuildApplyList), new UIElement() { Resource = "UI/Guild/UIFuildApplyList", Cache = false });
            this.UIResources.Add(typeof(UIPopCharMenu), new UIElement() { Resource = "UI/UIPopCharMenu", Cache = false });
            this.UIResources.Add(typeof(UIRide), new UIElement() { Resource = "UI/Ride/UIRide", Cache = false });
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
                Canvas canvas = GameObject.FindObjectOfType<Canvas>();
                info.Instance = (GameObject)GameObject.Instantiate(prefab, canvas.transform);
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
