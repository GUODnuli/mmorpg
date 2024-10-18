using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Services;
using SkillBridge.Message;
using Managers;
using System;

public class UICharacterSelect : MonoBehaviour
{

    public GameObject panelCreate;
    public GameObject panelSelect;
    public GameObject btnCreateCancel;
    public InputField charName;
    CharacterClass charClass;
    public Transform uiCharList;
    public GameObject uiCharInfo;
    public List<GameObject> uiChars = new List<GameObject>();
    public List<UICharInfo> uiCharInfos = new List<UICharInfo>();
    public Image[] titles;
    public SwitchableButton[] buttons;
    public Text descs;
    private int currentSelectedIndex = -1;
    public UICharacterView characterView;

    // Use this for initialization
    private void Start()
    {
        DataManager.Instance.Load();
        InitCharacterSelect(true);
        UserService.Instance.OnCharacterCreate = OnCharacterCreate;
    }

    public void InitCharacterSelect(bool init)
    {
        panelCreate.SetActive(false);
        panelSelect.SetActive(true);

        if (init)
        {
            foreach (Transform child in uiCharList)
            {
                Destroy(child.gameObject);
            }
            uiChars.Clear();

            for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
            {
                GameObject go = Instantiate(uiCharInfo, this.uiCharList);
                UICharInfo charInfo = go.GetComponent<UICharInfo>();
                charInfo.info = User.Instance.Info.Player.Characters[i];

                Button button = go.GetComponent<Button>();
                int index = i;
                button.onClick.AddListener(() =>
                {
                    OnSelectCharacter(index);
                });

                uiChars.Add(go);
                go.SetActive(true);
            }

            InitializeCharacters();
            OnSelectCharacter(0);
        }
    }

    public void InitCharacterCreate()
    {
        characterView.CurrentCharacter = CharacterClass.Warrior;
        this.currentSelectedIndex = -1;
        panelCreate.SetActive(true);
        panelSelect.SetActive(false);
    }

    public void OnClickCreate()
    {
        if (string.IsNullOrEmpty(this.charName.text))
        {
            MessageBox.Show("请输入昵称");
            return;
        }
        UserService.Instance.SendCharacterCreate(this.charName.text, charClass);
    }

    public void OnSelectClass(int charClass)
    {
        this.charClass = (CharacterClass)charClass;

        characterView.CurrentCharacter = this.charClass;

        HideAllImages();
        titles[charClass - 1].gameObject.SetActive(true);

        descs.text = DataManager.Instance.Characters[charClass].Description;

        SetAllButtonUnselected();
        SetButtonState(buttons[charClass - 1], true);
    }

    private void OnCharacterCreate(Result result, string message)
    {
        if (result == Result.Success)
        {
            MessageBox.Show(message, "创建成功！");
            InitCharacterSelect(true);
        }
        else
        {
            MessageBox.Show(message, "创建失败！", MessageBoxType.Error);
        }
        panelCreate.SetActive(false);
        panelSelect.SetActive(true);
        SetHighlight(0);
    }

    public void OnSelectCharacter(int index)
    {
        if (User.Instance.Info.Player.Characters.Count == 0) 
        { 
            return;
        }
        if (index == this.currentSelectedIndex)
        {
            return;
        }

        var cha = User.Instance.Info.Player.Characters[index];
        Debug.LogFormat("Select Char: Character ID: {0}, Character Name: {1}, Character Class: {2}", cha.Id, cha.Name, cha.Class);
        User.Instance.CurrentCharacter = cha;
        characterView.CurrentCharacter = cha.Class;

        SetHighlight(index);

        this.currentSelectedIndex = index;
    }

    public void OnClickPlay()
    {
        if (currentSelectedIndex >= 0)
        {
            UserService.Instance.SendGameEnter(currentSelectedIndex);
        }
    }

    private void HideAllImages()
    {
        foreach (Image img in titles)
        {
            img.gameObject.SetActive(false);
        }
    }

    private void SetAllButtonUnselected()
    {
        foreach (SwitchableButton btn in buttons)
        {
            SetButtonState(btn, false);
        }
    }

    private void SetHighlight(int index)
    {
        if (this.currentSelectedIndex != -1 && this.currentSelectedIndex < uiCharInfos.Count)
        {
            uiCharInfos[this.currentSelectedIndex].Selected = false;
        }
        uiCharInfos[index].Selected = true;
    }

    private void SetButtonState(SwitchableButton btn, bool selected)
    {
        btn.button.image.sprite = selected ? btn.selectedSprite : btn.unselectedSprite;
    }

    public void InitializeCharacters()
    {
        uiCharInfos.Clear();
        foreach (GameObject charObject in uiChars)
        {
            UICharInfo charInfo = charObject.GetComponent<UICharInfo>();
            if (charInfo != null)
            {
                uiCharInfos.Add(charInfo);
            }
        }
    }

    
}
