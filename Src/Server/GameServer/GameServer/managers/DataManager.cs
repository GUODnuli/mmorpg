﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Common;
using Common.Data;
using Newtonsoft.Json;

namespace GameServer.Managers
{
    public class DataManager : Singleton<DataManager>
    {
        internal string DataPath;
        internal Dictionary<int, MapDefine> Maps = null;
        internal Dictionary<int, CharacterDefine> Characters = null;
        internal Dictionary<int, TeleporterDefine> Teleporters = null;
        public Dictionary<int, NPCDefine> NPCs = null;
        public Dictionary<int, ItemDefine> Items = null;
        public Dictionary<int, ShopDefine> Shops = null;
        public Dictionary<int, Dictionary<int, ShopItemDefine>> ShopItems = null;
        public Dictionary<int, EquipDefine> Equips = null;
        public Dictionary<int, QuestDefine> Quests = null;
        public Dictionary<int, Dictionary<int, SpawnPointDefine>> SpawnPoints = null;
        public Dictionary<int, Dictionary<int, SpawnRuleDefine>> SpawnRules = null;

        public DataManager()
        {
            this.DataPath = "Data/";
            Log.Info("DataManager > DataManager()");
        }

        internal void Load()
        {
            string json = File.ReadAllText(this.DataPath + "MapDefine.txt");
            this.Maps = JsonConvert.DeserializeObject<Dictionary<int, MapDefine>>(json);

            json = File.ReadAllText(this.DataPath + "CharacterDefine.txt");
            this.Characters = JsonConvert.DeserializeObject<Dictionary<int, CharacterDefine>>(json);

            json = File.ReadAllText(this.DataPath + "TeleporterDefine.txt");
            this.Teleporters = JsonConvert.DeserializeObject<Dictionary<int, TeleporterDefine>>(json);

            json = File.ReadAllText(this.DataPath + "NpcDefine.txt");
            this.NPCs = JsonConvert.DeserializeObject<Dictionary<int, NPCDefine>>(json);

            json = File.ReadAllText(this.DataPath + "ItemDefine.txt");
            this.Items = JsonConvert.DeserializeObject<Dictionary<int, ItemDefine>>(json);

            json = File.ReadAllText(this.DataPath + "ShopDefine.txt");
            this.Shops = JsonConvert.DeserializeObject<Dictionary<int, ShopDefine>>(json);

            json = File.ReadAllText(this.DataPath + "ShopItemDefine.txt");
            this.ShopItems = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, ShopItemDefine>>>(json);

            json = File.ReadAllText(this.DataPath + "EquipDefine.txt");
            this.Equips = JsonConvert.DeserializeObject<Dictionary<int ,EquipDefine>>(json);

            json = File.ReadAllText(this.DataPath + "QuestDefine.txt");
            this.Quests = JsonConvert.DeserializeObject<Dictionary<int, QuestDefine>>(json);

            json = File.ReadAllText(this.DataPath + "SpawnPointDefine.txt");
            this.SpawnPoints = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, SpawnPointDefine>>>(json);

            json = File.ReadAllText(this.DataPath + "SpawnRuleDefine.txt");
            this.SpawnRules = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, SpawnRuleDefine>>>(json);
        }
    }
}