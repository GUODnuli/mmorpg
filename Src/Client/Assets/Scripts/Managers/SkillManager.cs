using Common.Data;
using Models;
using Services;
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
            }

            foreach(var kv in DataManager.Instance.Skills[(int)User.Instance.CurrentCharacter.Info.Class])
            {
                Skill skill = new Skill(kv.Value);
                allSkill.Add(skill.Define.ID, skill);
            }
        }

        public List<Skill> GetCharacterSkill()
        {
            return skillList;
        }

        public void SkillCast(int skillDefId)
        {
            //if ()
            //{
            //    MessageBox.Show("技能冷却中", "技能释放失败", MessageBoxType.Error);
            //    return;
            //}
            SkillService.Instance.SendSkillCast(skillDefId);
        }

        public void SkillLearn(int skillDefId)
        {
            SkillService.Instance.SendSkillLearn(skillDefId);
        }
    }
}