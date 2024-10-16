using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{
    public GameObject nameBarPrefab;
    public Dictionary<Transform, GameObject> elements = new Dictionary<Transform, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCharacterNameBar(Transform owner, Character character)
    {
        GameObject goNameBar = Instantiate(nameBarPrefab, this.transform);
        goNameBar.name = "NameBar_" + character.entityId;
        goNameBar.GetComponent<UIWorldElement>().owner = owner;
        goNameBar.GetComponent<UICharBar>().character = character;
        goNameBar.SetActive(true);
        this.elements[owner] = goNameBar;
    }

    public void RemoveCharacterNameBar(Transform owner)
    {
        if (this.elements.ContainsKey(owner))
        {
            Destroy(this.elements[owner]);
            this.elements.Remove(owner);
        }
    }
}
