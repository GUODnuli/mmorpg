using UnityEngine;

class InputBox
{
    static Object cacheObject = null;

    public static UIInputBox Show(string title = "", string btnSubmit = "", string btnCancel = "", string emptyTips = "")
    {
        if (cacheObject == null)
        {
            cacheObject = Resloader.Load<Object>("UI/UIInputBox");
        }

        GameObject go = (GameObject)GameObject.Instantiate(cacheObject);
        UIInputBox inputBox = go.GetComponent<UIInputBox>();
        inputBox.Init(title, btnSubmit, btnCancel, emptyTips);
        return inputBox;
    }
}