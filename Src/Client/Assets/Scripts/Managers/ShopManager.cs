using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Services;

namespace Managers
{
    class ShopManager : Singleton<ShopManager>
    {
        public void Init()
        {
            NPCManager.Instance.RegisterNpcEvent(NpcFunction.InvokeShop, OnOpenShop);
        }

        private bool OnOpenShop(NPCDefine npcDefine)
        {
            this.ShowShop(npcDefine.Param);
            return true;
        }

        public void ShowShop(int shopId)
        {
            if (DataManager.Instance.Shops.TryGetValue(shopId, out ShopDefine shop))
            {
                UIShop uiShop = UIManager.Instance.Show<UIShop>();
                if(uiShop != null)
                {
                    uiShop.SetShop(shop);
                }
            }
        }

        //public bool BuyItem(int shopId, int shopItemId)
        //{
        //    ItemService.Instance.SendBuyItem(shopId, shopItemId);
        //    return true;
        //}
    }
}