/**
 * Created by zhang on 2019/1/25
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Text;
//Click点击事件写法样本
//其他点击事件参考EventTriggerListener 不做过多演示
public class Main : MonoBehaviour {

	//UI
	[HideInInspector]
	public GameObject uiParent;

	private GameObject btn;

	private Text text;

	private int btnNum;


	//3D
	[HideInInspector]
	public GameObject cube3D;
	private int cubeNum;

	void Awake() {
		//UI
		uiParent = GameObject.Find("Canvas").gameObject;
        btn = uiParent.transform.Find("Button").gameObject;
		text = uiParent.transform.Find("Text").gameObject.GetComponent<Text>();

		//3D物体
		cube3D = GameObject.Find("Cube").gameObject;
	}

	void Start () {
		//UI
        EventTriggerListener.Get(btn).onClick = BtnClick;//给UI绑定事件

		//3D物体
		EventTriggerListener.Get(cube3D).onClick = CubeClick;//给3D物体绑定事件
	}

	//UI点击事件
	public void BtnClick(GameObject go)
	{
		int i = btnNum++;
	
  		StringBuilder sb = new StringBuilder();
		sb.Append("点击了");
		sb.Append(go.name);
		sb.Append("按钮");
		sb.Append(i.ToString());

		text.text = sb.ToString();
	}
	//3D物体点击事件
	public void CubeClick(GameObject go)
	{
		int i = cubeNum++;

  		StringBuilder sb = new StringBuilder();
		sb.Append("点击了");
		sb.Append(go.name);
		sb.Append("物体");
		sb.Append(i.ToString());

		text.text = sb.ToString();
	}

}
