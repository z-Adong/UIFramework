/**
 * Created by zhang on 2019/1/26
 */
using System;
using System.Collections.Generic;

//面板信息集合类
//用于和json文件进行映射
[Serializable]
public class UIPanelInfoList
{
    public List<UIPanelInfo> panelInfoList = new List<UIPanelInfo>();
    public UIPanelInfoList() { }
}
//面板的信息类
//用于和json文件进行映射
[Serializable]
public class UIPanelInfo
{
    public string panelType;//面板的名称
    public string path;//面板Prefab的路径
    public UIPanelInfo() { }
}