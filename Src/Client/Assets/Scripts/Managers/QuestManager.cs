using Common.Data;
using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    public enum NpcQuestStatus
    {
        None = 0, // 无任务
        Completed, // 存在已完成任务
        Available, // 存在可接取任务
        Incomplete, // 存在未完成任务
    }

    public class QuestManager : Singleton<QuestManager>
    {
        public List<NQuestInfo> questInfos;
        public Dictionary<int, Quest> allQuest = new Dictionary<int, Quest>();
        public Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>> npcQuest = new Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>>();

        public void Init(List<NQuestInfo> quests)
        {
            if (questInfos == null)
            {
                Debug.LogFormat("Error: questInfos is null");
            }
            questInfos = quests;
            allQuest.Clear();
            npcQuest.Clear();
            InitQuest();
        }

        private void InitQuest()
        {
            foreach (var info in questInfos)
            {
                Quest quest = new Quest(info);
                this.AddNpcQuest(quest.Define.AcceptNPC, quest);
                this.AddNpcQuest(quest.Define.SubmitNPC, quest);
                allQuest.Add(quest.Info.QuestId, quest);
            }
        }

        private void AddNpcQuest(int npcId, Quest quest)
        {
            if (!npcQuest.ContainsKey(npcId))
            {
                Dictionary<NpcQuestStatus, List<Quest>> npcQuestDic = new Dictionary<NpcQuestStatus, List<Quest>>();

                npcQuest.Add(npcId, npcQuestDic);
            }
        }
    }
}