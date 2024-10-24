using Common.Data;
using Managers;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterObject : MonoBehaviour
{
    public int ID;
    Mesh mesh = null;
    // Start is called before the first frame update
    void Start()
    {
        this.mesh = GetComponent<MeshFilter>().sharedMesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInputController playerController = other.GetComponent<PlayerInputController>();
        if (playerController == null || !playerController.isActiveAndEnabled)
            return;

        TeleporterDefine td = DataManager.Instance.Teleporters[this.ID];
        if (td == null)
        {
            Debug.LogErrorFormat("TeleporterObject: Character [{0}] enter teleporter [{1}], but teleporterDefine not existed.", playerController.character.Info.Name, this.ID);
            return;
        }

        Debug.LogFormat("TeleporterObject: Character [{0}] enter teleporter [{1}: {2}].", playerController.character.Info.Name, td.ID, td.Name);
        if (td.LinkTo <= 0)
            return;

        if (DataManager.Instance.Teleporters.ContainsKey(td.LinkTo))
        {
            MapService.Instance.SendMapTeleport(this.ID);
        }
        else
        {
            Debug.LogErrorFormat("Teleporter ID: {0}, LinkTo ID: {1} error!", td.ID, td.LinkTo);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (this.mesh != null)
        {
            Gizmos.DrawWireMesh(this.mesh, this.transform.position + Vector3.up * this.transform.localScale.y * .5f, this.transform.rotation, this.transform.localScale);
        }
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.ArrowHandleCap(0, this.transform.position, this.transform.rotation, 1f, EventType.Repaint);
    }
#endif
}
