using Common.Data;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Services;

public class UISkill : UIWindow
{
    public Text title;
    public GameObject itemPrefab;
    public UISkillInfo skillInfo;
    public ListView skillList;

    private void Start()
    {
        this.skillList.onItemSelected += this.OnSkillSelected;
        RefreshUI();
    }

    public void OnSkillSelected(ListView.ListViewItem item)
    {
        UISkillItem skillItem = item as UISkillItem;
        this.skillInfo.SetSkillInfo(skillItem.skill);
    }

    private void RefreshUI()
    {
        CleanAllSkillList();
        InitAllSkillItems();
    }

    private void CleanAllSkillList()
    {
        this.skillList.RemoveAll();
    }

    private void InitAllSkillItems()
    {
        foreach(var skill in SkillManager.Instance.allSkill)
        {
            GameObject go = Instantiate(this.itemPrefab, this.skillList.transform);
            UISkillItem ui = go.GetComponent<UISkillItem>();
            ui.SetSkillInfo(skill.Value);
        }
    }
}