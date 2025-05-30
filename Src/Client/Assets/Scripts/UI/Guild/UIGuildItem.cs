using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildItem : ListView.ListViewItem
{
    public Image backgroud;
    public Sprite normalBg;
    public Sprite selectedBg;

    private NGuildInfo _Info;
    public NGuildInfo Info { get { return _Info; } }

    public override void onSelected(bool selected)
    {
        this.backgroud.overrideSprite = selected ? normalBg : selectedBg;
    }

    public void SetGuildInfo(NGuildInfo guildInfo)
    {
        _Info = guildInfo;
    }
}