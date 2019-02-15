/**
 * Created by zhang on 2019/1/26
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//实例化游戏中的主面板
public class UIMain : MonoBehaviour {
    UIPanelManager panelManager;
    void Start () {

        InitPlatform();
        //UI基本面板
        panelManager = UIPanelManager.Instance;
        panelManager.PushPanel(UIPanelType.MainMenu);

        //通信原生
        NativeCall.CallVoid();
    }
    //初始化原生
    public void InitPlatform()
    {
#if UNITY_EDITOR
        return;
#elif UNITY_ANDROID
        Define_Android.Instance.Init();
#endif
    }
}