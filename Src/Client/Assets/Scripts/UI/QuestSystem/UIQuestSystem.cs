using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIQuestSystem : UIWindow
{
    public Text title;

    public GameObject itemPrefab;
    public ListView listMain;
    public ListView listside;

    public UIQuestInfo questInfo;

    private bool showAvailableList = false;

    private void Start()
    {
        this.listMain.onItemSelected += this.OnQuestSelected;
    }

    public void OnQuestSelected(ListView.ListViewItem item)
    {
        UIQuestItem questItem = item as UIQuestItem;
        this.questInfo.SetQuestInfo(questItem.quest);
    }
}