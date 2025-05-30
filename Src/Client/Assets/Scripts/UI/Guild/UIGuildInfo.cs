using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using SkillBridge.Message;
using Common.Data;

public class UIGuildInfo : MonoBehaviour
{
    public Text guildName;
    public Text guildId;
    public Text leader;
    public Text notice;
    public Text memberNumber;

    private NGuildInfo info;
    public NGuildInfo Info
    {
        get { return info; }
        set { info = value; this.UpdateUI(); }
    }

    private void UpdateUI()
    {
        if (this.info == null)
        {
            guildName.text = "无";
            guildId.text = "ID: 0";
            leader.text = "会长: 无";
            notice.text = "";
            memberNumber.text = string.Format("成员数量: 0/{0}", 100);
        }
        else
        {
            guildName.text = Info.guildName;
            guildId.text = "ID: " + Info.Id;
            leader.text = "会长: " + Info.leaderName;
            notice.text = Info.Notice;
            memberNumber.text = string.Format("成员数量: {0}/{1}", Info.memberCount, 100);
        }
    }
}