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
        if (this.selectedItem == null)
        {
            MessageBox.Show("请选择转让会长职位的成员");
            return;
        }

        MessageBox.Show(string.Format("确定要将会长转让给【{0}】吗？", this.selectedItem.Info.Info.Name), "转让会长", MessageBoxType.Confirm, "确定", "取消").OnYes = () => {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickPromotion()
    {
        if (this.selectedItem == null)
        {
            MessageBox.Show("请选择要晋升的成员。");
            return;
        }

        if (this.selectedItem.Info.Title != GuildTitle.None)
        {
            MessageBox.Show("对方已身份尊贵。");
            return;
        }

        MessageBox.Show(string.Format("确定要将【{0}】提升为副会长吗？", this.selectedItem.Info.Info.Name), "晋升", MessageBoxType.Confirm, "确定", "取消").OnYes = () => {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickDismiss()
    {
        if (this.selectedItem == null)
        {
            MessageBox.Show("请选择要罢免的成员。");
            return;
        }

        if (this.selectedItem.Info.Title == GuildTitle.None)
        {
            MessageBox.Show("所选成员无职位。");
            return;
        }

        if (this.selectedItem.Info.Title == GuildTitle.President)
        {
            MessageBox.Show("无法罢免会长。");
            return;
        }

        MessageBox.Show(string.Format("确定要罢免【{0}】的公会职务吗？", this.selectedItem.Info.Info.Name), "罢免", MessageBoxType.Confirm, "确定", "取消").OnYes = () => {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickApproveList()
    {
        UIManager.Instance.Show<UIGuildApplyList>();
    }

    public void OnClickDeport()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要踢出的成员。");
            return;
        }
        MessageBox.Show(string.Format("确定要将【{0}】踢出公会吗？", this.selectedItem.Info.Info.Name), "踢出公会", MessageBoxType.Confirm, "确定", "取消").OnYes = () => {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickQuit()
    {
        // fix me
    }

    //public void OnClickChat()
    //{

    //}
}