using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using Common.Data;
using Models;
using SkillBridge.Message;
using Managers;

public class UISkillInfo : MonoBehaviour
{
    public Text title;
    public Text description;
    public Text descriptionNextLevel;
    public Text updateCost;
    public Button updateButton;
    public ListView listMain;
    public Skill skill;

    public void SetSkillInfo(Skill skill)
    {
        this.skill = skill;
        this.title.text = skill.Define.Name;
        if (skill.Define.Description != null)
        {
            this.description.text = skill.Define.Description;
        }

        foreach (var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
        {
            fitter.SetLayoutVertical();
        }
    }

    public void OnClickLearn()
    {
        SkillManager.Instance.SkillLearn(skill.Info.SkillDefId);
    }
}