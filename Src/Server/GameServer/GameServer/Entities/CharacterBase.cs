﻿using Common.Data;
using GameServer.Core;
using SkillBridge.Message;
using GameServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Entities
{
    class CharacterBase : Entity
    {

        public int Id { get; set; }
        public string Name { get { return Info.Name; } }

        public NCharacterInfo Info;
        public CharacterDefine Define;

        public CharacterBase(Vector3Int pos, Vector3Int dir):base(pos,dir)
        {

        }

        public CharacterBase(CharacterType type,int configId, int entityId, int level, Vector3Int pos, Vector3Int dir) :
           base(pos, dir)
        {
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Level = level;
            this.Info.ConfigId = configId;
            this.Info.EntityId = this.entityId;
            this.Info.Entity = this.EntityData;
            this.Define = DataManager.Instance.Characters[this.Info.ConfigId];
            this.Info.Name = this.Define.Name;
        }
    }
}
