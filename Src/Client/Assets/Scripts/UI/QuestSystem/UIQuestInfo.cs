using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using Common.Data;
using Models;
using SkillBridge.Message;
using Managers;

public class UIQuestInfo : MonoBehaviour
{
    public Text title;
    public Text[] target;
    public Text description;
    public UIIconItem rewardItems;
    public Text experience;
    public Text gold;
    public Text overview;

    public Button navButton;
    private int npc = 0;

    public void SetQuestInfo(Quest quest)
    {
        this.title.text = string.Format("[{0}]{1}",quest.Define.Type, quest.Define.Name);
        if (this.overview != null) this.overview.text = quest.Define.Overview;
        
        // fix me
        //for (int i = 1; i <= 3; i++)
        //{
        //    PropertyInfo targetProperty = quest.Define.GetType().GetProperty($"Target{i}");
        //    PropertyInfo idProperty = quest.Define.GetType().GetProperty($"Target{i}ID");
        //    PropertyInfo numProperty = quest.Define.GetType().GetProperty($"Target{i}Num");
        //}

        if (this.description != null)
        {
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
        }

        this.gold.text = quest.Define.RewardGold.ToString();
        this.experience.text = quest.Define.RewardExp.ToString();

        if (quest.Info == null)
        {
            this.npc = quest.Define.AcceptNPC;
        }
        else if (quest.Info.Status == QuestStatus.Completed)
        {
            this.npc = quest.Define.SubmitNPC;
        }
        this.navButton.gameObject.SetActive(this.npc > 0);

        foreach (var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
        {
            fitter.SetLayoutVertical();
        }
    }

    public void OnClickNav()
    {
        Vector3 pos = NPCManager.Instance.GetNpcPosition(this.npc);
        User.Instance.CurrentCharacterObject.StartNav(pos);
        UIManager.Instance.Close<UIQuestSystem>();
    }
}