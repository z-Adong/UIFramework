/**
 * Created by zhang on 2019/1/26
 */
using UnityEngine;
using UnityEngine.UI;

public class ItemMessagePanel : BasePanel
{   
     private GameObject closeButton;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = transform.GetComponent<CanvasGroup>();

        closeButton = transform.Find("btn").gameObject;
        EventTriggerListener.Get(closeButton).onClick = ClosePlane;//给UI绑定事件
    }

    public override void OnEnter()
    {
        transform.gameObject.SetActive(true);
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
         transform.gameObject.SetActive(false);
    }

    public void ClosePlane(GameObject go)
    {
        UIPanelManager.Instance.PopPanel();
    }
}