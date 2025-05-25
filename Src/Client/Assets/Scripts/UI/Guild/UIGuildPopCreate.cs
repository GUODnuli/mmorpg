using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildPopCreate : UIWindow
{
    public InputField inputName;
    public InputField inputNotice;

    private void Start()
    {
        GuildService.Instance.OnGuildCreateResult = OnGuildCreated;
    }


}
