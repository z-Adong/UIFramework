/**
 * Created by zhang on 2019/1/26
 */
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : BasePanel
{
    private GameObject prefaberButton;
    private GameObject spriteButton;
    private GameObject itemButton;
    private GameObject iconButton;
    private CanvasGroup canvasGroup;
    
    void Awake()
    {
        canvasGroup = transform.GetComponent<CanvasGroup>();

        prefaberButton = transform.Find("IconPanel/PrefaberButton").gameObject;
        spriteButton = transform.Find("IconPanel/SpriteButton").gameObject;
        itemButton = transform.Find("IconPanel/ItemButton").gameObject;
        iconButton = transform.Find("IconPanel/IconButton").gameObject;
        EventTriggerListener.Get(prefaberButton).onClick = OnPrefaberButtonClick;//给UI绑定事件
        EventTriggerListener.Get(spriteButton).onClick = OnSpriteButtonClick;//给UI绑定事件
        EventTriggerListener.Get(itemButton).onClick = OnItemButtonClick;//给UI绑定事件
        EventTriggerListener.Get(iconButton).onClick = OnIconButtonClick;//给UI绑定事件
    }

    public override void OnEnter()
    {
    }

    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }

    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }

    public override void OnExit()
    {
        
    }

    private void OnPrefaberButtonClick(GameObject go)
    {
        UIPanelManager.Instance.PushPanel(UIPanelType.PrefaberMessage);
    }
    private void OnSpriteButtonClick(GameObject go)
    {
        UIPanelManager.Instance.PushPanel(UIPanelType.SpriteMessage);
    }
    private void OnItemButtonClick(GameObject go)
    {
        UIPanelManager.Instance.PushPanel(UIPanelType.ItemMessage);
    }
    private void OnIconButtonClick(GameObject go)
    {
        UIPanelManager.Instance.PushPanel(UIPanelType.IconMessage);
    }
}