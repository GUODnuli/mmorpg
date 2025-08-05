using Common.Data;
using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class SkillManager : Singleton<SkillManager>
    {
        public List<NSkillInfo> skillInfos;
        public List<Skill> skillList;
        public Dictionary<int, Skill> allSkill = new Dictionary<int, Skill>();
        public Dictionary<Skill, SkillStatus> skillStatus = new Dictionary<Skill, SkillStatus>();

        public void Init(List<NSkillInfo> skills)
        {
            if (skillInfos == null)
            {
                Debug.LogFormat("Error: skillInfos is null");
            }
            skillInfos = skills;
            foreach(var skill in skills)
            {
                Skill item = new Skill(skill);
                skillList.Add(item);
                allSkill.Add(item.Info.SkillId, item);
                skillStatus.Add(item, item.Info.Status);
            }
        }

        public Dictionary<Skill, SkillStatus> GetSkillStatus()
        {
            return skillStatus;
        }

        public List<Skill> GetCharacterSkill()
        {
            return skillList;
        }
    }
}