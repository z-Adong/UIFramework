/**
 * Created by zhang on 2019/1/26
 */
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
//框架的核心 单例
//进行解析面板信息json文件，实例化面板Prefab，面板的入栈和出栈等等一系列操作
public class UIPanelManager
{
    private static UIPanelManager _instance;
    private Transform canvasTransform;
    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }
    public static UIPanelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIPanelManager();
            }


            return _instance;
        }
    }

    private Dictionary<string, string> panelPathDict;
    private Dictionary<string, BasePanel> panelDict;
    private Stack<BasePanel> panelStack;

    private UIPanelManager()
    {
        ParseUIPanelTypeJson();
    }

    public void PushPanel(string panelType)
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }

        //停止上一个界面
        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(panelType);
        panelStack.Push(panel);
        panel.OnEnter();
    }

    public void PopPanel()
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }
        if (panelStack.Count <= 0)
        {
            return;
        }

        //退出栈顶面板
        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();

        //恢复上一个面板
        if (panelStack.Count > 0)
        {
            BasePanel panel = panelStack.Peek();
            panel.OnResume();
        }
    }

    private BasePanel GetPanel(string panelType)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<string, BasePanel>();
        }

        BasePanel panel = panelDict.GetValue(panelType);

        //如果没有实例化面板，寻找路径进行实例化，并且存储到已经实例化好的字典面板中
        if (panel == null)
        {
            string path = panelPathDict.GetValue(panelType);
            GameObject panelGo = GameObject.Instantiate(Resources.Load<GameObject>(path), CanvasTransform, false);
            panel = panelGo.GetComponent<BasePanel>();
            panelDict.Add(panelType, panel);
        }
        return panel;
    }

    //解析json文件
    private void ParseUIPanelTypeJson()
    {
        panelPathDict = new Dictionary<string, string>();
        string json = Resources.Load<TextAsset>("UIPanelTypeJson").text;
        UIPanelInfoList panelInfoList = new UIPanelInfoList();
        JsonUtility.FromJsonOverwrite(json, panelInfoList);
   
        foreach (UIPanelInfo panelInfo in panelInfoList.panelInfoList)
        {
            panelPathDict.Add(panelInfo.panelType, panelInfo.path);
        }
    }
}