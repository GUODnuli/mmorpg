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


public class UITeamItem : ListView.ListViewItem
{
    public Text nickname;
    public Image backgroud;
    public Image classIcon;
    public Image leaderIcon;

    public override void onSelected(bool selected)
    {
        this.backgroud.enabled = selected;
    }

    public int idx;

    public NCharacterInfo info;

    private void Start()
    {
        this.backgroud.enabled = false;
    }

    public void SetMemberInfo(int idx, NCharacterInfo item, bool isLeader)
    {
        this.idx = idx;
        this.info = item;
        if (this.nickname != null)
            this.nickname.text = this.info.Level.ToString().PadRight(4) + this.info.Name;
        if (this.classIcon != null)
            this.classIcon.overrideSprite = SpriteManager.Instance.classIcons[(int)this.info.Class - 1];
        if (this.leaderIcon != null)
            this.leaderIcon.gameObject.SetActive(isLeader);
    }
}