using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using Network;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class SkillManager
    {
        readonly Character Owner;

        public Dictionary<int, Skill> Skills = new Dictionary<int, Skill>();

        public SkillManager(Character owner)
        {
            this.Owner = owner;
            foreach(var skill in owner.Data.Skills)
            {
                this.Skills.Add(skill.SkillDefID, new Skill(skill));
            }
        }

        public void GetSkillLists(List<NSkillInfo> list)
        {
            foreach (var skill in this.Owner.Data.Skills)
            {
                list.Add(GetSkillLists(skill));
            }
        }

        private NSkillInfo GetSkillLists(TCharacterSkill skill)
        {
            return new NSkillInfo()
            {
                SkillDefId = skill.SkillDefID
            };
        }

        public Result LearnSkill(NetConnection<NetSession> sender, int skillDefId)
        {
            if (this.Skills.TryGetValue(skillDefId, out Skill skill))
            {
                return Result.Failed;
            }
            else
            {
                TCharacterSkill dbCharacterSkill = new TCharacterSkill()
                {
                    SkillDefID = skillDefId,
                    Owner = Owner.Data,
                    TCharacterID = Owner.Data.ID,
                    SkillLevel = 1,
                };
                Owner.Data.Skills.Add(dbCharacterSkill);
                skill = new Skill(dbCharacterSkill);
                this.Skills.Add(skillDefId, skill);
            }
            Log.InfoFormat("Character [{0}] Learn new Skill: [{1}]", this.Owner.Data.ID, skill);
            return Result.Success;
        }

        public Result CastSkill(NetConnection<NetSession> sender, int skillDefId)
        {
            if (this.Skills.TryGetValue(skillDefId, out Skill skill))
            {
                return Result.Success;
            }
            else
            {
                return Result.Failed;
            }
        }
    }
}
