using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInputBox : MonoBehaviour {
    public Text title;
    public Text tips;
    public string emptyTips;
    public InputField input;
    public Button buttonSubmit;
    public Button buttonCancel;

    public Text buttonSubmitTitle;
    public Text buttonCancelTitle;

    public delegate bool SubmitHandler(string inputText, out string tips);
    public event SubmitHandler OnSubmit;
    public UnityAction OnCancel;
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(string title, string btnSubmit = "", string btnCancel = "", string emptyTips = "")
    {
        if (!string.IsNullOrEmpty(title))
        {
            this.title.text = title;
        }
        this.tips.text = null;
        this.OnSubmit = null;
        this.emptyTips = emptyTips;

        if (!string.IsNullOrEmpty(btnSubmit)) this.buttonSubmitTitle.text = title;
        if (!string.IsNullOrEmpty(btnCancel)) this.buttonCancelTitle.text = title;

        this.buttonSubmit.onClick.AddListener(OnClickSubmit);
        this.buttonCancel.onClick.AddListener(OnClickCancel);
    }

    void OnClickSubmit()
    {
        this.tips.text = "";
        if (string.IsNullOrEmpty(input.text))
        {
            this.tips.text = this.emptyTips;
            return;
        }
        if (OnSubmit != null)
        {
            string tips;
            if (!OnSubmit(this.input.text, out tips))
            {
                this.tips.text = tips;
                return;
            }
        }
        Destroy(this.gameObject);
    }

    void OnClickCancel()
    {
        Destroy(this.gameObject);
        this.OnCancel?.Invoke();
    }
}
