using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using Managers;

public class UIGuild : UIWindow
{
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;
    public UIGuildItem selectedItem;

    private void Start()
    {
        GuildService.Instance.OnGuildUpdate = UpdateUI;
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
        this.UpdateUI();
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate = null;
    }

    void UpdateUI()
    {
        this.uiInfo.Info = GuildManager.Instance.guildInfo;

        ClearList();
        InitItems();
    }

    public void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIGuildItem;
    }

    private void InitItems()
    {
        foreach (var item in GuildManager.Instance.guildInfo.Members)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
            ui.SetGuildMemberInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    private void ClearList()
    {
        this.listMain.RemoveAll();
    }

    public void OnClickTransferOfPresidency()
    {

    }

    public void OnClickPromotion()
    {

    }

    public void OnClickDismiss()
    {

    }

    public void OnClickApproveList()
    {

    }

    public void OnClickDeport()
    {

    }

    public void OnClickQuit()
    {

    }

    //public void OnClickChat()
    //{

    //}
}