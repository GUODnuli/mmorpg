﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;

using Common;
using Common.Data;

using Network;
using GameServer.Managers;
using GameServer.Entities;

namespace GameServer.Models
{
    class Map
    {
        internal MapDefine Define;
        Dictionary<int, MapCharacter> MapCharacters = new Dictionary<int, MapCharacter>();

        internal Map(MapDefine define)
        {
            this.Define = define;
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

        }

        /// <summary>
        /// 角色进入地图
        /// </summary>
        /// <param name="character"></param>
        internal void CharacterEnter(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("CharacterEnter: Map ID: {0}, Character ID: {1}", this.Define.ID, character.Info.Id);

            character.Info.mapId = this.ID;

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();

            message.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            message.Response.mapCharacterEnter.mapId = this.Define.ID;
            message.Response.mapCharacterEnter.Characters.Add(character.Info);

            foreach (var i in this.MapCharacters)
            {
                // 这个遍历里面分别干了两件事，节省了一次遍历时间
                // 遍历当前地图的所有角色，并加入到本次response
                message.Response.mapCharacterEnter.Characters.Add(i.Value.character.Info);

                // 把当前用户的角色加入的消息发送给该地图的所有用户
                this.SendCharacterEnterMap(i.Value.connection, character.Info);
            }

            this.MapCharacters[character.Id] = new MapCharacter(conn, character);

            byte[] data = PackageHandler.PackMessage(message);
            conn.SendData(data, 0, data.Length);
        }

        void SendCharacterEnterMap(NetConnection<NetSession> conn, NCharacterInfo character)
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();

            message.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            message.Response.mapCharacterEnter.mapId = this.Define.ID;
            message.Response.mapCharacterEnter.Characters.Add(character);

            byte[] data = PackageHandler.PackMessage(message);
            conn.SendData(data, 0, data.Length);
        }

        internal void CharacterLeave(NCharacterInfo character)
        {
            Log.InfoFormat("CharacterLeave: Map ID： {0}， Character ID: {1}", this.Define.ID, character.Id);

            this.MapCharacters.Remove(character.Id);
            foreach (var kv in this.MapCharacters)
            {
                this.SendCharacterLeaveMap(kv.Value.connection, character);
            }
        }

        void SendCharacterLeaveMap(NetConnection<NetSession> conn, NCharacterInfo character)
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();

            message.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
            message.Response.mapCharacterLeave.characterId = character.Id;

            byte[] data = PackageHandler.PackMessage(message);
            conn.SendData(data, 0, data.Length);
        }
    }
}
