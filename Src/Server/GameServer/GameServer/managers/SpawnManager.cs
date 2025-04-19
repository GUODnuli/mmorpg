using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Models;
using GameServer.Entities;
using Common;

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

            foreach (var spawner in Rules.ToList())
            {
                if(spawner.IsValid)
                    spawner.Update();
                else
                {
                    Rules.Remove(spawner);
                    Log.WarningFormat("已移除无效的刷怪器: {0}", spawner.rule.ID);
                }
            }
        }
    }
}
