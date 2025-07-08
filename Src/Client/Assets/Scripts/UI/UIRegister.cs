using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using Managers;

public class UIRegister : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public InputField passwordConfirm;
    public Button buttonRegister;
    public Toggle aggreToS;
    public Toggle rememberUsername;
    public GameObject uiLogin;

    // Start is called before the first frame update
    void Start()
    {
        UserService.Instance.OnRegister = this.OnRegister;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnRegister(SkillBridge.Message.Result result, string msg)
    {
        MessageBox.Show(string.Format("�����{0}, msg��{1}", result, msg));
    }

    public void OnClickRegister()
    {
        if (string.IsNullOrEmpty(this.username.text))
        {
            MessageBox.Show("�������˺�");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("����������");
            return;
        }
        if (string.IsNullOrEmpty(this.passwordConfirm.text))
        {
            MessageBox.Show("��ȷ������");
            return;
        }
        if (this.password.text != this.passwordConfirm.text)
        {
            MessageBox.Show("������������벻һ��");
            return;
        }
        if (!this.aggreToS.isOn)
        {
            MessageBox.Show("���Ķ���ͬ���û�Э��");
            return;
        }
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        UserService.Instance.SendRegister(this.username.text, this.password.text);
    }

    void CloseRegister()
    {
        this.gameObject.SetActive(false);
        uiLogin.SetActive(true);
    }
}
