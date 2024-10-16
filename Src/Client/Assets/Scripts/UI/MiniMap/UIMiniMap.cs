using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Managers;

public class UIMiniMap : MonoBehaviour
{
    public Image miniMap;
    public Image arrorw;
    public Text mapName;
    public Collider miniMapBoundBox;

    public Transform playerTransform;

    private float realWidth;
    private float realHeight;

    // Start is called before the first frame update
    void Start()
    {
        InitMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null && User.Instance.CurrentCharacterObject != null)
        {
            this.playerTransform = User.Instance.CurrentCharacterObject.transform;
        }

        if (miniMapBoundBox == null || playerTransform == null)
        {
            return;
        }
        float relativePosX = playerTransform.position.x - miniMapBoundBox.bounds.min.x;
        float relativePosY = playerTransform.position.z - miniMapBoundBox.bounds.min.z;

        float pivotX = relativePosX / realWidth;
        float pivotY = relativePosY / realHeight;

        this.miniMap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        this.miniMap.rectTransform.localPosition = Vector2.zero;
        this.arrorw.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);
    }

    private void InitMap()
    {
        this.mapName.text = User.Instance.CurrentMap.Name;
        if (this.miniMap.overrideSprite == null)
        {
            this.miniMap.overrideSprite = MiniMapManager.Instance.LoadCurrentMiniMap();
        }
        this.miniMap.SetNativeSize();
        this.miniMap.transform.localPosition = Vector3.zero;

        realWidth = miniMapBoundBox.bounds.size.x;
        realHeight = miniMapBoundBox.bounds.size.z;
    }
}
