using UnityEngine;

public class Global : MonoBehaviour {

    public static Global instance;
    [HideInInspector]//密钥
    public string m_UserSession;

    static Global()
    {
        GameObject go = new GameObject("Global");
        DontDestroyOnLoad(go);
        instance = go.AddComponent<Global>();
    }
}