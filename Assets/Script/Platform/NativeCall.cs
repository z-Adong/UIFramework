/**
 * Created by zhang on 2019/1/26
 */
using UnityEngine;

/// <summary>
/// 通信中转类—区分平台接口
/// </summary>
public class NativeCall
{
    public static void CallVoid()
    {
#if UNITY_EDITOR
        return;
#elif UNITY_IOS
        Define_IOS.IOS_CallVoid();
#elif UNITY_ANDROID
        Define_Android.CallVoid();
#endif
    }
}