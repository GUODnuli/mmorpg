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

    public void SetSkillInfo(SkillDefine skill)
    {
        this.title.text = skill.Name;
        if (skill.Description != null)
        {
            this.description.text = skill.Description;
        }

        foreach (var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
        {
            fitter.SetLayoutVertical();
        }
    }

    public void OnClickUpdate()
    {

    }
}