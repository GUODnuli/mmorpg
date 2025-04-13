using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Models;
using GameServer.Entities;
using Common.Data;

namespace GameServer.Managers
{
    class SpawnManager
    {
        private List<Spawner> Rules = new List<Spawner>();
        private Map map;
        public SpawnManager() { }
        public void Init(Map map)
        {
            this.map = map;
            if(DataManager.Instance.SpawnRules.ContainsKey(map.Define.ID))
            {
                foreach(var rule in DataManager.Instance.SpawnRules[map.Define.ID].Values)
                {
                    this.Rules.Add(new Spawner(rule, this.map));
                }
            }
        }

        public void Update()
        {
            if (Rules.Count == 0)
                return;

            for (int i = 0; i < Rules.Count; i++)
            {
                this.Rules[i].Update();
            }
        }
    }
}
