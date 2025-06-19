using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using SkillBridge.Message;
using Managers;
using Services;

public class PlayerInputController : MonoBehaviour
{
    public Rigidbody rb;
    CharacterState state;

    public Character character;

    public float rotateSpeed = 2.0f;
    public float turnAngle = 10;
    public int speed;

    public EntityController entityController;

    public bool onAir = false;

    // Use this for initialization
    private void Start()
    {
        state = CharacterState.Idle;
        if (this.character == null)
        {
            DataManager.Instance.Load();
            NCharacterInfo chainfo = new NCharacterInfo();
            //{
            //    Id = 1,
            //    Name = "Test",
            //    ConfigId = 1,
            //    Entity = new NEntity
            //    {
            //        Position = new NVector3(),
            //        Direction = new NVector3
            //        {
            //            X = 0,
            //            Y = 100,
            //            Z = 0
            //        },
            //    },
            //};
            this.character = new Character(chainfo);

            if (entityController != null)
            {
                entityController.entity = this.character;
            }
        }
    }

    private void FixedUpdate()
    {
        if (character == null)
        {
            return;
        }

        if (InputManager.Instance != null && InputManager.Instance.IsInputMode) return;

        if (InputManager.Instance.IsInputMode)
        {
            return;
        }

        float v = Input.GetAxis("Vertical");
        if (v > 0.01)
        {
            if (state != CharacterState.Move)
            {
                state = CharacterState.Move;
                this.character.MoveForward();
                this.SendEntityEvent(EntityEvent.MoveFwd);
            }
            this.rb.velocity = GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;
        }
        else if (v < -0.01) {
            if (state != CharacterState.Move)
            {
                state = CharacterState.Move;
                this.character.MoveBack();
                this.SendEntityEvent(EntityEvent.MoveBack);
            }
            this.rb.velocity = GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;
        }
        else
        {
            if (state != CharacterState.Idle)
            {
                state = CharacterState.Idle;
                this.rb.velocity = Vector3.zero;
                this.character.Stop();
                this.SendEntityEvent(EntityEvent.Idle);
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            this.SendEntityEvent(EntityEvent.Jump);
        }

        float h = Input.GetAxis("Horizontal");
        if (h < -0.1 || h > 0.1)
        {
            this.transform.Rotate(0, h * rotateSpeed, 0);
            Vector3 dir = GameObjectTool.LogicToWorld(character.direction);
            Quaternion rot = new();
            rot.SetFromToRotation(dir, this.transform.forward);

            if (rot.eulerAngles.y > this.turnAngle && rot.eulerAngles.y < (360 - this.turnAngle))
            {
                character.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward));
                rb.transform.forward = this.transform.forward;
                this.SendEntityEvent(EntityEvent.None);
            }
        }
    }

    Vector3 lastPos;
    //float lastSync = 0;
    private void LateUpdate()
    {
        Vector3 offset = this.rb.transform.position - lastPos;
        this.speed = (int)(offset.magnitude * 100f / Time.deltaTime);
        this.lastPos = this.rb.transform.position;

        if ((GameObjectTool.WorldToLogic(this.rb.transform.position) - this.character.position).magnitude > 200)
        {
            this.character.SetPosition(GameObjectTool.WorldToLogic(this.rb.transform.position));
            this.SendEntityEvent(EntityEvent.None);
        }
        this.transform.position = this.rb.transform.position;
    }

    public void SendEntityEvent(EntityEvent entityEvent, int param = 0)
    {
        if (entityController != null)
        {
            entityController.OnEntityEvent(entityEvent, param);
        }
        MapService.Instance.SendMapEntitySync(entityEvent, this.character.EntityData, param);
    }
}