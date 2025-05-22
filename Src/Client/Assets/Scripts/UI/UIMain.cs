using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Services;
using Managers;

public class UIMain : MonoSingleton<UIMain>
{
    public Text avatarName;
    public Text avatarLevel;
    public Image avatarImage;
    public UITeam TeamWindow;

    // Start is called before the first frame update
    protected override void OnStart()
    {
        this.UpdateAvatar();
    }

    private void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}", User.Instance.CurrentCharacter.Name);
        this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
    }

    public void BackToCharacterSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        UserService.Instance.SendGameLeave();
    }

    public void OnclickBag()
    {
        UIManager.Instance.Show<UIBag>();
    }

    public void OnClickCharEquip()
    {
        UIManager.Instance.Show<UICharEquip>();
    }

    public void OnClickQuestSystem()
    {
        UIManager.Instance.Show<UIQuestSystem>();
    }

    public void OnClickFriendSystem()
    {
        UIManager.Instance.Show<UIFriends>();
    }

    public void OnClickGuild()
    {
        //GuildManager.Instance.ShowGuild();
    }

    public void OnClickRide()
    {

    }

    public void OnClickSetting()
    {

    }

    public void OnClickSkill()
    {

    }

    public void ShowTeamUI(bool show)
    {
        TeamWindow.ShowTeam(show);
    }
}
