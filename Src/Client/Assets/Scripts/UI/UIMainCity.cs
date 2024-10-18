using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Services;

public class UIMainCity : MonoBehaviour
{
    public Text avatarName;
    public Text avatarLevel;
    public Image avatarImage;

    // Start is called before the first frame update
    void Start()
    {
        this.UpdateAvatar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}", User.Instance.CurrentCharacter.Name);
        this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
    }

    public void BackToCharacterSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        UserService.Instance.SendGameLeave();
    }
}
