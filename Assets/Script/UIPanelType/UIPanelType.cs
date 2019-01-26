/**
 * Created by zhang on 2019/1/26
 */
//面板类型
//用来记录面板的类型
//每添加一个面板都要在此类里面添加对应面板的类型
[System.Serializable]
public class UIPanelType {
    public const string MainMenu = "MainMenu";
    public const string PrefaberMessage = "PrefaberMessage";
    public const string SpriteMessage = "SpriteMessage";
    public const string ItemMessage = "ItemMessage";
    public const string IconMessage = "IconMessage";
}