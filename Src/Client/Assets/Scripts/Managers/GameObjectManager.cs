using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using SkillBridge.Message;
using Models;

namespace Managers
{
    public class GameObjectManager : MonoSingleton<GameObjectManager>
    {
        Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();

        // Use this for initialization
        void Awake()
        {
            Debug.Log("GameObjectManager Awake: " + this.GetInstanceID());
        }

        protected override void OnStart()
        {
            Debug.Log("GameObjectManager OnStart: " + this.GetInstanceID());
            StartCoroutine(InitGameObjects());
            CharacterManager.Instance.OnCharacterEnter += OnCharacterEnter;
            CharacterManager.Instance.OnCharacterLeave += OnCharacterLeave;
        }

        private void OnDestroy()
        {
            Debug.Log("GameObjectManager OnDestroy: " + this.GetInstanceID());
            CharacterManager.Instance.OnCharacterEnter -= OnCharacterEnter;
            CharacterManager.Instance.OnCharacterLeave -= OnCharacterLeave;
        }

        private void OnCharacterEnter(Character character)
        {
            CreateCharacterObject(character);
        }

        private void OnCharacterLeave(Character character)
        {
            DestroyCharacterObject(character);
        }

        IEnumerator InitGameObjects()
        {
            foreach (var cha in CharacterManager.Instance.Characters.Values)
            {
                CreateCharacterObject(cha);
                yield return null;
            }
        }

        private void CreateCharacterObject(Character character)
        {
            if (!Characters.ContainsKey(character.entityId) || Characters[character.entityId] == null)
            {
                Object obj = Resloader.Load<Object>(character.Define.Resource);
                if (obj == null)
                {
                    Debug.LogErrorFormat("Character: [{0}], Resource: [{1}] not existed.", character.Define.TID, character.Define.Resource);
                    return;
                }
                GameObject go = (GameObject)Instantiate(obj, this.transform);
                go.name = "Character_" + character.entityId + "_" + character.Info.Name;
                Characters[character.entityId] = go;

                UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);
            }
            this.InitGameObject(Characters[character.entityId], character);
        }

        private void DestroyCharacterObject(Character character)
        {
            if (!Characters.ContainsKey(character.entityId))
            {
                return;
            }

            if (Characters[character.entityId] != null)
            {
                Destroy(Characters[character.entityId]);
                this.Characters.Remove(character.entityId);
            }
        }

        private void InitGameObject(GameObject go, Character character)
        {
            go.transform.position = GameObjectTool.LogicToWorld(character.position);
            go.transform.forward = GameObjectTool.LogicToWorld(character.direction);
            EntityController ec = go.GetComponent<EntityController>();
            if (ec != null)
            {
                ec.entity = character;
                ec.isPlayer = character.IsPlayer;
            }

            PlayerInputController pc = go.GetComponent<PlayerInputController>();
            if (pc != null)
            {
                if (character.Info.Id == Models.User.Instance.CurrentCharacter.Entity.Id)
                {
                    User.Instance.CurrentCharacterObject = go;
                    MainPlayerCamera.Instance.player = go;
                    pc.enabled = true;
                    pc.character = character;
                    pc.entityController = ec;
                }
                else
                {
                    pc.enabled = false;
                }
            }
        }
    }
}