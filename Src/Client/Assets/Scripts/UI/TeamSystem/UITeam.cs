using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using Services;
using UnityEngine.UI;

public class UITeam : MonoBehaviour
{
    public Text teamTitle;
    public UITeamItem[] Members;
    public ListView list;

    private void Start()
    {
        if (User.Instance.TeamInfo == null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        foreach (var item in Members)
        {
            this.list.AddItem(item);
        }
    }

    private void OnEnable()
    {
        UpdateTeamUI();
    }

    public void ShowTeam(bool show)
    {
        this.gameObject.SetActive(show);
        if (show)
        {
            UpdateTeamUI();
        }
    }

    public void UpdateTeamUI()
    {
        if (User.Instance.TeamInfo == null) return;
        this.teamTitle.text = string.Format("ÎÒµÄ¶ÓÎé ({0}/5)", User.Instance.TeamInfo.Members.Count);

        for(int i = 0; i < 5; i++)
        {
            if (i < User.Instance.TeamInfo.Members.Count)
            {
                this.
            }
        }
    }
}
