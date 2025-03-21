using Managers;
using Common.Data;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIEquipItem : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public Text title;
    public Text level;
    public Text limitClass;
    public Text limitCategory;

    public Image backgroud;
    public Sprite normalBg;
    public Sprite selectedBg;

    public int index { get; set; }
    private UICharEquip owner;

    private Item item;
    bool isEquiped = false;

    private bool selected;
    public bool Selected
    {
        get { return  selected; }
        set
        {
            selected = value;
            this.backgroud.overrideSprite = selected ? selectedBg : normalBg;
        }
    }

    public void SetEquipItem(int idx, Item item, UICharEquip owner, bool equiped)
    {
        this.owner = owner;
        this.index = idx;
        this.item = item;
        this.isEquiped = equiped;

        if (this.title != null) this.title.text = this.item.Define.Name;
        if (this.level != null) this.level.text = item.Define.Level.ToString();
        if (this.limitClass != null) this.limitClass.text = item.Define.LimitClass.ToString();
        if (this.limitCategory != null) this.limitCategory.text = item.Define.Category;
        if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.isEquiped) UnEquip();
        else
        {
            if (this.selected)
            {
                DoEquip();
                this.Selected = false;
            }
            else this.Selected = true;
        }
    }

    private void DoEquip()
    {
        var msg = MessageBox.Show(string.Format("要装备[{0}]吗？", this.item.Define.Name), "确认", MessageBoxType.Confirm);
        msg.OnYes = () =>
        {
            var oldEquip = EquipManager.Instance.GetEquip(item.EquipInfo.Slot);
            if (oldEquip != null)
            {
                var newmsg = MessageBox.Show(string.Format("当前已装备[{0}]，需要替换掉吗？", oldEquip.Define.Name), "确认", MessageBoxType.Confirm);
                newmsg.OnYes = () =>
                {
                    this.owner.UnEquip(oldEquip);
                };
            }
            this.owner.DoEquip(this.item);
        };
    }

    private void UnEquip()
    {
        var msg = MessageBox.Show(string.Format("要去下装备[{0}]吗？", this.item.Define.Name), "确认", MessageBoxType.Confirm);
        msg.OnYes = () =>
        {
            this.owner.UnEquip(this.item);
        };
    }
}