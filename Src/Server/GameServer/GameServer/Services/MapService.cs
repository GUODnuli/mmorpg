using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;
using Network;
using GameServer.Managers;
using GameServer.Entities;
using SkillBridge.Message;


namespace GameServer.Services
{
    class MapService : Singleton<MapService>
    {
        public MapService()
        {
            //MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapCharacterEnterRequest>(this.OnMapCharacterEnter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(this.OnMapEntitySync);
        }

        public void Init()
        {
            MapManager.Instance.Init();
        }

        //private void OnMapCharacterEnter(NetConnection<NetSession> sender, MapCharacterEnterRequest message)
        //{
        //    throw new NotImplementedException();
        //}

        private void OnMapEntitySync(NetConnection<NetSession> sender, MapEntitySyncRequest request)
        {
            Character character = sender.Session.Character;
            NEntitySync entitySync = request.entitySync;
            Log.InfoFormat("OnMapEntitySync: Character ID: {0}, Character Name: {1}, Entity ID: {2}, Entity Event: {3}, Entity: {4}", character.Id, character.Info.Name, entitySync.Id, entitySync.Event, entitySync.Entity);

            MapManager.Instance[character.Info.mapId].UpdateEntity(entitySync);
        }

        public void SendEntityUpdate(NetConnection<NetSession> conn, NEntitySync nEntitySync)
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.mapEntitySync = new MapEntitySyncResponse();

            message.Response.mapEntitySync.entitySyncs.Add(nEntitySync);
            byte[] data = PackageHandler.PackMessage(message);
            conn.SendData(data, 0, data.Length);
        }
    }
}
