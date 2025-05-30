using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Models;
using Services;

public class UIFriends : UIWindow
{
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIFriendItem selectedItem;

    private void Start()
    {
        FriendService.Instance.OnFriendUpdate = RefreshUI;
        this.listMain.onItemSelected += this.OnFriendSelected;
        RefreshUI();
    }

    public void OnFriendSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIFriendItem;
    }

    public void OnClickFriendAdd()
    {
        InputBox.Show("����Ҫ��ӵĺ������ƻ�ID", "��Ӻ���").OnSubmit += OnFriendAddSubmit;
    }

    private bool OnFriendAddSubmit(string input, out string tips)
    {
        tips = "";
        int friendId = 0;
        string friendName = "";
        if (!int.TryParse(input, out friendId))
        {
            friendName = input;
        }
        if (friendId == User.Instance.CurrentCharacter.Id || friendName == User.Instance.CurrentCharacter.Name)
        {
            tips = "�޷����Լ����Ϊ����";
            return false;
        }

        FriendService.Instance.SendFriendAddRequest(friendId, friendName);
        return true;
    }

    public void OnClickPrivateChat()
    {
        MessageBox.Show("��δ����");
    }

    public void OnClickFriendRemove()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("��ѡ��Ҫɾ���ĺ���");
            return;
        }

        MessageBox.Show(string.Format("ȷ��Ҫɾ������{0}��", selectedItem.Info.friendInfo.Name), "ɾ������", MessageBoxType.Confirm, "ɾ��", "ȡ��").OnYes = () =>
        {
            FriendService.Instance.SendFriendRemoveRequest(this.selectedItem.Info.friendInfo.Id);
        };


    }

    private void RefreshUI()
    {
        ClearFriendList();
        InitFriendList();
    }

    private void InitFriendList()
    {
        foreach (var item in FriendManager.Instance.allFriends)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIFriendItem ui = go.GetComponent<UIFriendItem>();
            ui.SetFriendInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    void ClearFriendList()
    {
        this.listMain.RemoveAll();
    }

    public void OnClickFriendTeamInvite()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("��ѡ��Ҫ����ĺ���");
            return;
        }

        if (selectedItem.Info.Status == 0)
        {
            MessageBox.Show("��ѡ�����ߵĺ���");
            return;
        }

        MessageBox.Show(string.Format("ȷ��Ҫ�������:{0}���������", selectedItem.Info.friendInfo.Name), "����������", MessageBoxType.Confirm, "����", "ȡ��").OnYes = () =>
        {
            TeamService.Instance.SendTeamInviteRequest(this.selectedItem.Info.friendInfo.Id, this.selectedItem.Info.friendInfo.Name);
        };
    }
}
