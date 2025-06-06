using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    class GuildService : Singleton<GuildService>, IDisposable
    {
        public UnityAction OnGuildUpdate;
        public UnityAction<bool> OnGuildCreateResult;
        public UnityAction<List<NGuildInfo>> OnGuildListResult;

        public void Init()
        {

        }

        public GuildService()
        {
            MessageDistributer.Instance.Subscribe<GuildCreateResponse>(this.OnGuildCreate);
            MessageDistributer.Instance.Subscribe<GuildListResponse>(this.OnGuildList);
            MessageDistributer.Instance.Subscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer.Instance.Subscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer.Instance.Subscribe<GuildResponse>(this.OnGuild);
            MessageDistributer.Instance.Subscribe<GuildLeaveResponse>(this.OnGuildLeave);
            MessageDistributer.Instance.Subscribe<GuildAdminResponse>(this.OnGuildAdmin);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<GuildCreateResponse>(this.OnGuildCreate);
            MessageDistributer.Instance.Unsubscribe<GuildListResponse>(this.OnGuildList);
            MessageDistributer.Instance.Unsubscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer.Instance.Unsubscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer.Instance.Unsubscribe<GuildResponse>(this.OnGuild);
            MessageDistributer.Instance.Unsubscribe<GuildLeaveResponse>(this.OnGuildLeave);
            MessageDistributer.Instance.Unsubscribe<GuildAdminResponse>(this.OnGuildAdmin);
        }

        /// <summary>
        /// 发送创建公会请求
        /// </summary>
        /// <param name="guildName"></param>
        /// <param name="notice"></param>
        public void SendGuildCreate(string guildName, string notice)
        {
            Debug.Log("SendGuildCreate");
            NetMessage message = new();
            message.Request = new();
            message.Request.guildCreate = new();
            message.Request.guildCreate.GuildName = guildName;
            message.Request.guildCreate.GuildNotice = notice;
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildCreate(object sender, GuildCreateResponse response)
        {
            Debug.LogFormat("OnGuildCreateResponse: {0}", response.Result);
            // fixme
            //if(OnGuildCraeteResult != null)
            //{
            //    this.OnGuildCreateResult(response.Result);
            //}
            if (response.Result == Result.Success)
            {
                GuildManager.Instance.Init(response.guildInfo);
                MessageBox.Show(string.Format("{0} 公会创建成功", response.guildInfo.guildName, "公会"));
            }
            else
            {
                MessageBox.Show(string.Format("{0} 公会创建失败", response.guildInfo.guildName, "公会"));
            }
        }

        public void SendGuildJoinRequest(int guildId)
        {
            Debug.LogFormat("SendGuildJoinRequest: player Id: {0}, guildId: {1}", User.Instance.Info.Player.Id, guildId);
            NetMessage message = new();
            message.Request = new();
            message.Request.guildJoinReq = new();
            message.Request.guildJoinReq.Apply = new();
            message.Request.guildJoinReq.Apply.guildId = guildId;
            NetClient.Instance.SendMessage(message);
        }

        public void SendGuildJoinResponse(bool accept, GuildJoinRequest request)
        {
            Debug.LogFormat("SendGuildJoinResponse: Approver Id: {0}, Applyer Id: {1}", User.Instance.Info.Player.Id, request.Apply.characterId);
            NetMessage message = new();
            message.Request = new();
            message.Request.guildJoinRes = new();
            message.Request.guildJoinRes.Result = Result.Success;
            message.Request.guildJoinRes.Apply = request.Apply;
            message.Request.guildJoinRes.Apply.Result = accept ? ApplyResult.Accept : ApplyResult.Reject;
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildJoinRequest(object sender, GuildJoinRequest request)
        {
            if (request.Apply.Name == null)
                return;
            var confirm = MessageBox.Show(string.Format("{0} 申请加入公会", request.Apply.Name), "公会申请", MessageBoxType.Confirm, "同意", "不同意");
            confirm.OnYes = () =>
            {
                this.SendGuildJoinResponse(true, request);
            };
            confirm.OnNo = () =>
            {
                this.SendGuildJoinResponse(false, request);
            };
        }

        private void OnGuildJoinResponse(object sender, GuildJoinResponse response)
        {
            Debug.LogFormat("OnGuildJoinResponse: GuildJoin Application Approved Result: {0}", response.Apply.Result);
            if (response.Apply.Result == ApplyResult.Accept)
            {
                MessageBox.Show("加入公会成功", "通知");
            }
            else
            {
                MessageBox.Show("加入公会失败", "通知");
            }
        }

        private void OnGuild(object sender, GuildResponse message)
        {
            Debug.LogFormat("OnGuild: {0} {1}: {2}", message.Result, message.guildInfo.Id, message.guildInfo.guildName);
            GuildManager.Instance.Init(message.guildInfo);
            if (this.OnGuildUpdate != null)
                this.OnGuildUpdate();
        }

        public void SendGuildLeaveRequest()
        {
            Debug.Log("SendFriendRemoeRequest");
            NetMessage message = new();
            message.Request = new();
            message.Request.guildLeave = new GuildLeaveRequest();
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildLeave(object sender, GuildLeaveResponse message)
        {
            if (message.Result == Result.Success)
            {
                GuildManager.Instance.Init(null);
                MessageBox.Show("离开公会成功", "通知");
            }
            else
            {
                MessageBox.Show("离开公会失败", "通知", MessageBoxType.Error);
            }
        }

        public void SendGuildListRequest()
        {
            Debug.Log("SendGuildListRequest");
            NetMessage message = new();
            message.Request = new();
            message.Request.guildList = new GuildListRequest();
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildList(object sender, GuildListResponse response)
        {
            if (OnGuildListResult != null)
            {
                this.OnGuildListResult(response.Guilds);
            }
        }

        public void SendGuildJoinApply(bool accept, NGuildApplyInfo apply)
        {
            Debug.Log("SendGuildJoinResponse");
            NetMessage message = new();
            message.Request = new();
            message.Request.guildJoinRes = new();
            message.Request.guildJoinRes.Result = Result.Success;
            message.Request.guildJoinRes.Apply = apply;
            message.Request.guildJoinRes.Apply.Result = accept ? ApplyResult.Accept : ApplyResult.Reject;
            NetClient.Instance.SendMessage(message);
        }

        public void SendAdminCommand(GuildAdminCommand command, int characterId)
        {
            Debug.Log("SendAdminCommand");
            NetMessage message = new();
            message.Request = new();
            message.Request.guildAdmin = new();
            message.Request.guildAdmin.Command = command;
            message.Request.guildAdmin.Target = characterId;
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildAdmin(object sender, GuildAdminResponse message)
        {
            Debug.LogFormat("GuildAdmin: {0} {1}", message.Command, message.Result);
            MessageBox.Show(string.Format("执行操作: {0}, 结果: {1} {2}", message.Command, message.Result, message.Errormsg));
        }
    }
}