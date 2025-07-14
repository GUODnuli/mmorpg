﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;
using UnityEngine.Events;

using SkillBridge.Message;
using Entities;
using Models;

namespace Managers
{
    class CharacterManager : Singleton<CharacterManager>, IDisposable
    {
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();

        public UnityAction<Character> OnCharacterEnter;
        public UnityAction<Character> OnCharacterLeave;

        public CharacterManager()
        {

        }

        public void Dispose()
        {
            
        }

        public void Init()
        {

        }

        public void Clear()
        {
            int[] keys = this.Characters.Keys.ToArray();
            foreach (var key in keys)
            {
                this.RemoveCharacter(key);
            }
            this.Characters.Clear();
        }

        public void AddCharacter(NCharacterInfo cha)
        {
            Debug.LogFormat("AddCharacter: Character ID: {0}, Character Name: {1}, Map Id: {2}, Entity Name: {3}", cha.Id, cha.Name, cha.mapId, cha.Entity.String());
            Character character = new Character(cha);
            this.Characters[cha.EntityId] = character;
            EntityManager.Instance.AddEntity(character);
            OnCharacterEnter?.Invoke(character);
            if (cha.EntityId == User.Instance.CurrentCharacterInfo.EntityId)
            {
                User.Instance.CurrentCharacter = character;
            }
        }

        public void RemoveCharacter(int entityId)
        {
            Debug.LogFormat("RemoveCharacter: Character ID: {0}", entityId);
            if (this.Characters.ContainsKey(entityId))
            {
                EntityManager.Instance.RemoveEntity(this.Characters[entityId].Info.Entity);
                OnCharacterLeave?.Invoke(this.Characters[entityId]);
                this.Characters.Remove(entityId);
            }
        }

        public Character GetCharacter(int id)
        {
            this.Characters.TryGetValue(id, out var character);
            return character;
        }
    }
}