using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;
using System.Linq;

public class UICharacterView : MonoBehaviour
{
    public GameObject[] characters;
    private Dictionary<string, GameObject> characterDict;
    private CharacterClass currentCharacter = CharacterClass.None;
    private CharacterClass oldCharacter = CharacterClass.None;
    public CharacterClass CurrentCharacter
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
        characterDict = new Dictionary<string, GameObject>();
        foreach (GameObject character in characters)
        {
            characterDict[character.name] = character;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateCurrentCharacter ()
    {
        if (currentCharacter != oldCharacter)
        {
            if (oldCharacter != CharacterClass.None)
            {
                characters.FirstOrDefault(character => character.name == oldCharacter.ToString()).SetActive(false);
            }
            characters.FirstOrDefault(character => character.name == currentCharacter.ToString()).SetActive(true);
        }
    }
}
