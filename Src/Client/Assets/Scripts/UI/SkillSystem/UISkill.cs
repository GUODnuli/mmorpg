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
    public Text skillName;
    public Text skillDescription;
    public ListView skillList;

    private void Start()
    {
        this.skillList.onItemSelected += this.OnSkillSelected;
        //RefreshUI();
    }

    public void SetSkillInfo(SkillDefine skill)
    {
        this.title.text = skill.Name;
        if (skill.Description != null)
        {
            this.skillDescription.text = skill.Description;
        }

        foreach (var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
        {
            fitter.SetLayoutVertical();
        }
    }

    public void OnClickUpdate()
    {

    }

    public void OnSkillSelected(ListView.ListViewItem item)
    {
        UISkillItem skillItem = item as UISkillItem;

    }
}