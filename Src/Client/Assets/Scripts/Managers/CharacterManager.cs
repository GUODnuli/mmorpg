using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;
using UnityEngine.Events;

using SkillBridge.Message;
using Entities;

namespace Managers
{
    class CharacterManager : Singleton<CharacterManager>, IDisposable
    {
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();

        public UnityAction<Character> OnCharacterEnter;

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
            this.Characters.Clear();
        }

        public void AddCharacter(NCharacterInfo cha)
        {
            Debug.LogFormat("AddCharacter: Character Id: {0}, Character Name: {1}, Map Id: {2}, Entity Name: {3}", cha.Id, cha.Name, cha.mapId, cha.Entity.String());
            Character character = new Character(cha);
            this.Characters[cha.Id] = character;    

            OnCharacterEnter?.Invoke(character);
        }
    }
}