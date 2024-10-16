using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;

public class UICharBar : MonoBehaviour
{
    public Text Name;
    public Character character;

    // Start is called before the first frame update
    void Start()
    {
        if (this.character != null)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateInfo();

        if (this.transform != null & Camera.main.transform != null)
        {
            this.transform.forward = Camera.main.transform.forward;
        }
    }

    private void UpdateInfo()
    {
        if (this.character != null)
        {
            string name = this.character.Name + " Lv." + this.character.Info.Level;
            if (name != this.Name.text)
            {
                this.Name.text = name;
            }
        }
    }
}
