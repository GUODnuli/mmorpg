using Common.Data;
using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

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
        public UnityAction<Quest> onQuestStatusChanged;

        public void Init(List<NQuestInfo> quests)
        {
            if (questInfos == null)
            {
                Debug.LogFormat("Error: questInfos is null");
            }
            questInfos = quests;
            allQuest.Clear();
            npcQuest.Clear();
            InitQuests();
        }

        private void InitQuests()
        {
            foreach (var info in questInfos)
            {
                Quest quest = new(info);
                allQuest.Add(quest.Info.QuestId, quest);
            }

            foreach (var kv in DataManager.Instance.Quests)
            {
                if (kv.Value.LimitClass != CharacterClass.None && kv.Value.LimitClass != User.Instance.CurrentCharacterInfo.Class)
                    continue;

                if (kv.Value.LimitLevel > User.Instance.CurrentCharacterInfo.Level)
                    continue;

                if (this.allQuest.ContainsKey(kv.Key))
                    continue;

                if (kv.Value.PreQuest > 0)
                {
                    if (this.allQuest.TryGetValue(kv.Value.PreQuest, out Quest preQuest))
                    {
                        if (preQuest.Info == null || preQuest.Info.Status == QuestStatus.Finished)
                            continue;
                    }
                    else
                        continue;
                }

                Quest quest = new(kv.Value);
                this.AddNpcQuest(quest.Define.AcceptNPC, quest);
                this.AddNpcQuest(quest.Define.SubmitNPC, quest);
                allQuest.Add(quest.Define.ID, quest);
            }
        }

        private void AddNpcQuest(int npcId, Quest quest)
        {
            if (!npcQuest.ContainsKey(npcId))
                npcQuest.Add(npcId, new Dictionary<NpcQuestStatus, List<Quest>>());

            
            if (!npcQuest[npcId].TryGetValue(NpcQuestStatus.Available, out List<Quest> availables))
            {
                if(availables == null)
                    availables = new List<Quest>();
                npcQuest[npcId].Add(NpcQuestStatus.Available, availables);
            }

            if (!npcQuest[npcId].TryGetValue(NpcQuestStatus.Completed, out List<Quest> completes))
            {
                if (completes == null)
                    completes = new List<Quest>();
                npcQuest[npcId].Add(NpcQuestStatus.Completed, completes);
            }

            if (!npcQuest[npcId].TryGetValue(NpcQuestStatus.Incomplete, out List<Quest> incompletes))
            {
                if (incompletes == null)
                    incompletes = new List<Quest>();
                npcQuest[npcId].Add(NpcQuestStatus.Incomplete, incompletes);
            }

            if (quest.Info == null)
            {
                if (quest.Define.AcceptNPC == npcId && !npcQuest[npcId][NpcQuestStatus.Available].Contains(quest))
                    npcQuest[npcId][NpcQuestStatus.Available].Add(quest);
            }
            else
            {
                if (quest.Define.SubmitNPC == npcId && quest.Info.Status == QuestStatus.Completed)
                    if (!npcQuest[npcId][NpcQuestStatus.Completed].Contains(quest))
                        npcQuest[npcId][NpcQuestStatus.Completed].Add(quest);
                if (quest.Define.SubmitNPC == npcId && quest.Info.Status == QuestStatus.InProgress)
                    if (!npcQuest[npcId][NpcQuestStatus.Incomplete].Contains(quest))
                        npcQuest[npcId][NpcQuestStatus.Incomplete].Add(quest);
            }
        }

        public NpcQuestStatus GetNpcQuestStatus(int npcId)
        {
            if (npcQuest.TryGetValue(npcId, out Dictionary<NpcQuestStatus, List<Quest>> quests))
            {
                if (quests[NpcQuestStatus.Available].Count > 0)
                    return NpcQuestStatus.Available;
                if (quests[NpcQuestStatus.Completed].Count > 0)
                    return NpcQuestStatus.Completed;
                if (quests[NpcQuestStatus.Incomplete].Count > 0)
                    return NpcQuestStatus.Incomplete;
            }
            return NpcQuestStatus.None;
        }

        public bool OpenNpcQuest(int npcId)
        {
            if (npcQuest.TryGetValue(npcId, out Dictionary<NpcQuestStatus, List<Quest>> npcQuestDic))
            {
                if (npcQuestDic[NpcQuestStatus.Completed].Count > 0)
                    return ShowQuestDialog(npcQuestDic[NpcQuestStatus.Completed].First());
                if (npcQuestDic[NpcQuestStatus.Available].Count > 0)
                    return ShowQuestDialog(npcQuestDic[NpcQuestStatus.Available].First());
                if (npcQuestDic[NpcQuestStatus.Incomplete].Count > 0)
                    return ShowQuestDialog(npcQuestDic[NpcQuestStatus.Incomplete].First());
            }
            return false;
        }

        private Quest RefereshQuestStatus(NQuestInfo quest)
        {
            this.npcQuest.Clear();
            Quest result;
            if (this.allQuest.ContainsKey(quest.QuestId))
            {
                this.allQuest[quest.QuestId].Info = quest;
                result = this.allQuest[quest.QuestId];
            }
            else
            {
                result = new Quest(quest);
                this.allQuest.Add(quest.QuestId, result);
            }

            foreach (var kv in DataManager.Instance.Quests)
            {
                if (kv.Value.LimitClass != CharacterClass.None && kv.Value.LimitClass != User.Instance.CurrentCharacterInfo.Class)
                    continue;

                if (kv.Value.LimitLevel > User.Instance.CurrentCharacterInfo.Level)
                    continue;

                if (this.allQuest.ContainsKey(kv.Key))
                    continue;

                if (kv.Value.PreQuest > 0)
                {
                    if (this.allQuest.TryGetValue(kv.Value.PreQuest, out Quest preQuest))
                    {
                        if (preQuest.Info == null || preQuest.Info.Status == QuestStatus.Finished)
                            continue;
                    }
                    else
                        continue;
                }

                Quest tmpQuest = new(kv.Value);
                this.AddNpcQuest(tmpQuest.Define.AcceptNPC, tmpQuest);
                this.AddNpcQuest(tmpQuest.Define.SubmitNPC, tmpQuest);
                allQuest.Add(tmpQuest.Define.ID, tmpQuest);
            }

            onQuestStatusChanged?.Invoke(result);
            return result;
        }

        private bool ShowQuestDialog(Quest quest)
        {
            if (quest.Info == null || quest.Info.Status == QuestStatus.Completed)
            {
                UIQuestDialog dlg = UIManager.Instance.Show<UIQuestDialog>();
                dlg.SetQuest(quest);
                dlg.OnClose += OnQuestDialogClose;
                return true;
            }
            if (quest.Info != null || quest.Info.Status == QuestStatus.InProgress)
            {
                if (!string.IsNullOrEmpty(quest.Define.DialogIncomplete))
                    MessageBox.Show(quest.Define.DialogIncomplete);
            }
            return true;
        }

        private void OnQuestDialogClose(UIWindow sender, UIWindow.WindowResult result)
        {
            UIQuestDialog dlg = (UIQuestDialog)sender;
            if (result == UIWindow.WindowResult.Yes)
            {
                MessageBox.Show(dlg.quest.Define.DialogAccept);
            }
            else if (result == UIWindow.WindowResult.No)
            {
                MessageBox.Show(dlg.quest.Define.DialogDeny);
            }
        }

        public void OnQuestAccepted(NQuestInfo info)
        {
            var quest = this.RefereshQuestStatus(info);
            MessageBox.Show(quest.Define.DialogAccept);
        }

        public void OnQuestSubmitted(NQuestInfo info)
        {
            var quest = this.RefereshQuestStatus(info);
            MessageBox.Show(quest.Define.DialogFinish);
        }
    }
}