using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Candlelight.UI;
using Managers;
using UnityEngine.UI;

public class UIChat : MonoBehaviour
{
    public HyperText textArea; // ����������ʾ����
    public TabView channelTab;
    public InputField chatText; // ���������
    public Text chatTarget;
    public Dropdown channelSelect;

    // Start is called before the first frame update
    void Start()
    {
        //this.channelTab.OnTabSelect += OnDisplayChannelSelected;
        //ChatManager.Instance.OnChat += RefreshUI;
    }

    private void OnDestroy()
    {
        //ChatManager.Instance.OnChat -= RefreshUI;
    }

    private void Update()
    {
        InputManager.Instance.IsInputMode = chatText.isFocused;
    }

    void OnDisPlayChannelSelected(int idx)
    {
        //ChatManager.Instance.displayChannel = (ChatManager.LocalChannel)idx;
        RefreshUI();
    }

    public void RefreshUI()
    {

    }
}
