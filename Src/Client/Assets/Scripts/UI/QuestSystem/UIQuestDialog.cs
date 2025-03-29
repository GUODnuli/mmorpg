using Models;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestDialog : UIWindow
{
    public UIQuestInfo questInfo;
    public Quest quest;
    public GameObject assignedBtns;
    public GameObject submitBtn;

    public void SetQuest(Quest quest)
    {
        this.quest = quest;
        this.UpdateQuest();
        if (this.quest.Info == null)
        {
            assignedBtns.SetActive(true);
            submitBtn.SetActive(false);
        }
        else
        {
            if (this.quest.Info.Status == SkillBridge.Message.QuestStatus.Completed)
            {
                assignedBtns.SetActive(false);
                submitBtn.SetActive(true);
            }
            else
            {
                assignedBtns.SetActive(false);
                submitBtn.SetActive(false);
            }
        }
    }

    private void UpdateQuest()
    {
        if (this.quest != null && this.questInfo != null)
        {
            this.questInfo.SetQuestInfo(this.quest);
        }
    }
}