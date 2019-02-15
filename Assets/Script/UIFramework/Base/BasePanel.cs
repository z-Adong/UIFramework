/**
 * Created by zhang on 2019/1/26
 */
using UnityEngine;
//基类:所有面板继承自该类
//定义了四个事件:
//OnEnter:面板进入时调用
//OnPause:面板停止时调用<鼠标与面板的交互停止>
//OnResume:面板恢复使用时调用<鼠标与面板的交互恢复>
//OnExit:面板退出时调用
public abstract class BasePanel : MonoBehaviour
{
    public abstract void OnEnter();
    public abstract void OnPause();
    public abstract void OnResume();
    public abstract void OnExit();

}