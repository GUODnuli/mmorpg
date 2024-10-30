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

    private void Start()
    {
        MiniMapManager.Instance.miniMap = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null)
        {
            this.playerTransform = MiniMapManager.Instance.PlayerTransform;
        }

        if (miniMapBoundBox == null || playerTransform == null)
        {
            return;
        }
        float relativePosX = playerTransform.position.x - miniMapBoundBox.bounds.min.x;
        float relativePosY = playerTransform.position.z - miniMapBoundBox.bounds.min.z;

        float pivotX = relativePosX / this.realWidth;
        float pivotY = relativePosY / this.realHeight;

        this.miniMap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        this.miniMap.rectTransform.localPosition = Vector2.zero;
        this.arrorw.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);
    }

    internal void UpdateMap()
    {
        this.mapName.text = User.Instance.CurrentMap.Name;
        this.miniMap.overrideSprite = MiniMapManager.Instance.LoadCurrentMiniMap();
        this.miniMap.SetNativeSize();
        this.miniMap.transform.localPosition = Vector3.zero;
        this.miniMapBoundBox = MiniMapManager.Instance.MiniMapBoundingBox;
        this.playerTransform = null;
        this.realWidth = this.miniMapBoundBox.bounds.size.x;
        this.realHeight = this.miniMapBoundBox.bounds.size.z;
    }
}
