using Managers;
using SkillBridge.Message;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.Battle;

public class UICharEquip : UIWindow
{
    public Text title;

    public GameObject itemPrefab;
    public GameObject itemEquipedPrefab;

    public Transform itemListRoot;

    public List<Transform> slots;

    public Text hp;
    public Slider hpSlider;
    public Text mp;
    public Slider mpSlider;

    public Text[] attrs;

    private void Start()
    {
        RefreshUI();
        EquipManager.Instance.OnEquipChanged += RefreshUI;
    }

    private void OnDestroy()
    {
        EquipManager.Instance.OnEquipChanged -= RefreshUI;
    }

    void RefreshUI()
    {
        ClearAllEquipList();
        InitAllEquipItems();
        ClearEquipedList();
        InitEquipedItems();
        InitAttributes();
    }

    void InitAllEquipItems()
    {
        foreach(var kv in ItemManager.Instance.Items)
        {
            if (kv.Value.Define.Type == ItemType.Equip)
            {
                if (EquipManager.Instance.Contains(kv.Key))
                    continue;
                GameObject go = Instantiate(itemPrefab, itemListRoot);
                UIEquipItem ui = go.GetComponent<UIEquipItem>();
                ui.SetEquipItem(kv.Key, kv.Value, this, false);
            }
        }
    }

    void ClearAllEquipList()
    {
        foreach(var item in itemListRoot.GetComponentsInChildren<UIEquipItem>())
        {
            Destroy(item.gameObject);
        }
    }

    void ClearEquipedList()
    {
        foreach (var item in slots)
        {
            if (item.childCount > 0)
                Destroy(item.GetChild(0).gameObject);
        }
    }

    void InitEquipedItems()
    {
        for (int i = 0; i < (int)EquipSlot.SlotMax; i++)
        {
            var item = EquipManager.Instance.Equips[i];
            if (item != null)
            {
                GameObject go = Instantiate(itemEquipedPrefab, slots[i]);
                UIEquipItem ui = go.GetComponent<UIEquipItem>();
                ui.SetEquipItem(i, item, this, true);
            }
        }
    }

    void InitAttributes()
    {
        var charAttr = User.Instance.CurrentCharacter.Attributes;
        if (this.hp.text != null)
        {
            string.Format("{0}/{1}", charAttr.HP, charAttr.MaxHP);
        }
        if (this.mp.text != null)
        {
            string.Format("{0}/{1}", charAttr.MP, charAttr.MaxMP);
        }
        if (this.hpSlider != null)
        {
            this.hpSlider.maxValue = charAttr.MaxHP;
            this.hpSlider.value = charAttr.HP;
        }
        if (this.mpSlider != null)
        {
            this.mpSlider.maxValue = charAttr.MaxMP;
            this.mpSlider.value = charAttr.MP;
        }

        for (int i = (int)AttributeType.STR; i < (int)AttributeType.STR; i++)
        {
            if (i == (int)AttributeType.CRI)
            {
                this.attrs[i - 2].text = string.Format("{0.f2}%", charAttr.Final.Data[i] * 100);
            }
            else
            {
                this.attrs[i - 2].text = ((int)charAttr.Final.Data[i]).ToString();
            }
        }
    }

    public void DoEquip(Item item) 
    {
        EquipManager.Instance.EquipItem(item);
    }

    public void UnEquip(Item item)
    {
        EquipManager.Instance.UnequipItem(item);
    }
}