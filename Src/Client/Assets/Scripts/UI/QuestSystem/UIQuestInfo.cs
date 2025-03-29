using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Reflection;
using Common.Data;
using Models;

public class UIQuestInfo : MonoBehaviour
{
    public Text title;
    public Text[] target;
    public Text description;
    public UIIconItem rewardItems;
    public Text experience;
    public Text gold;

    public void SetQuestInfo(Quest quest)
    {
        this.title.text = string.Format("[{0}]{1}",quest.Define.Type, quest.Define.Name);
        
        // fix me
        //for (int i = 1; i <= 3; i++)
        //{
        //    PropertyInfo targetProperty = quest.Define.GetType().GetProperty($"Target{i}");
        //    PropertyInfo idProperty = quest.Define.GetType().GetProperty($"Target{i}ID");
        //    PropertyInfo numProperty = quest.Define.GetType().GetProperty($"Target{i}Num");
        //}

        if (quest.Info == null)
        {
            this.description.text = quest.Define.Dialog;
        }
        else
        {
            if (quest.Info.Status == SkillBridge.Message.QuestStatus.Completed)
            {
                this.description.text = quest.Define.DialogFinish;
            }
            else
            {
                this.description.text = quest.Define.DialogIncomplete;
            }
        }

        this.gold.text = quest.Define.RewardGold.ToString();
        this.experience.text = quest.Define.RewardExp.ToString();

        foreach (var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
        {
            fitter.SetLayoutVertical();
        }
    }
}