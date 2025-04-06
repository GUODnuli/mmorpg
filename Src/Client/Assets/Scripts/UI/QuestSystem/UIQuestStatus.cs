using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

public class UIQuestStatus : MonoBehaviour
{
    public Image[] statusImage;
    private NpcQuestStatus npcQuestStatus;

    public void SetQuestStatus(NpcQuestStatus questStatus)
    {
        npcQuestStatus = questStatus;

        for (int i = 0; i < 4; i++)
        {
            if (statusImage[i] != null)
            {
                statusImage[i].gameObject.SetActive(i == (int)npcQuestStatus);
            }
        }
    }
}
