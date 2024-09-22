using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Network;

public class Login : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Network.NetClient.Instance.Init("127.0.0.1", 8000);
        Network.NetClient.Instance.Connect();

        SkillBridge.Message.NetMessage msg = new();
        msg.Request = new SkillBridge.Message.NetMessageRequest
        {
            firstRequest = new SkillBridge.Message.FirstTestRequest
            {
                Helloworld = "Hello World"
            }
        };
        Network.NetClient.Instance.SendMessage(msg);
    }
}
