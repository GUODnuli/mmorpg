using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Models;
using SkillBridge.Message;

namespace Managers
{
    class BagManager : Singleton<BagManager>
    {
        public int Unlocked;
        public BagItem[] Items;
        NBagInfo Info;

        unsafe public void Init(NBagInfo info)
        {
            this.Info = info;
            this.Unlocked = info.Unlocked;
            Items = new BagItem[this.Unlocked];
            if (info.Items != null && info.Items.Length >= this.Unlocked)
            {
                Analyze(info.Items);
            }
            else
            {
                Info.Items = new byte[sizeof(BagItem) * this.Unlocked];
                Reset();
            }
        }

        public void Reset()
        {
            int i = 0;
            foreach (var kv in ItemManager.Instance.Items)
            {
                if (kv.Value.Count <= kv.Value.Define.StackLimit)
                {
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)kv.Value.Count;
                }
                else
                {
                    int count = kv.Value.Count;
                    while (count > kv.Value.Define.StackLimit)
                    {
                        this.Items[i].ItemId = (ushort)kv.Key;
                        this.Items[i].Count = (ushort)kv.Value.Define.StackLimit;
                        i++;
                        count -= kv.Value.Define.StackLimit;
                    }
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)count;
                }
                i++;
            }
        }

        public void AddItem(int itemId, int count)
        {
            ushort remaining = (ushort)count;
            foreach (ref var slot in Items.AsSpan())
            {
                if (slot.ItemId != itemId) continue;

                ushort available = (ushort)(DataManager.Instance.Items[itemId].StackLimit - slot.Count);
                if (available <= 0) continue;

                ushort add = Math.Min(remaining, available);
                slot.Count += add;
                remaining -= add;

                if (remaining == 0) return;
            }

            foreach (ref var slot in Items.AsSpan())
            {
                if (slot.ItemId != 0) continue;

                ushort add = (ushort)Math.Min(remaining, (ushort)DataManager.Instance.Items[itemId].StackLimit);
                slot.ItemId = (ushort)itemId;
                slot.Count = add;
                remaining -= add;

                if (remaining == 0) break;
            }


            // fix me: Handing full inventory
            if (remaining > 0)
            {
                //背包空间不足，无法放入{remaining}个{item.Name};
            }
        }

        public void RemoveItem(int itemId, int count)
        {

        }

        unsafe void Analyze(byte[] data)
        {
            fixed (byte* pt = data)
            {
                for (int i = 0; i < this.Unlocked; i ++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    Items[i] = *item;
                }
            }
        }

        unsafe public NBagInfo GetBagInfo()
        {
            fixed (byte* pt = Info.Items)
            {
                for (int i = 0; i < this.Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    *item = Items[i];
                }
            }
            return this.Info;
        }
    }
}