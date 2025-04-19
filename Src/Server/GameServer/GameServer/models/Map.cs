using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;
using Common.Data;
using Network;
using GameServer.Managers;
using SkillBridge.Message;
using GameServer.Entities;
using GameServer.Services;

namespace GameServer.Models
{
    class Map
    {
        internal MapDefine Define;

        // 地图中的角色以CharacterID为Key
        Dictionary<int, MapCharacter> MapCharacters = new Dictionary<int, MapCharacter>();
        SpawnManager SpawnManager = new SpawnManager();
        public MonsterManager MonsterManager = new MonsterManager();
        
        internal Map(MapDefine define)
        {
            this.Define = define;
            this.SpawnManager.Init(this);
            this.MonsterManager.Init(this);
        }

        internal class MapCharacter
        {
            public NetConnection<NetSession> connection;
            public Character character;

            public MapCharacter(NetConnection<NetSession> conn, Character cha)
            {
                this.connection = conn;
                this.character = cha;
            }
        }

        public int ID
        {
            get { return this.Define.ID; }
        }

        public void Update()
        {
            SpawnManager.Update();
        }

        /// <summary>
        /// 角色进入地图
        /// </summary>
        /// <param name="character"></param>
        internal void CharacterEnter(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("CharacterEnter: Map ID: {0}, Character ID: {1}", this.Define.ID, character.Id);

            character.Info.mapId = this.ID;
            this.MapCharacters.Add(character.Id, new MapCharacter(conn, character));

            conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            //conn.Session.Response.mapCharacterEnter.Characters.Add(character.Info);

            foreach (var i in this.MapCharacters)
            {
                // 这个遍历里面分别干了两件事，节省了一次遍历时间
                // 遍历当前地图的所有角色，并加入到本次response
                conn.Session.Response.mapCharacterEnter.Characters.Add(i.Value.character.Info);
                if (i.Value.character != character)
                    this.AddCharacterEnterMap(i.Value.connection, character.Info);
            }
            
            foreach (var i in this.MonsterManager.Monsters)
            {
                conn.Session.Response.mapCharacterEnter.Characters.Add(i.Value.Info);
            }
            conn.SendResponse();

            //this.MapCharacters[character.entityId] = new MapCharacter(conn, character);

            //byte[] data = PackageHandler.PackMessage(message);
            //conn.SendData(data, 0, data.Length);
        }

        void AddCharacterEnterMap(NetConnection<NetSession> conn, NCharacterInfo character)
        {
            if (conn.Session.Response.mapCharacterEnter != null)
            {
                conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
                conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            }

            conn.Session.Response.mapCharacterEnter.Characters.Add(character);
            conn.SendResponse();
        }

        internal void CharacterLeave(Character character)
        {
            Log.InfoFormat("CharacterLeave: Map ID： {0}， Character ID: {1}", this.Define.ID, character.Id);
            foreach (var kv in this.MapCharacters)
            {
                this.RemoveCharacterEnterMap(kv.Value.connection, character);
            }
            this.MapCharacters.Remove(character.Id);
        }

        void RemoveCharacterEnterMap(NetConnection<NetSession> conn, Character character)
        {
            conn.Session.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
            conn.Session.Response.mapCharacterLeave.entityId = character.entityId;
            conn.SendResponse();
        }

        internal void UpdateEntity(NEntitySync entitySync)
        {
            foreach (var kv in this.MapCharacters)
            {
                if (kv.Value.character.entityId == entitySync.EntityId)
                {
                    kv.Value.character.Position = entitySync.Entity.Position;
                    kv.Value.character.Direction = entitySync.Entity.Direction;
                    kv.Value.character.Speed = entitySync.Entity.Speed;
                } 
                else
                {
                    MapService.Instance.SendEntityUpdate(kv.Value.connection, entitySync);
                }
            }
        }

        internal void MonsterEnter(Monster monster)
        {
            Log.InfoFormat("MonsterSpawned: Map: {0}, monsterId: {1}", this.Define.ID, monster.Id);
            foreach(var kv in this.MapCharacters)
            {
                this.AddCharacterEnterMap(kv.Value.connection, monster.Info);
            }
        }
    }
}
