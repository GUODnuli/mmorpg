using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabButton : MonoBehaviour
{
    public UITabView tabView;
    public Sprite activeImage;
    private Sprite normalImage;

    public int tabIndex = 0;
    public bool selected = false;
    private Image tabImage;

    private void Start()
    {
        tabImage = this.GetComponent<Image>();
        normalImage = tabImage.sprite;

        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Select(bool select)
    {
        tabImage.overrideSprite = select ? activeImage : normalImage;
    }

    private void OnClick()
    {
        this.tabView.SelectTab(this.tabIndex);
    }
}
