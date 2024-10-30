using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
using Managers;

public class NPCController : MonoBehaviour
{
    public int NPCId;
    Animator animator;
    NPCDefine npc;

    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        npc = NPCManager.Instance.GetNPCDefine(NPCId);
    }

    private void OnMouseDown()
    {
        NPCManager.Instance.Interactive(NPCId);
    }
}
