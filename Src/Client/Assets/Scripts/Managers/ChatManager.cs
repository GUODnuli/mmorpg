using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Models;
using SkillBridge.Message;
using Common;
using System;
using Services;

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
            this.PrivateID = targetId;
            this.PrivateName = targetName;

            this.sendChannel = LocalChannel.Private;
            if (this.OnChat != null)
            {
                this.OnChat();
            }
        }

        public List<ChatMessage>[] Messages = new List<ChatMessage>[6] {
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
        };
        public LocalChannel displayChannel;
        public LocalChannel sendChannel;
        public int PrivateID = 0;
        public string PrivateName = "";

        public ChatChannel SendChannel
        {
            get {
                switch (sendChannel)
                {
                    case LocalChannel.Local: return ChatChannel.Local;
                    case LocalChannel.World: return ChatChannel.World;
                    case LocalChannel.Team: return ChatChannel.Team;
                    case LocalChannel.Guild: return ChatChannel.Guild;
                    case LocalChannel.Private: return ChatChannel.Private;
                    case LocalChannel.All:
                        break;
                    default:
                        break;
                }
                return ChatChannel.Local;
            }
        }

        public Action OnChat { get; internal set; }

        public void Init()
        {
            foreach(var messages in this.Messages)
            {
                messages.Clear();
            }
        }

        public void SendChat(string content, int toId = 0, string toName = "")
        {
            ChatService.Instance.SendChat(this.SendChannel, content, toId, toName);
            //this.Messages.Add(new ChatMessage()
            //{
            //    Channel = ChatChannel.Local,
            //    Message = content,
            //    FromId = User.Instance.CurrentCharacter.Id,
            //    FromName = User.Instance.CurrentCharacter.Name,
            //});
        }

        public bool SetSendChannel(LocalChannel channel)
        {
            if (channel == LocalChannel.Team)
            {
                if (User.Instance.TeamInfo == null)
                {
                    this.AddSystemMessage("��û�м����κζ��顣");
                    return false;
                }
            }

            if (channel == LocalChannel.Guild)
            {
                if (User.Instance.CurrentCharacter.Guild == null)
                {
                    this.AddSystemMessage("��û�м����κι��ᡣ");
                    return false;
                }
            }

            this.sendChannel = channel;
            Debug.LogFormat("Set Channel: {0}", this.sendChannel);
            return true;
        }

        public void AddMessages(ChatChannel channel, List<ChatMessage> messages)
        {
            for (int ch = 0; ch < 6; ch ++)
            {
                if ((this.ChannelFilter[ch] & channel) == channel)
                {
                    this.Messages[ch].AddRange(messages);
                }
            }
            if (this.OnChat != null)
            {
                this.OnChat();
            }
        }

        public void AddSystemMessage(string message, string from = "")
        {
            this.Messages[(int)LocalChannel.All].Add(new ChatMessage()
            {
                Channel = ChatChannel.System,
                Message = message,
                FromName = from,
            });
            if (this.OnChat != null)
            {
                this.OnChat();
            }
        }

        public string GetCurrentMessages()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var message in this.Messages[(int)displayChannel])
            {
                sb.AppendLine(FormatMessage(message));
            }
            return sb.ToString();
        }

        private string FormatMessage(ChatMessage message)
        {
            switch(message.Channel)
            {
                case ChatChannel.Local:
                    return string.Format("[����]{0}{1}", FormatFromPlayer(message), message.Message);
                case ChatChannel.World:
                    return string.Format("<color=cyan>[����]{0}{1}</color>", FormatFromPlayer(message), message.Message);
                case ChatChannel.System:
                    return string.Format("<color=yellow>[ϵͳ]{0}</color>", message.Message);
                case ChatChannel.Private:
                    return string.Format("<color=magenta>[˽��]{0}{1}</color>", FormatFromPlayer(message), message.Message);
                case ChatChannel.Team:
                    return string.Format("<color=green>[����]{0}{1}</color>", FormatFromPlayer(message), message.Message);
                case ChatChannel.Guild:
                    return string.Format("<color=blue>[����]{0}{1}</color>", FormatFromPlayer(message), message.Message);
            }
            return "";
        }

        private string FormatFromPlayer(ChatMessage message)
        {
            if (message.FromId == User.Instance.CurrentCharacter.Id)
            {
                return "<a name=\"\" class=\"palyer\">[��]</a>";
            }
            else
                return string.Format("<a name=\"c:{0}:{1}\" class=\"palyer\">[{1}]</a>", message.FromId, message.FromName);
        }
    }
}