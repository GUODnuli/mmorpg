using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Models;

public class UISkillSlot : MonoBehaviour, IDropHandler
{
    public int SlotIndex;
    public Image iconImage;
    public Skill skill;

    public void OnDrop(PointerEventData eventData)
    {
        UISkillItem item = eventData.pointerDrag.GetComponent<UISkillItem>();
        if (item != null)
        {
            if (iconImage != null && item.skillIcon != null)
            {
                iconImage.sprite = item.skillIcon.sprite;
                skill = item.skill;
                iconImage.enabled = true;
            }
        }

    }

    // Çå¿Õ²ÛÎ»
    public void ClearSlot()
    {
        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }
    }
}