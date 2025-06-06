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
            MessageBox.Show("��ѡ��ת�û᳤ְλ�ĳ�Ա");
            return;
        }

        MessageBox.Show(string.Format("ȷ��Ҫ���᳤ת�ø���{0}����", this.selectedItem.Info.Info.Name), "ת�û᳤", MessageBoxType.Confirm, "ȷ��", "ȡ��").OnYes = () => {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickPromotion()
    {
        if (this.selectedItem == null)
        {
            MessageBox.Show("��ѡ��Ҫ�����ĳ�Ա��");
            return;
        }

        if (this.selectedItem.Info.Title != GuildTitle.None)
        {
            MessageBox.Show("�Է���������");
            return;
        }

        MessageBox.Show(string.Format("ȷ��Ҫ����{0}������Ϊ���᳤��", this.selectedItem.Info.Info.Name), "����", MessageBoxType.Confirm, "ȷ��", "ȡ��").OnYes = () => {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickDismiss()
    {
        if (this.selectedItem == null)
        {
            MessageBox.Show("��ѡ��Ҫ����ĳ�Ա��");
            return;
        }

        if (this.selectedItem.Info.Title == GuildTitle.None)
        {
            MessageBox.Show("��ѡ��Ա��ְλ��");
            return;
        }

        if (this.selectedItem.Info.Title == GuildTitle.President)
        {
            MessageBox.Show("�޷�����᳤��");
            return;
        }

        MessageBox.Show(string.Format("ȷ��Ҫ���⡾{0}���Ĺ���ְ����", this.selectedItem.Info.Info.Name), "����", MessageBoxType.Confirm, "ȷ��", "ȡ��").OnYes = () => {
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
            MessageBox.Show("��ѡ��Ҫ�߳��ĳ�Ա��");
            return;
        }
        MessageBox.Show(string.Format("ȷ��Ҫ����{0}���߳�������", this.selectedItem.Info.Info.Name), "�߳�����", MessageBoxType.Confirm, "ȷ��", "ȡ��").OnYes = () => {
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