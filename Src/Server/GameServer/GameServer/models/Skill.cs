using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    class Skill
    {
        TCharacterSkill dbSkill;

        public int SkillDefID;
        public int SkillLevel;

        public Skill(TCharacterSkill skill)
        {
            this.dbSkill = skill;
            this.SkillDefID = skill.SkillDefID;
            this.SkillLevel = skill.SkillLevel;
        }

        public void Update()
        {
            this.SkillLevel += 1;
            dbSkill.SkillLevel = this.SkillLevel;
        }
    }
}
