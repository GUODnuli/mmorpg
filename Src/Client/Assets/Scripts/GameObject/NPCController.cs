using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
using Managers;
using Models;

public class NPCController : MonoBehaviour
{
    public int NPCId;
    Animator animator;
    SkinnedMeshRenderer renderer;
    Color originColor;

    private bool inInteractive = false;

    NPCDefine npc;

    NpcQuestStatus questStatus;

    void Start()
    {
        renderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        animator = this.gameObject.GetComponent<Animator>();
        originColor = renderer.sharedMaterial.color;
        npc = NPCManager.Instance.GetNPCDefine(NPCId);
        this.StartCoroutine(Actions());
        RefreshNpcStatus();
        QuestManager.Instance.onQuestStatusChanged += OnQuestStatusChanged;
    }

    private void OnDestroy()
    {
        QuestManager.Instance.onQuestStatusChanged -= OnQuestStatusChanged;
        if (UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveNpcQuestStatus(this.transform);
        }
    }

    IEnumerator Actions()
    {
        while (true)
        {
            if (inInteractive)
            {
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }

            this.Relax();
        }
    }

    private void Relax()
    {
        animator.SetTrigger("Relax");
    }

    private void Interactive()
    {
        if (!inInteractive)
        {
            inInteractive = true;
            StartCoroutine(DoInteractive());
        }
    }

    IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();
        if (NPCManager.Instance.Interactive(npc))
        {
            animator.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inInteractive = false;
    }

    IEnumerator FaceToPlayer()
    {
        Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - transform.position).normalized;
        while (Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward, faceTo)) > 5)
        {
            this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }

    private void OnMouseDown()
    {
        if (Vector3.Distance(this.transform.position, User.Instance.CurrentCharacterObject.transform.position) > 2f)
        {
            User.Instance.CurrentCharacterObject.StartNav(this.transform.position);
        }
        Interactive();
    }

    private void OnMouseOver()
    {
        Highlight(true);
    }

    private void OnMouseEnter()
    {
        Highlight(true);
    }

    private void OnMouseExit()
    {
        Highlight(false);
    }

    private void Highlight(bool highlight)
    {
        if (!highlight && renderer.sharedMaterial.color != originColor)
        {
            renderer.sharedMaterial.color = originColor;
        }

        if (highlight && renderer.sharedMaterial.color != Color.white)
        {
            renderer.sharedMaterial.color = Color.white;
        }
    }

    private void OnQuestStatusChanged(Quest quest)
    {
        RefreshNpcStatus();
    }

    private void RefreshNpcStatus()
    {
        questStatus = QuestManager.Instance.GetNpcQuestStatus(NPCId);
        if (UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.AddNpcQuestStatus(this.transform, questStatus);
        }
    }
}
