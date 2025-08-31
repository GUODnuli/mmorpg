using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Common.Data;
using UnityEngine.EventSystems;

public class UISkillItem : ListView.ListViewItem, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Text skillName;
    public Image backgroud;
    public Sprite normalBg;
    public Sprite selectedBg;
    public Image skillIcon;
    public Text level;
    public Skill skill;

    // 拖拽相关变量
    private Vector3 originalPosition;
    private Transform originalParent;
    private bool isDragging = false;

    // 拖拽预览图标
    private GameObject dragIcon;

    // 事件：当技能被成功装备时触发
    public System.Action<UISkillItem, int> OnSkillEquipped;


    public override void onSelected(bool selected)
    {
        this.backgroud.overrideSprite = selected ? normalBg : selectedBg;
    }

    public void SetSkillInfo(Skill item)
    {
        this.skill = item;

        if (this.skillName != null)
        {
            this.skillName.text = this.skill.Define.Name;
        }
        if (this.skillIcon != null)
        {
            this.skillIcon.overrideSprite = Resloader.Load<Sprite>(this.skill.Define.Icon);
        }
        if (this.level != null)
        {
            this.level.text = this.skill.Define.UnlockLevel.ToString();
        }
    }

    // 开始拖拽
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 记录原始位置和父级
        originalPosition = transform.position;
        originalParent = transform.parent;

        // 创建拖拽图标
        CreateDragIcon();

        // 设置拖拽状态
        isDragging = true;

        // 将拖拽对象移动到UI顶层
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        // 更新拖拽图标位置
        if (dragIcon != null)
        {
            dragIcon.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        isDragging = false;
        
        if (dragIcon != null)
        {
            Destroy(dragIcon);
            dragIcon = null;
        }

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            UISkillSlot slot = result.gameObject.GetComponent<UISkillSlot>();
            if (slot != null)
            {
                OnSkillEquipped?.Invoke(this, slot.SlotIndex);
                break;
            }
        }
    }

    // 创建拖拽显示的图标
    private void CreateDragIcon()
    {
        if (skillIcon.sprite == null) return;
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(transform.root, false);
        dragIcon.transform.SetAsLastSibling();
        Image image = dragIcon.AddComponent<Image>();
        image.sprite = skillIcon.sprite;
        image.raycastTarget = false;

        // 设置图标大小
        RectTransform rectTransform = dragIcon.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(50, 50);
    }
}