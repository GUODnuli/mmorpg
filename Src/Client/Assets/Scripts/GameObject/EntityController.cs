using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using SkillBridge.Message;
using Managers;

public class EntityController : MonoBehaviour, IEntityNotify
{
    public Animator anim;
    public Rigidbody rb;
    private AnimatorStateInfo currentBaseState;

    public Entity entity;

    public Vector3 position;
    public Vector3 direction;
    Quaternion rotation;

    public Vector3 lastPosition;
    Quaternion lastRotation;

    public float speed;
    public float animSpeed = 1.5f;
    public float jumpPower = 3.0f;

    public bool isPlayer = false;

    public RideController rideController;
    private int currentRide = 0;
    public Transform rideBone;

    // Use this for initialization
    private void Start()
    {
       if (entity != null)
        {
            EntityManager.Instance.RegisterEntityChangeNotify(entity.entityId, this);
            this.UpdateTransform();
        }

       if (!this.isPlayer)
        {
            rb.useGravity = false;
        }
    }

    private void UpdateTransform()
    {
        this.position = GameObjectTool.LogicToWorld(entity.position);
        this.direction = GameObjectTool.LogicToWorld(entity.position);

        this.rb.MovePosition(this.position);
        this.transform.forward = this.direction;
        this.lastPosition = this.position;
        this.lastRotation = this.rotation;
    }

    private void OnDestory()
    {
        if (entity != null)
        {
            Debug.LogFormat("{0} OnDestroy: Entity ID: {1}, Entity Pos: {2}, Entity Dir: {3}, Entity Speed: {4}", this.name, entity.entityId, entity.position, entity.direction, entity.speed);
        }

        if (UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveCharacterNameBar(this.transform);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (entity == null)
        {
            return;
        }

        this.entity.OnUpdate(Time.fixedDeltaTime);

        if (!this.isPlayer)
        {
            this.UpdateTransform();
        }
    }

    public void OnEntityEvent(EntityEvent entityEvent, int param)
    {
        switch(entityEvent)
        {
            case EntityEvent.Idle:
                anim.SetBool("Move", false);
                anim.SetTrigger("Idle");
                break;
            case EntityEvent.MoveFwd:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.MoveBack:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.Jump:
                anim.SetTrigger("Jump");
                break;
            case EntityEvent.Ride:
                {
                    this.Ride(param);
                }
            break;
        }
        if (this.rideController != null)
        {
            this.rideController.OnEntityEvent(entityEvent, param);
        }
    }

    public void OnEntityRemoved()
    {
        if (UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveCharacterNameBar(this.transform);
        }
        Destroy(this.gameObject);
    }

    public void OnEntityChanged(Entity entity)
    {
        Debug.LogFormat("OnEntityChanged: Entity ID: {0}, Entity Pos: {1}, Entity Dir: {2}, Entity Speed: {3}", entity.entityId, entity.position, entity.direction, entity.speed);
    }

    public void Ride(int rideID)
    {
        if (currentRide == rideID) return;

        currentRide = rideID;
        if (rideID > 0)
        {
            this.rideController = GameObjectManager.Instance.LoadRide(rideID, this.transform);
        }
        else
        {
            Destroy(this.rideController.gameObject);
            this.rideController = null;
        }

        if (rideController == null)
        {
            this.anim.transform.localPosition = Vector3.zero;
            this.anim.SetLayerWeight(1, 0);
        }
        else
        {
            this.rideController.SetRider(this);
            this.anim.SetLayerWeight(1, 1);
        }
    }

    public void SetRidePosition(Vector3 position)
    {
        this.anim.transform.position = position + (this.anim.transform.position + this.rideBone.position);
    }
}