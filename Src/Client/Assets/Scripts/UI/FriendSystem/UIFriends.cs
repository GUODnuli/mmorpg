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
        this.SekectedItem = item as UIFriendItem;
    }

    public void OnClickFriendAdd()
    {
        UIInputBox.Show("输入要添加的好友名称或ID", "添加好友")。OnSubmit += OnFriendAddSubmit;
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
            tips = "无法将自己添加为好友";
            return false;
        }

        FriendService.Instance.SendFriendAddRequest(friendId, friendNAme);
        return true;
    }
}
