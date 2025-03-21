using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class NPCManager : Singleton<NPCManager>
    {
        public delegate bool NpcActionHandler(NPCDefine npcDefine);
        Dictionary<NpcFunction, NpcActionHandler> eventMap = new Dictionary<NpcFunction, NpcActionHandler>();

        public void RegisterNpcEvent(NpcFunction npcFunction, NpcActionHandler action)
        {
            if (!eventMap.ContainsKey(npcFunction))
            {
                eventMap[npcFunction] = action;
            }
            else
            {
                eventMap[npcFunction] += action;
            }
        }

        public NPCDefine GetNPCDefine(int npcId)
        {
            NPCDefine npc = null;
            DataManager.Instance.NPCs.TryGetValue(npcId, out npc);
            return npc;
        }

        public bool Interactive(int npcId)
        {
            if (DataManager.Instance.NPCs.ContainsKey(npcId))
            {
                var npc = DataManager.Instance.NPCs[npcId];
                return Interactive(npc);
            }
            return false;
        }

        public bool Interactive(NPCDefine npcDefine)
        {
            if (npcDefine.Type == NpcType.Task)
            {
                return DoTaskInteractive(npcDefine);
            }
            else if (npcDefine.Type == NpcType.Functional)
            {
                return DoFunctionInteractive(npcDefine);
            }
            return false;
        }

        private bool DoTaskInteractive(NPCDefine npcDefine)
        {
            MessageBox.Show("µã»÷ÁËNPC: " + npcDefine.Name);
            return true;
        }

        private bool DoFunctionInteractive(NPCDefine npcDefine)
        {
            if (npcDefine.Type != NpcType.Functional)
            {
                return false;
            }

            if (!eventMap.ContainsKey(npcDefine.Function))
            {
                return false;
            }
            return eventMap[npcDefine.Function](npcDefine);
        }
    }
}
