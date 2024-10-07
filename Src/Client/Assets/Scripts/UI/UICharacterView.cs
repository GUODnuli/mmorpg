using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour
{
    public GameObject[] characters;
    private int currentCharacter = 0;
    private int oldCharacter = 0;
    public int CurrentCharacter
    {
        get
        {
            return currentCharacter;
        }
        set
        {
            oldCharacter = currentCharacter;
            currentCharacter = value;
            this.UpdateCurrentCharacter();      
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateCurrentCharacter ()
    {
        if (currentCharacter < characters.Length)
        {
            characters[oldCharacter].SetActive(false);
            characters[currentCharacter].SetActive(true);
        }
    }
}
