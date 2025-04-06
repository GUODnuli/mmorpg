using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;

namespace Managers
{
    public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
    {
        public GameObject nameBarPrefab;
        public GameObject questStatusPrefab;

        public Dictionary<Transform, GameObject> nameElements = new Dictionary<Transform, GameObject>();
        public Dictionary<Transform, GameObject> questElements = new Dictionary<Transform, GameObject>();

        public void AddCharacterNameBar(Transform owner, Character character)
        {
            GameObject goNameBar = Instantiate(nameBarPrefab, this.transform);
            goNameBar.name = "NameBar_" + character.entityId;
            goNameBar.GetComponent<UIWorldElement>().owner = owner;
            goNameBar.GetComponent<UICharBar>().character = character;
            goNameBar.SetActive(true);
            this.nameElements[owner] = goNameBar;
        }

        public void RemoveCharacterNameBar(Transform owner) 
        {
            if (this.nameElements.ContainsKey(owner))
            {
                Destroy(this.nameElements[owner]);
                this.nameElements.Remove(owner);
            }
        }

        public void AddNpcQuestStatus(Transform owner, NpcQuestStatus status)
        {
            if (this.questElements.ContainsKey(owner))
            {
                questElements[owner].GetComponent<UIQuestStatus>().SetQuestStatus(status);
            }
            else
            {
                GameObject go = Instantiate(questStatusPrefab, this.transform);
                go.name = "QuestStatus_" + owner.name;
                go.GetComponent<UIWorldElement>().owner = owner;
                go.GetComponent<UIQuestStatus>().SetQuestStatus(status);
                go.SetActive(true);
                this.questElements[owner] = go;
            }
        }

        public void RemoveNpcQuestStatus(Transform owner)
        {
            if (this.questElements.ContainsKey(owner))
            {
                Destroy(this.questElements[owner]);
                this.questElements.Remove(owner);
            }
        }
    }
}
