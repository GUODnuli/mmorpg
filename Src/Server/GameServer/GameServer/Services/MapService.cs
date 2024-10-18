using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;
using Network;
using GameServer.Managers;
using SkillBridge.Message;


namespace GameServer.Services
{
    class MapService : Singleton<MapService>
    {
        public MapService()
        {
            //MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapCharacterEnterRequest>(this.OnMapCharacterEnter);
        }

        public void Init()
        {
            MapManager.Instance.Init();
        }
    }
}
