using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using GameServer.Managers;
using SkillBridge.Message;
using Common.Utils;
using GameServer.Services;

namespace GameServer.Models
{
    class Guild
    {
        public TGuild Data;
        public int Id { get { return Data.Id; } }
        private Character Leader;
        public string Name { get { return Data.Name; } }
        public List<Character> Members = new List<Character>();
        public double timestamp;
        public Guild(TGuild guild)
        {
            Data = guild;
        }

        internal bool JoinApply(NGuildApplyInfo apply)
        {
            var oldApply = Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId);
            if (oldApply != null)
            {
                return false;
            }

            var dbApply = DBService.Instance.Entities.TGuildApplies.Create();
            dbApply.GuildId = apply.guildId;
            dbApply.CharacterId = apply.characterId;
            dbApply.Name = apply.Name;
            dbApply.Class = apply.Class;
            dbApply.Level = apply.Level;
            dbApply.ApplyTime = DateTime.Now;

            DBService.Instance.Entities.TGuildApplies.Add(dbApply);
            Data.Applies.Add(dbApply);
            return true;
        }

        internal bool JoinApprove(NGuildApplyInfo apply)
        {
            var oldApply = Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId && v.Result == 0);
            if (oldApply == null)
            {
                return false;
            }

            oldApply.Result = (int)apply.Result;

            if (apply.Result == ApplyResult.Accept)
            {
                this.AddMember(apply.characterId, apply.Name, apply.Class, apply.Level, GuildTitle.None);
            }

            DBService.Instance.Save();

            this.timestamp = TimeUtil.timestamp;
            return true;
        }

        public void AddMember(int id, string name, int @class, int level, GuildTitle title)
        {
            DateTime now = DateTime.Now;
            TGuildMember dbMember = new TGuildMember()
            {
                CharacterId = id,
                Name = name,
                Class = @class,
                Level = level,
                Title = (int)title,
                JoinTime = now,
            };
            this.Data.Members.Add(dbMember);
            timestamp = TimeUtil.timestamp;
        }

        public bool Leave(Character member)
        {
            if (member == null)
            {
                Log.ErrorFormat("Leave Guild: member is null");
                return false;
            }
            if (member.Guild == null)
            {
                Log.ErrorFormat("Leave Guild failed: Character {0} is not in a guild.\", character.Id");
                return false;
            }
            TGuildMember dbMember = Data.Members.FirstOrDefault(m => m.CharacterId == member.Id);
            if (dbMember == null)
                Log.ErrorFormat("Member {0} not found in database.", member.Id);
            this.Data.Members.Remove(dbMember);
            Members.Remove(member);
            member.Guild = null;
            Log.InfoFormat("Leave Guild: {0}: {1}", member.Id, member.Info.Name);
            timestamp = TimeUtil.timestamp;
            return true;
        }

        public void PostProcess(Character from, NetMessageResponse message)
        {
            if (message.Guild == null)
            {
                message.Guild = new GuildResponse();
                message.Guild.Result = Result.Success;
                message.Guild.guildInfo = this.GuildInfo(from);
            }
        }

        public NGuildInfo GuildInfo(Character from)
        {
            NGuildInfo info = new NGuildInfo()
            {
                Id = Id,
                guildName = Name,
                Notice = Data.Notice,
                leaderId = Data.LeaderID,
                leaderName = Data.LeaderName,
                creatTime = Data.CreateTime,
                memberCount = Data.Members.Count,
            };

            if (from != null)
            {
                info.Members.AddRange(GetMemberInfos());
                if (from.Id == Data.LeaderID)
                {
                    info.Applies.AddRange(GetApplyInfos());
                }
            }
            return info;
        }

        public List<NGuildMemberInfo> GetMemberInfos()
        {
            List<NGuildMemberInfo> members = new List<NGuildMemberInfo>();

            if (Data.Members == null || Data.Members.Count == 0)
            {
                Log.WarningFormat("Guild Id: {0}, Guild members list is null or empty.", Data.Id);
                return members;
            }

            foreach (var member in Data.Members)
            {
                var memberInfo = new NGuildMemberInfo()
                {
                    Id = member.Id,
                    characterId = member.CharacterId,
                    Title = (GuildTitle)member.Title,
                    joinTime = (long)TimeUtil.GetTimestamp(member.JoinTime),
                    lastTime = (long)TimeUtil.GetTimestamp(member.LastTime),
                };
                if (member.CharacterId <= 0)
                {
                    Log.WarningFormat("Guild Id: {0}, Member CharacterId is invalid: {1}", Data.Id, member.CharacterId);
                    continue;
                }
                var character = CharacterManager.Instance.GetCharacter(member.CharacterId);
                if (character != null)
                {
                    memberInfo.Info = character.GetBasicInfo();
                    memberInfo.Status = 1;
                    member.Level = character.Data.Level;
                    member.Name = character.Data.Name;
                    member.LastTime = DateTime.Now;
                }
                else
                {
                    memberInfo.Info = GetMemberInfo(member);
                    memberInfo.Status = 0;
                }

                if (member.Id == Data.LeaderID)
                {
                    Leader = character;
                }

                members.Add(memberInfo);
            }
            return members;
        }

        public NCharacterInfo GetMemberInfo(TGuildMember member)
        {
            return new NCharacterInfo()
            {
                Id = member.CharacterId,
                Name = member.Name,
                Class = (CharacterClass)member.Class,
                Level = member.Level,
            };
        }

        public List<NGuildApplyInfo> GetApplyInfos()
        {
            List<NGuildApplyInfo> applies = new List<NGuildApplyInfo>();
            foreach (var apply in Data.Applies)
            {
                applies.Add(new NGuildApplyInfo()
                {
                    guildId = apply.GuildId,
                    characterId = apply.CharacterId,
                    Name = apply.Name,
                    Class = apply.Class,
                    Level = apply.Level,
                    Result = (ApplyResult)apply.Result,
                });
            }
            return applies;
        }
    }
}