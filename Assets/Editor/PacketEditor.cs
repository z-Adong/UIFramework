/**
 * Created by zhang on 2019/1/26
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

//AssetBundle打包封装
public class PacketEditor
{
    //Mac打包路径需要自己改掉XXX XXX为电脑用户名
    public static string macPath = "/Users/XXX/Desktop/Packaging";
    public static string windowsPath = "E:/Edisk";

    [MenuItem("Assets/1.生成文件")]
    static void CreateAssetText()
    {
        Caching.ClearCache();
        //获取在Project视图中选择的所有游戏对象
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        ModelData bd = new ModelData();
        //遍历所有的游戏对象
        for (int i = 0; i < SelectedAsset.Length; ++i)
        {
            Object obj = SelectedAsset[i];
            Debug.Log("Create AssetBunldes name :" + obj.name);
            if (obj.GetType() == typeof(GameObject))
            {
                bd.name = obj.name;
            }
            if (obj.GetType() == typeof(Texture2D))
            {
                bd.texName = obj.name;
            }
        }
        string json = JsonUtility.ToJson(bd);
        string[] nn = Selection.assetGUIDs;
        string selectName = AssetDatabase.GUIDToAssetPath(nn[0]);
        int index = selectName.LastIndexOf('/');

        string path = selectName.Substring(0, index);
        Debug.Log(path);
        CreateFile(path, "data.txt", json);
        AssetDatabase.Refresh();
    }


    public static void CreateFile(string path, string name, string info)
    {
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "//" + name);
        if (t.Exists)
        {
            t.Delete();
        }

        sw = t.CreateText();
        sw.WriteLine(info);
        sw.Close();
        sw.Dispose();
    }

    [MenuItem("Assets/2.MAC打包模型/IOS")]
    static void CreateSceneALL_MAC_IOS()
    {
        Caching.ClearCache();
        string path = macPath + "/IOS/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        BuildPipeline.BuildAssetBundles(path, 0, BuildTarget.iOS);
        AssetDatabase.Refresh();
    }
    [MenuItem("Assets/2.MAC打包模型/Android")]
    static void CreateSceneALL_MAC_Android()
    {
        Caching.ClearCache();
        string path = macPath + "/Android/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        BuildPipeline.BuildAssetBundles(path, 0, BuildTarget.Android);
        AssetDatabase.Refresh();
    }
    [MenuItem("Assets/2.MAC打包模型/Windows")]
    static void CreateSceneALL_MAC_Windows()
    {
        Caching.ClearCache();
        string path = macPath + "/Windows/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        BuildPipeline.BuildAssetBundles(path, 0, BuildTarget.StandaloneWindows);
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/3.Windows打包模型/IOS")]
    static void CreateSceneALL_Windows_IOS()
    {
        Caching.ClearCache();
        string path = windowsPath +"/iOS";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        BuildPipeline.BuildAssetBundles(path, 0, BuildTarget.iOS);
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/3.Windows打包模型/Android")]
    static void CreateSceneALL_Windows_Android()
    {
        Caching.ClearCache();
        string path = windowsPath +"/Android";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        BuildPipeline.BuildAssetBundles(path, 0, BuildTarget.Android);
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/3.Windows打包模型/Windows")]
    static void CreateSceneALL_Windows_Windows()
    {
        Caching.ClearCache();
        string path = windowsPath +"/Windows";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        BuildPipeline.BuildAssetBundles(path, 0, BuildTarget.StandaloneWindows);
        AssetDatabase.Refresh();
    }

    // [MenuItem("Assets/4.打包场景")]
	// static void CreateSceneALL()
	// {
	// 	//清空一下缓存
	// 	Caching.ClearCache();
	// 	Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
	// 	string targetPath = Application.dataPath + "/StreamingAssets/" + SelectedAsset[0].name + "."+ BuildTarget.ToString();
	// 	string[] nn = Selection.assetGUIDs;
	// 	string selectName = AssetDatabase.GUIDToAssetPath(nn[0]);
	// 	Debug.Log(selectName);
	// 	string[] levels = { selectName };
	// 	//打包场景
	// 	BuildPipeline.BuildPlayer(levels, targetPath, BuildTarget, BuildOptions.BuildAdditionalStreamedScenes);
	// 	AssetDatabase.Refresh();
	// }
}
