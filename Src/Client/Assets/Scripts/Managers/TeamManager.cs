using SkillBridge.Message;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Models;

namespace Managers
{
    class TeamManager : Singleton<TeamManager>
    {
        public void Init()
        {

        }

        public void UpdateTeamInfo(NTeamInfo team)
        {
            User.Instance.TeamInfo = team;
            ShowTeamUI(team != null);
        }

        public void ShowTeamUI(bool show)
        {
            if (UIMain.Instance != null)
            {
                UIMain.Instance.ShowTeamUI(show);
            }
        }
    }
}