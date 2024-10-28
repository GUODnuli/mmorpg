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
using Common.Data;


namespace GameServer.Services
{
    class MapService : Singleton<MapService>
    {
        public MapService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(this.OnMapEntitySync);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapTeleportRequest>(this.OnMapTeleport);
        }

        public void Init()
        {
            MapManager.Instance.Init();
        }

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

        internal void OnMapTeleport(NetConnection<NetSession> sender, MapTeleportRequest request)
        {
            if (!DataManager.Instance.Teleporters.ContainsKey(request.teleporterId))
            {
                Log.WarningFormat("Source Teleporter ID [{0}] not existed.", request.teleporterId);
                return;
            }

            Character character = sender.Session.Character;
            if (!CharacterManager.Instance.Characters.ContainsKey(character.Id))
            {
                Log.WarningFormat("The character ID [{0}] not existed.", character.Id);
                return;
            }

            TeleporterDefine source = DataManager.Instance.Teleporters[request.teleporterId];
            if (source.LinkTo == 0 || !DataManager.Instance.Teleporters.ContainsKey(source.LinkTo))
            {
                Log.WarningFormat("Source Teleporter ID [{0}] LinkTo ID [{1}] not existed.", request.teleporterId, source.LinkTo);
            }

            TeleporterDefine target = DataManager.Instance.Teleporters[source.LinkTo];

            MapManager.Instance[source.MapID].CharacterLeave(character);
            character.Position = target.Position;
            character.Direction = target.Direction;
            MapManager.Instance[target.MapID].CharacterEnter(sender, character);
        }
    }
}
