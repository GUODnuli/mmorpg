using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Models;
using Services;
using SkillBridge.Message;

public class UIGuildApplyList : UIWindow
{
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;

    private void Start()
    {
        GuildService.Instance.OnGuildUpdate += UpdateList;
        GuildService.Instance.SendGuildListRequest();
        this.UpdateList();
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateList;
    }

    void UpdateList()
    {
        ClearList();
        InitItems();
    }

    private void ClearList()
    {
        this.listMain.RemoveAll();
    }

    private void InitItems()
    {
        foreach (var item in GuildManager.Instance.guildInfo.Applies)
        {

        }
    }
}
