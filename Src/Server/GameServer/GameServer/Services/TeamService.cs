using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    class TeamService : Singleton<TeamService>
    {

        public TeamService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamLeaveRequest>(this.OnTeamLeave);
        }

        public void Init()
        {
            TeamManager.Instance.Init();
        }

        private void OnTeamInviteRequest(NetConnection<NetSession> sender, TeamInviteRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnTeamInviteRequest: FromId: {0}, FromName: {1}, ToId: {2}, ToName: {3}", request.FromId, request.FromName, request.ToId, request.ToName);

            // Todo: 添加一些前置数据校验
            // 1.校验发送者与接收者是否为好友

            NetConnection<NetSession> target = SessionManager.Instance.GetSession(request.ToId);
            if (target == null || target.Session.Character.Team != null)
            {
                sender.Session.Response.teamInviteRsp = new TeamInviteResponse();
                sender.Session.Response.teamInviteRsp.Result = Result.Failed;
                sender.Session.Response.teamInviteRsp.Errormsg = target == null ? "好友不在线" : "对方已经有队伍";
                sender.SendResponse();
                return;
            }

            Log.InfoFormat("ForwardTeamInviteRequest: FromId: {0}, FromName: {1}, ToId: {2}, ToName: {3}", request.FromId, request.FromName, request.ToId, request.ToName);
            target.Session.Response.teamInviteReq = request;
            target.SendResponse();
        }

        private void OnTeamInviteResponse(NetConnection<NetSession> sender, TeamInviteResponse response)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnTeamInviteResponse: Character Id: {0}, Result: {1}, FromId: {2}, ToId: {3}", character.Id, response.Result, response.Request.FromId, response.Request.ToId);
            sender.Session.Response.teamInviteRsp = response;
            if (response.Result == Result.Success)
            {
                var requester = SessionManager.Instance.GetSession(response.Request.FromId);
                if (requester == null)
                {
                    sender.Session.Response.teamInviteRsp.Result = Result.Failed;
                    sender.Session.Response.teamInviteRsp.Errormsg = "请求者已下线";
                }
                else
                {
                    TeamManager.Instance.AddTeamMember(requester.Session.Character, character);
                    requester.Session.Response.teamInviteRsp = response;
                    requester.SendResponse();
                }
            }
            sender.SendResponse();
        }

        private void OnTeamLeave(NetConnection<NetSession> sender, TeamLeaveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnTeamLeave: character Id: {0}, Team Id: {1} : {2}", character.Id, request.TeamId, request.characterId);
            sender.Session.Response.teamLeave = new TeamLeaveResponse();
            sender.Session.Response.teamLeave.Result = Result.Success;
            sender.Session.Response.teamLeave.characterId = request.characterId;


        }
    }
}
