﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;
using Common.Battle;

namespace Common.Data
{
    public enum TargetType
    {
        None,
    }

    public enum BuffEffect
    {
        None,
    }

    public class BuffDefine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public TargetType Target { get; set; }
        public BuffEffect Effect { get; set; }
        public float CD { get; set; }
        public float Duration { get; set; }
        public List<int> Buff { get; set; }
        public float AD { get; set; }
        public float AP { get; set; }
        public float ADFactor { get; set; }
        public float APFactor { get; set; }
        public float DEFRatio { get; set; }
    }
}
