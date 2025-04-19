using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Models;
using GameServer.Entities;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class MonsterManager
    {
        private Map map;
        private int spawnMonId = 0;
        public Dictionary<int, Monster> Monsters = new Dictionary<int, Monster>();

        public void Init(Map map)
        {
            this.map = map;
        }

        internal Monster Create(int spawnMonConfigId, int spawnLevel, NVector3 position, NVector3 direction)
        {
            Monster monster = new Monster(spawnMonConfigId, this.spawnMonId, spawnLevel, position, direction);
            EntityManager.Instance.AddEntity(this.map.ID, monster);
            monster.Id = monster.entityId;
            monster.Info.Id = monster.entityId;
            monster.Info.EntityId = monster.entityId;
            monster.Info.mapId = this.map.ID;
            Monsters.Add(monster.Id, monster);

            this.map.MonsterEnter(monster);
            this.spawnMonId += 1;
            return monster;
        }
    }
}
