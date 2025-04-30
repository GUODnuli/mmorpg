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
    public class TeamService : Singleton<TeamService>, IDisposable
    {
        public void Init()
        {

        }

        public TeamService()
        {
            MessageDistributer.Instance.Subscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Subscribe<TeamInfoResponse>(this.OnTeamInfo);
            MessageDistributer.Instance.Subscribe<TeamLeaveResponse>(this.OnTeamLeave);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Unsubscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Unsubscribe<TeamInfoResponse>(this.OnTeamInfo);
            MessageDistributer.Instance.Unsubscribe<TeamLeaveResponse>(this.OnTeamLeave);
        }

        public void SendTeamInviteRequest()
        {

        }

        public void SendTeamInviteResponse(bool accept, TeamInviteRequest request)
        {
            Debug.Log("SendTeamInviteResponse");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamInviteRsp = new TeamInviteResponse();
            message.Request.teamInviteRsp.Result = accept ? Result.Success : Result.Failed;
            message.Request.teamInviteRsp.Request = request;
            NetClient.Instance.SendMessage(message);
        }
    }
}