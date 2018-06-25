using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeshArray : MonoBehaviour {

	public bool realTimeEdit=true;

	[Space(), Header("Configuration")]
	public int count=1;
	public Vector3 distOffset = Vector3.right;
	public Vector3 posOffset;
	public Vector3 rotationOffset;

	[Space(), Header("Prefabs")]
	public GameObject prefab;
	public GameObject iniPrefab;
	public GameObject endPrefab;

	[Space(), Header("Instances")]
	public List<GameObject> objs;
	public GameObject ini;
	public GameObject end;

	#if UNITY_EDITOR
	private void OnValidate() {
		if(realTimeEdit)
			UpdateArray();
	}

	public void UpdateArray(){
		int n = objs.Count - count; 
		for (int i = 0; i < n; i++) {
			Remove();
		}
		for (int i = 0; i > n; i--) {
			Add();
		}
		AdjustAll();
	}

	void Remove(){
		if(objs.Count <= 1) return;
		GameObject go = objs[objs.Count-1];
		UnityEditor.EditorApplication.delayCall +=()=>{
			DestroyImmediate(go);
		};
		objs.RemoveAt(objs.Count-1);
	}

	void Add(int i=-1){
		GameObject obj =  PrefabUtility.InstantiatePrefab(prefab) as GameObject;
		ObjArray orv = obj.GetComponent<ObjArray>();
		if(orv){
			orv.array = this;
		}
		obj.transform.position = transform.position;
		obj.transform.SetParent(transform,true);
		obj.transform.localScale = Vector3.one;
		if(i>=0)
			objs[i] = obj;
		else
			objs.Add(obj);
	}

	void AdjustAll(){
		for (int i = 0; i < objs.Count; i++){
			if(!objs[i])
				Add(i);
			AdjustPos(objs[i],i-1);
		}
		AdjustRotation();
		for (int i = 0; i < objs.Count; i++){
			objs[i].transform.Translate(posOffset,Space.Self);
		}
		AdjustIniEnd();
	}

	void AdjustRotation(){
		for (int i = 2; i < objs.Count; i++){
			objs[i].transform.SetParent(objs[i-1].transform,true);
		}
		for (int i = 1; i < objs.Count; i++){
			objs[i].transform.Rotate(rotationOffset);
		}
		for (int i = 0; i < objs.Count; i++){
			objs[i].transform.SetParent(transform,true);
		}
	}

	void AdjustPos(GameObject obj,int last){
		if(last <= -1){
			obj.transform.position = transform.position;
			return;
		}
		obj.transform.rotation = Quaternion.identity;
		Renderer rend1 = objs[last].GetComponent<Renderer>();
		Renderer rend2 = obj.GetComponent<Renderer>();
		if(!rend1) rend1 = objs[last].GetComponentInChildren<Renderer>();
		if(!rend2) rend2 = obj.GetComponentInChildren<Renderer>();
		
		Vector3 v = rend1.bounds.max - objs[last].transform.position;
		Vector3 v2 =  obj.transform.position - rend2.bounds.min;
		Vector3 pos = obj.transform.position;
		pos = objs[last].transform.position + Vector3.Scale((v + v2),distOffset);
		obj.transform.position = pos;
	}

	void AdjustIniEnd(){
		if(iniPrefab){
			if(!ini) ini = PrefabUtility.InstantiatePrefab(iniPrefab) as GameObject;
			ini.transform.position = objs[0].transform.position;
			ini.transform.SetParent(transform,true);
			PosIni(ini);
		}
		if(endPrefab){
			if(!end) end = PrefabUtility.InstantiatePrefab(endPrefab) as GameObject;
			end.transform.position = objs[0].transform.position;
			end.transform.SetParent(transform,true);
			AdjustPos(end,objs.Count-1);
		}
	}

	void PosIni(GameObject obj){
		Renderer rend1 = objs[0].GetComponent<Renderer>();
		Renderer rend2 = obj.GetComponent<Renderer>();
		if(!rend1) rend1 = objs[0].GetComponentInChildren<Renderer>();
		if(!rend2) rend2 = obj.GetComponentInChildren <Renderer>();
		
		Vector3 width = objs[0].transform.position -rend1.bounds.min ;
		Vector3 width2 =  rend2.bounds.max - obj.transform.position;
		Vector3 pos = obj.transform.position;
		pos = objs[0].transform.position - Vector3.Scale(width + width2,distOffset);
		obj.transform.position = pos;
	}
	#endif
}
