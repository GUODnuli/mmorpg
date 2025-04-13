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
        public Dictionary<int, Monster> Monsters = new Dictionary<int, Monster>();

        public void Init(Map map)
        {
            this.map = map;
        }

        internal Monster Create(int spawnMonId, int spawnLevel, NVector3 position, NVector3 direction)
        {
            Monster monster = new Monster(spawnMonId, spawnLevel, position, direction);
            EntityManager.Instance.AddEntity(this.map.ID, monster);
            monster.Info.Id = monster.entityId;
            monster.Info.mapId = this.map.ID;
            Monsters.Add(monster.Id, monster);

            this.map.MonsterEnter(monster);
            return monster;
        }
    }
}
