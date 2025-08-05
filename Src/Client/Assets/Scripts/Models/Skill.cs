using Common.Data;
using Managers;
using SkillBridge.Message;

namespace Models
{
    public class Skill
    {
        public SkillDefine Define;
        public NSkillInfo Info;

        public Skill() { }

        public Skill(NSkillInfo skillInfo)
        {
            this.Info = skillInfo;
            this.Define = DataManager.Instance.Skills[(int)User.Instance.CurrentCharacterInfo.Class][skillInfo.SkillId];
        }

        public Skill(SkillDefine define)
        {
            this.Define = define;
            this.Info = null;
        }
    }
}