using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameServer;
using GameServer.Entities;
using GameServer.Services;
using SkillBridge.Message;
using GameServer.Managers;
using GameServer.Network;

namespace Network
{
    class NetSession : INetSession
    {
        public TUser User { get; set; }
        public Character Character { get; set; }
        public NEntity Entity { get; set; } 
        public IPostResponse PostResponse { get; set; }

        public void Disconnected()
        {
            this.PostResponse = null;
            if (this.Character != null)
            {
                UserService.Instance.UserLeaveGame(this.Character);
            }
        }

        NetMessage response;

        public NetMessageResponse Response
        {
            get
            {
                if (response == null)
                    response = new NetMessage();
                if (response.Response == null)
                    response.Response = new NetMessageResponse();
                return response.Response;
            }
        }

        public byte[] GetResponse()
        {
            if (response != null)
            {
                if (PostResponse != null)
                    this.PostResponse.PostProcess(Response);
                byte[] data = PackageHandler.PackMessage(response);
                response = null;
                return data;
            }
            return null;
        }
    }
}
