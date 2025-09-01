using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class SkillService : Singleton<SkillService>, IDisposable
    {
        public SkillService()
        {
            MessageDistributer.Instance.Subscribe<SkillLearnResponse>(this.OnSkillLearn);
            MessageDistributer.Instance.Subscribe<SkillCastResponse>(this.OnSkillCast);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<SkillLearnResponse>(this.OnSkillLearn);
            MessageDistributer.Instance.Unsubscribe<SkillCastResponse>(this.OnSkillCast);
        }

        public void Init()
        {

        }

        public void SendSkillLearn(int skillDefId)
        {
            Debug.LogFormat("SendSkillLearn: SkillDefId: {0}", skillDefId);
            NetMessage message = new();
            message.Request = new();
            message.Request.skillLearn = new SkillLearnRequest
            {
                SkillDefId = skillDefId
            };
            NetClient.Instance.SendMessage(message);
        }

        private void OnSkillLearn(object sender, SkillLearnResponse response)
        {
            if (response.Result == Result.Success)
            {
                MessageBox.Show(response.Errormsg, "技能习得");
            }
            else
            {
                MessageBox.Show(response.Errormsg, "技能学习失败");
            }
        }

        public void SendSkillCast(int skillDefId)
        {
            Debug.LogFormat("SendSkillCast: SkillDefId: {0}", skillDefId);
            NetMessage message = new();
            message.Request = new();
            message.Request.skillCast = new SkillCastRequest
            {
                SkillDefId = skillDefId
            };
            NetClient.Instance.SendMessage(message);
        }

        private void OnSkillCast(object sender, SkillCastResponse response)
        {
            if (response.Result == Result.Success)
            {
                MessageBox.Show(response.Errormsg, "技能释放成功");
            }
            else
            {
                MessageBox.Show(response.Errormsg, "技能释放失败");
            }
        }
    }
}
