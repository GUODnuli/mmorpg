using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;

using SkillBridge.Message;
using Models;
using Common.Data;
using Managers;

namespace Services
{
    class MapService : Singleton<MapService>, IDisposable
    {
        public int CurrentMapId { get; set; }

        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(this.OnMapEntitySync);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Unsubscribe<MapEntitySyncResponse>(this.OnMapEntitySync);
        }

        public void Init()
        {

        }

        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.LogFormat("OnMapCharacterEnter: Map: {0}, Count: {1}", response.mapId, response.Characters.Count);
            foreach (var cha in response.Characters)
            {
                if (User.Instance.CurrentCharacter.Id == cha.Id)
                {
                    User.Instance.CurrentCharacter = cha;
                }
                CharacterManager.Instance.AddCharacter(cha);
            }

            if (CurrentMapId != response.mapId)
            {
                this.EnterMap(response.mapId);
                this.CurrentMapId = response.mapId;
            }

        }

        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.Maps[mapId];
                User.Instance.CurrentMap = map;
                SceneManager.Instance.LoadScene(map.Resource);
            }
            else
            {
                Debug.LogErrorFormat("EnterMap: Map {0} not exited.", mapId);
            }
        }

        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        {
            Debug.LogFormat("OnCharacterLeave: Character ID: {0}", response.characterId);
            if (User.Instance.CurrentCharacter.Id != response.characterId)
            {
                CharacterManager.Instance.RemoveCharacter(response.characterId);
            }
            else
            {
                CharacterManager.Instance.Clear();
            }
        }

        public void SendMapEntitySync(EntityEvent entityEvent, NEntity nEntity)
        {
            Debug.LogFormat("MapEntityUpdateRequest: Entity ID: {0}, Entity Pos: {1}, Entity Dir: {2}, Entity Speed: {3}", nEntity.Id, nEntity.Position.String(), nEntity.Direction.ToString(), nEntity.Speed.ToString());
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapEntitySync = new MapEntitySyncRequest();
            message.Request.mapEntitySync.entitySync = new NEntitySync
            {
                Id = nEntity.Id,
                Event = entityEvent,
                Entity = nEntity,
            };
            NetClient.Instance.SendMessage(message);
        }

        private void OnMapEntitySync(object sender, MapEntitySyncResponse response)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("MapEntityUpdateResponse: Entitys: {0}", response.entitySyncs.Count);
            sb.AppendLine();
            foreach (var v in response.entitySyncs)
            {
                EntityManager.Instance.OnEntitySync(v);
                sb.AppendFormat("Entity ID: {0}, Entity Event: {1}, Entity: {2}", v.Id, v.Event, v.Entity.String());
                sb.AppendLine();
            }
            Debug.Log(sb.ToString());
        }
    }
}