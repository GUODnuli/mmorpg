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

    // ��ק��ر���
    private Vector3 originalPosition;
    private Transform originalParent;
    private bool isDragging = false;

    // ��קԤ��ͼ��
    private GameObject dragIcon;

    // �¼��������ܱ��ɹ�װ��ʱ����
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

    // ��ʼ��ק
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ��¼ԭʼλ�ú͸���
        originalPosition = transform.position;
        originalParent = transform.parent;

        // ������קͼ��
        CreateDragIcon();

        // ������ק״̬
        isDragging = true;

        // ����ק�����ƶ���UI����
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        // ������קͼ��λ��
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

    // ������ק��ʾ��ͼ��
    private void CreateDragIcon()
    {
        if (skillIcon.sprite == null) return;
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(transform.root, false);
        dragIcon.transform.SetAsLastSibling();
        Image image = dragIcon.AddComponent<Image>();
        image.sprite = skillIcon.sprite;
        image.raycastTarget = false;

        // ����ͼ���С
        RectTransform rectTransform = dragIcon.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(50, 50);
    }
}