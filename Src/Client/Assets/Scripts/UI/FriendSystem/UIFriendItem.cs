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


public class UIFriendItem : ListView.ListViewItem
{
    public Image backgroud;
    public Sprite normalBg;
    public Sprite selectedBg;

    private NFriendInfo _Info;

    public NFriendInfo Info
    {
        get { return _Info; }
    }

    public override void onSelected(bool selected)
    {
        this.backgroud.overrideSprite = selected ? normalBg : selectedBg;
    }

    public void SetFriendInfo(NFriendInfo friendInfo)
    {
        _Info = friendInfo;
    }
}