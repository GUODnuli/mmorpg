using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Utils;
using GameServer.Entities;
using GameServer.Services;
using GameServer.Models;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class GuildManager : Singleton<GuildManager>
    {
        public Dictionary<int, Guild> Guilds = new Dictionary<int, Guild>();
        private HashSet<string> GuildNames = new HashSet<string>();

        public GuildManager() { }
        public void Init()
        {
            this.Guilds.Clear();
            foreach (var guild in DBService.Instance.Entities.TGuilds)
            {
                this.AddGuild(new Guild(guild));
            }
        }

        private void AddGuild(Guild guild)
        {
            this.Guilds.Add(guild.Id, guild);
            this.GuildNames.Add(guild.Name);
            guild.timestamp = TimeUtil.timestamp;
        }

        public bool CheckNameExisted(string name)
        {
            return this.GuildNames.Contains(name);
        }

        public bool CreateGuild(string name, string notice, Character leader)
        {
            DateTime now = DateTime.Now;
            TGuild dbGuild = DBService.Instance.Entities.TGuilds.Create();
            dbGuild.Name = name;
            dbGuild.Notice = notice;
            dbGuild.LeaderID = leader.Id;
            dbGuild.LeaderName = leader.Name;
            dbGuild.CreateTime = (long)TimeUtil.GetTimestamp(now);
            DBService.Instance.Entities.TGuilds.Add(dbGuild);

            Guild guild = new Guild(dbGuild);
            guild.AddMember(leader.Id, leader.Name, leader.Data.Class, leader.Data.Level, GuildTitle.President);
            leader.Guild = guild;
            DBService.Instance.Save();
            leader.Data.GuildId = dbGuild.Id;
            this.AddGuild(guild);
            return true;
        }

        internal Guild GetGuild(int guildId)
        {
            if (guildId == 0)
                return null;
            this.Guilds.TryGetValue(guildId, out Guild guild);
            return guild;
        }

        internal List<NGuildInfo> GetGuildsInfo()
        {
            List<NGuildInfo> result = new List<NGuildInfo>();
            foreach (var kv in this.Guilds)
            {
                result.Add(kv.Value.GuildInfo(null));
            }
            return result;
        }
    }
}
