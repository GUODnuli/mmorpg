using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class FriendService : Singleton<FriendService>
    {
        public FriendService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendRemoveRequest>(this.OnFriendRemove);
        }

        public void Init()
        {

        }

        /// <summary>
        /// 收到好友添加请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="request"></param>
        private void OnFriendAddRequest(NetConnection<NetSession> sender, FriendAddRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddRequest: From Id: {0}, From Name: {1}, To Id: {2}, To Name: {3}", request.FromId, request.FromName, request.ToId, request.ToName);

            if (request.ToId == 0)
            {
                foreach (var cha in CharacterManager.Instance.Characters)
                {
                    if (cha.Value.Data.Name == request.ToName)
                    {
                        request.ToId = cha.Key;
                        break;
                    }
                }
            }

            NetConnection<NetSession> friend = null;
            if (request.ToId > 0)
            {
                if (character.FriendManager.GetFriendInfo(request.ToId) != null)
                {
                    sender.Session.Response.friendAddRsp.Result = Result.Failed;
                    sender.Session.Response.friendAddRsp.Errormsg = "已经是好友了。";
                    sender.SendResponse();
                    return;
                }
                friend = SessionManager.Instance.GetSession(request.ToId);
            }
            if (friend == null)
            {
                sender.Session.Response.friendAddRsp = new FriendAddResponse();
                sender.Session.Response.friendAddRsp.Result = Result.Failed;
                sender.Session.Response.friendAddRsp.Errormsg = "好友不存在或不在线。";
                sender.SendResponse();
                return;
            }

            Log.InfoFormat("ForwardRequest: From Id: {0}, From Name: {1}, To Id: {2}, To Name: {3}", request.FromId, request.FromName, request.ToId, request.ToName);
            friend.Session.Response.friendAddReq = request;
            friend.SendResponse();
        }

        /// <summary>
        /// 收到好友添加响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private void OnFriendAddResponse(NetConnection<NetSession> sender, FriendAddResponse response)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddResponse: Character Id: {0}, Result: {1}, From Id: {2}, To Id: {3}", character.Id, response.Result, response.Request.FromId, response.Request.ToId);
            sender.Session.Response.friendAddRsp = response;
            var requester = SessionManager.Instance.GetSession(response.Request.FromId);
            if (requester == null)
            {
                sender.Session.Response.friendAddRsp.Result = Result.Failed;
                sender.Session.Response.friendAddRsp.Errormsg = "请求者已下线";
                sender.SendResponse();
                return;
            }

            if (response.Result == Result.Success)
            {
                character.FriendManager.AddFriend(requester.Session.Character);
                requester.Session.Character.FriendManager.AddFriend(character);
                DBService.Instance.Save();
                requester.Session.Response.friendAddRsp = response;
                requester.Session.Response.friendAddRsp.Result = Result.Success;
                requester.Session.Response.friendAddRsp.Errormsg = "添加好友成功";
                requester.SendResponse();

                sender.Session.Response.friendAddRsp.Result = Result.Success;
                sender.Session.Response.friendAddRsp.Errormsg = response.Request.FromName + "成为了你的好友";
            }
            else
            {
                requester.Session.Response.friendAddRsp = response;
                requester.Session.Response.friendAddRsp.Result = Result.Failed;
                requester.Session.Response.friendAddRsp.Errormsg = "对方拒绝了你的好友申请。";
                requester.SendResponse();

                sender.Session.Response.friendAddRsp.Result = Result.Success;
                sender.Session.Response.friendAddRsp.Errormsg = string.Format("你拒绝了{0}的好友申请。", response.Request.FromName);
            }
            sender.SendResponse();
        }
    }
}
