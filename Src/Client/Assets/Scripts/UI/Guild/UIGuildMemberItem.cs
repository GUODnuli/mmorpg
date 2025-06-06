using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.Utils;

public class UIGuildMemberItem : ListView.ListViewItem
{
    public Text nickname;
    public Text @class;
    public Text level;
    public Text title;
    public Text joinTime;
    public Text status;

    public Image backgroud;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        this.backgroud.overrideSprite = selected ? selectedBg : normalBg;
    }

    public NGuildMemberInfo Info;

    public void SetGuildMemberInfo(NGuildMemberInfo item)
    {
        Info = item;
        if (nickname != null) nickname.text = Info.Info.Name;
        if (@class != null) @class.text = Info.Info.Class.ToString();
        if (level != null) level.text = Info.Info.Level.ToString();
        if (title != null) title.text = Info.Title.ToString();
        if (joinTime != null) joinTime.text = TimeUtil.GetTime(this.Info.joinTime).ToShortDateString();
        if (status != null) status.text = Info.Status == 1 ? "����" : TimeUtil.GetTime(this.Info.lastTime).ToShortDateString();
    }
}
