using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using Managers;
using Network;
using SkillBridge.Message;

namespace Services
{
    public class QuestService : Singleton<QuestService>, IDisposable
    {
        public QuestService()
        {
            MessageDistributer.Instance.Subscribe<QuestAcceptResponse>(this.OnQuestAccept);
            MessageDistributer.Instance.Subscribe<QuestSubmitResponse>(this.OnQuestSubmit);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<QuestAcceptResponse>(this.OnQuestAccept);
            MessageDistributer.Instance.Unsubscribe<QuestSubmitResponse>(this.OnQuestSubmit);
        }

        public bool SendQuestAccept(Quest quest)
        {
            Debug.Log("SendQuestAccept");
            NetMessage msg = new();
            msg.Request = new();
            msg.Request.questAccept = new();
            msg.Request.questAccept.QuestId = quest.Define.ID;
            NetClient.Instance.SendMessage(msg);
            return true;
        }

        private void OnQuestAccept(object sender, QuestAcceptResponse message)
        {
            Debug.LogFormat("OnQuestAccept Result: {0}, Error: {1}", message.Result, message.Errormsg);
            if (message.Result == Result.Success)
            {
                QuestManager.Instance.OnQuestAccepted(message.Quest);
            }
            else
            {
                MessageBox.Show("任务接受失败","错误", MessageBoxType.Error);
            }
        }

        public bool SendQuestSubmit(Quest quest)
        {
            Debug.Log("SendQuestSubmit");
            NetMessage msg = new();
            msg.Request = new();
            msg.Request.questSubmit = new();
            msg.Request.questSubmit.QuestId = quest.Define.ID;
            NetClient.Instance.SendMessage(msg);
            return true;
        }

        private void OnQuestSubmit(object sender, QuestSubmitResponse message)
        {
            Debug.LogFormat("OnQuestSubmit Result: {0}, Error: {1}", message.Result, message.Errormsg);
            if (message.Result == Result.Success)
            {
                QuestManager.Instance.OnQuestSubmitted(message.Quest);
            }
            else
            {
                MessageBox.Show("任务提交失败", "错误", MessageBoxType.Error);
            }
        }
    }

}
