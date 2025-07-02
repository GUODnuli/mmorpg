using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using Managers;

public class UISetting : UIWindow
{
    public void ExitToCharacterSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        SoundManager.Instance.PlayerMusic(SoundDefine.Music_Select);
        UserService.Instance.SendGameLeave();
    }

    public void SystemConfig()
    {
        UIManager.Instance.Show<UISystemConfig>();
        this.Close();
    }

    public void ExitGame()
    {
        UserService.Instance.SendGameLeave(true);
    }
}
