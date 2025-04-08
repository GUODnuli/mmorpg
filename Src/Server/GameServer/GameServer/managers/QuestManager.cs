using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class QuestManager
    {
        Character Owner;

        public QuestManager(Character Owner)
        {
            this.Owner = Owner;
        }

        public void GetQuestInfo(List<NQuestInfo> list)
        {
            foreach (var quest in this.Owner.Data.Quests)
            {
                list.Add(GetQuestInfo(quest));
            }
        }

        public NQuestInfo GetQuestInfo(TCharacterQuest quest)
        {
            return new NQuestInfo()
            {
                QuestId = quest.QuestId,
                QuestGuid = quest.Id,
                Status = (QuestStatus)quest.Status,
                Targets = new int[3]
                {
                    quest.Target1,
                    quest.Target2,
                    quest.Target3,
                }
            };
        }

        public Result AcceptQuest(NetConnection<NetSession> sender, int questId)
        {
            Character character = sender.Session.Character;
            QuestDefine quest;

            if (!DataManager.Instance.Quests.TryGetValue(questId, out quest))
            {
                sender.Session.Response.questAccept.Errormsg = "Quest not found";
                return Result.Failed;
            }

            var dbquest = DBService.Instance.Entities.CharacterQuests.Create();
            dbquest.QuestId = quest.ID;
            if (quest.Target1 == QuestTarget.None)
            {
                dbquest.Status = (int)QuestStatus.Completed;
            }
            else
            {
                dbquest.Status = (int)QuestStatus.InProgress;
            }

            sender.Session.Response.questAccept.Quest = this.GetQuestInfo(dbquest);
            character.Data.Quests.Add(dbquest);
            DBService.Instance.Save();
            return Result.Success;
        }
    }
}
