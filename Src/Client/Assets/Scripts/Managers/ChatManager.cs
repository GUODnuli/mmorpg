using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Models;
using SkillBridge.Message;

namespace Managers
{
    class ChatManager : Singleton<ChatManager>
    {
        public ChatManager() { }

        public enum LocalChannel
        {
            All = 0,
            Local = 1,
            World = 2,
            Team = 3,
            Guild = 4,
            Private = 5,
        }

        private ChatChannel[] ChannelFilter = new ChatChannel[6]
        {
            ChatChannel.Local | ChatChannel.World | ChatChannel.Team | ChatChannel.Guild | ChatChannel.Private | ChatChannel.System,
            ChatChannel.Local,
            ChatChannel.World, 
            ChatChannel.Team, 
            ChatChannel.Guild, 
            ChatChannel.Private,
        };

        internal void StartPrivateChat(int targetId, string targetName)
        {
            
        }
    }
}