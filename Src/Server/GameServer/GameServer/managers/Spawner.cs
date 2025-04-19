using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Data;
using GameServer.Models;

namespace GameServer.Managers
{
    internal class Spawner
    {
        public bool IsValid { get; private set; }
        public SpawnRuleDefine rule { get; set; }
        private Map map;
        private float spawnTime = 0f;
        private float MonsterDeadTime = 0f;
        private bool isSpawningCooldown = false;
        private SpawnPointDefine spawnPoint = null;
        public Spawner(SpawnRuleDefine rule, Map map)
        {
            // 参数基础校验
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            if (map == null) throw new ArgumentNullException(nameof(map));
            this.rule = rule;
            this.map = map;

            // 配置完整性校验
            if (!DataManager.Instance.SpawnPoints.TryGetValue(map.Define.ID, out var pointDict))
            {
                Log.Error($"地图配置缺失: MapID={map.Define.ID}");
                IsValid = false; // 标记实例无效
                return;
            }

            if (!pointDict.TryGetValue(rule.SpawnPoint, out this.spawnPoint))
            {
                Log.Error($"刷怪点配置错误: MapID={map.Define.ID}, SpawnPointID={rule.SpawnPoint}");
                IsValid = false;
                return;
            }

            IsValid = true; // 通过所有校验

            if (DataManager.Instance.SpawnPoints.ContainsKey(map.Define.ID))
            {
                if (DataManager.Instance.SpawnPoints[map.Define.ID].ContainsKey(this.rule.SpawnPoint))
                {
                    this.spawnPoint = DataManager.Instance.SpawnPoints[map.Define.ID][this.rule.SpawnPoint];
                }
                else
                {
                    Log.ErrorFormat("SpawnRule: [{0}], SpawnPoint: [{1}] not existed.", this.rule.ID, this.rule.SpawnPoint);
                }
            }
        }

        public void Update()
        {
            if (CanSpawn())
            {
                Spawn();
            }
        }

        private bool CanSpawn()
        {
            if (this.isSpawningCooldown)
                return false;
            if (this.MonsterDeadTime + this.spawnTime > Time.time)
                return false;

            return true;
        }

        private void Spawn()
        {
            this.isSpawningCooldown = true;
            Log.InfoFormat("Map: [{0}] Spawn Monster [{1}] Lv.[{2}] at SpawnPoint [{3}].", this.rule.MapID, this.rule.SpawnMonConfigID, this.rule.SpawnLevel, this.spawnPoint.ID);
            this.map.MonsterManager.Create(this.rule.SpawnMonConfigID, this.rule.SpawnLevel, this.spawnPoint.Position, this.spawnPoint.Direction);
        }
    }
}
