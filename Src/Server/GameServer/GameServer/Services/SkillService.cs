using Common;
using Network;
using SkillBridge.Message;
using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class SkillService : Singleton<SkillService>
    {
        public SkillService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<SkillLearnRequest>(this.OnSkillLearn);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<SkillCastRequest>(this.OnSkillCast);
        }

        public void Init()
        {

        }

        private void OnSkillLearn(NetConnection<NetSession> sender, SkillLearnRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("SkillLearnRequest: Character: {0}, Skill id: {1}", character.Id, request.SkillDefId);

            var result = character.SkillManager.LearnSkill(sender, request.SkillDefId);
            if(result == Result.Failed)
            {
                Log.ErrorFormat("玩家: {0} 学习技能失败: {1}", character.Id, request.SkillDefId);
                sender.Session.Response.skillLearn.Result = result;
                sender.Session.Response.skillLearn.Errormsg = "习得失败，该技能你已经习得。";
            }
            else
            {
                sender.Session.Response.skillLearn.Result = result;
                sender.Session.Response.skillLearn.Errormsg = "技能学习成功。";
            }

            sender.SendResponse();
        }

        private void OnSkillCast(NetConnection<NetSession> sender, SkillCastRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("SkillCastRequest: Character: {0], Skill id: {1}", character.Id, request.SkillDefId);

            var result = character.SkillManager.CastSkill(sender, request.SkillDefId);
            if (result == Result.Success)
            {
                sender.Session.Response.skillLearn.Result = result;
                sender.Session.Response.skillLearn.Errormsg = "技能释放成功。";
            }
            else
            {
                sender.Session.Response.skillLearn.Result = result;
                sender.Session.Response.skillLearn.Errormsg = "技能释放失败。";
            }

            sender.SendResponse();
        }
    }
}
