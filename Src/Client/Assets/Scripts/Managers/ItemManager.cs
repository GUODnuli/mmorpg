using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
using Models;
using SkillBridge.Message;

namespace Managers
{
    public class ItemManager : MonoSingleton<ItemManager>
    {
        public Dictionary<int, Item> Items = new Dictionary<int, Item>();
        internal void Init(List<NItemInfo> items)
        {
            this.Items.Clear();
            foreach(var info in items)
            {
                Item item = new Item(info);
                this.Items.Add(item.Id, item);
                Debug.LogFormat("IteManager: Init[{0}]", item);
            }
        }

        public ItemDefine GetItem(int itemId)
        {
            return null;
        }

        public bool UseItem(int itemId)
        {
            return false;
        }

        public bool UseItem(ItemDefine item)
        {
            return false;
        }
    }
}