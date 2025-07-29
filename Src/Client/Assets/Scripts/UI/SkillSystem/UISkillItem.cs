using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Common.Data;

public class UISkillItem : ListView.ListViewItem
{
    public Text skillName;
    public Image backgroud;
    public Sprite normalBg;
    public Sprite selectedBg;
    public Image skillIcon;
    public Text Level;

    public override void onSelected(bool selected)
    {
        this.backgroud.overrideSprite = selected ? normalBg : selectedBg;
    }

    public SkillDefine skillInfo;

    public void SetSkillInfo(SkillDefine item)
    {
        this.skillInfo = item;
        if (this.skillName != null)
        {
            this.skillName.text = this.skillInfo.Name;
        }
        if (this.skillIcon != null)
        {
            this.skillIcon.overrideSprite = Resloader.Load<Sprite>(this.skillInfo.Icon);
        }
        if (this.Level != null)
        {
            this.Level.text = this.skillInfo.UnlockLevel.ToString();
        }
    }
}