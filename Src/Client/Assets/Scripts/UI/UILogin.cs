using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;
using Managers;

public class UILogin : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public Button buttonLogin;
    public Toggle aggreToS;
    public Toggle rememberUsername;

    // Start is called before the first frame update
    void Start()
    {
        UserService.Instance.OnLogin = OnLogin;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLogin(Result result, string msg)
    {
        if (result == Result.Success)
        {
            SceneManager.Instance.LoadScene("CharSelect");
            SoundManager.Instance.PlayMusic(SoundDefine.Music_Select);
        }
        else
        {
            MessageBox.Show(msg, "错误", MessageBoxType.Error);
        }
    }

    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(this.username.text))
        {
            MessageBox.Show("请输入账号");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }
        if (!this.aggreToS.isOn)
        {
            MessageBox.Show("请阅读并同意用户协议");
            return;
        }
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        UserService.Instance.SendLogin(this.username.text, this.password.text);
    }
}
