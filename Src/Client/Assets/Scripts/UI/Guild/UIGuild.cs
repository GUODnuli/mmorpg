using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using Managers;
using SkillBridge.Message;

public class UIGuild : UIWindow
{
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;
    public UIGuildMemberItem selectedItem;

    public GameObject panelAdmin;
    public GameObject panelLeader;

    private void Start()
    {
        GuildService.Instance.OnGuildUpdate = UpdateUI;
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
        this.UpdateUI();
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateUI;
    }

    void UpdateUI()
    {
        this.uiInfo.Info = GuildManager.Instance.guildInfo;

        ClearList();
        InitItems();

        this.panelAdmin.SetActive(GuildManager.Instance.myMemberInfo.Title > GuildTitle.None);
        this.panelLeader.SetActive(GuildManager.Instance.myMemberInfo.Title == GuildTitle.President);
    }

    public void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIGuildMemberItem;
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
        UIManager.Instance.Show<UIGuildApproveList>();
    }

    public void OnClickDeport()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要踢出的成员。");
            return;
        }
        MessageBox.Show(string.Format("确定要将【{0}】踢出公会吗？", this.selectedItem.Info.Info.Name), "踢出公会", MessageBoxType.Confirm);
    }

    public void OnClickQuit()
    {
        // fix me
    }

    //public void OnClickChat()
    //{

    //}
}