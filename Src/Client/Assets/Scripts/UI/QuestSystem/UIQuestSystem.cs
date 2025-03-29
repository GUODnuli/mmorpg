using Common.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIQuestSystem : UIWindow
{
    public Text title;

    public GameObject itemPrefab;

    public TabView Tabs;
    public ListView listMain;
    public ListView listside;

    public UIQuestInfo questInfo;

    private bool showAvailableList = false;

    private void Start()
    {
        this.listMain.onItemSelected += this.OnQuestSelected;
        this.listside.onItemSelected += this.OnQuestSelected;
        this.Tabs.OnTabSelect += this.OnTabSelected;
        RefreshUI();
    }

    private void OnDestroy()
    {
        
    }

    public void OnQuestSelected(ListView.ListViewItem item)
    {
        UIQuestItem questItem = item as UIQuestItem;
        this.questInfo.SetQuestInfo(questItem.quest);
    }

    private void OnTabSelected(int index)
    {
        this.showAvailableList = index == 1;
        RefreshUI();
    }

    private void RefreshUI()
    {
        CleanAllQuestList();
        InitAllQuestItems();
    }

    private void CleanAllQuestList()
    {
        this.listMain.RemoveAll();
        this.listside.RemoveAll();
    }

    private void InitAllQuestItems()
    {
        foreach (var quest in QuestManager.Instance.allQuest)
        {
            if (this.showAvailableList && quest.Info != null)
            {
                continue;
            }
            if (!this.showAvailableList && quest.Info == null)
            {
                continue;
            }
            GameObject go = Instantiate(this.itemPrefab, quest.Value.Define.Type == QuestType.Main ? this.listMain.transform : this.listside.transform);
            UIQuestItem ui = go.GetComponent<UIQuestItem>();
            ui.SetQuestInfo(quest);
            if (quest.Value.Define.Type == QuestType.Main)
            {
                this.listMain.AddItem(ui);
            }
            else
            {
                this.listside.AddItem(ui);
            }
        }
    }
}