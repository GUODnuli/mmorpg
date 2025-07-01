using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
using Managers;
using Models;
using SkillBridge.Message;
using UnityEngine.UI;

public class UIRide : UIWindow
{
    public Text description;
    public GameObject itemPrefab;
    public ListView listMain;
    private UIRideItem selectedItem;

    private void Start()
    {
        RefreshUI();
        this.listMain.onItemSelected += this.OnItemSelected;
    }

    public void OnItemSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIRideItem;
        this.description.text = this.selectedItem.item.Define.Description;
    }

    private void RefreshUI()
    {
        ClearItems();
        InitItems();
    }

    private void InitItems()
    {
        foreach(var kv in ItemManager.Instance.Items)
        {
            var define = kv.Value.Define;
            if (define.Type == ItemType.Ride && (define.LimitClass == CharacterClass.None || define.LimitClass == User.Instance.CurrentCharacter.Class))
            {
                GameObject go = Instantiate(itemPrefab, this.listMain.transform);
                UIRideItem ui = go.GetComponent<UIRideItem>();
                ui.SetRideItem(kv.Value, this, false);
                this.listMain.AddItem(ui);
            }
        }
    }

    private void ClearItems()
    {
        this.listMain.RemoveAll();
    }

    public void DoRide()
    {
        if (this.selectedItem == null)
        {
            MessageBox.Show("请选择要召唤的坐骑", "提示");
        }
        User.Instance.Ride(selectedItem.item.Id);
    }
}
