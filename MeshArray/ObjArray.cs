using UnityEngine;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class ObjArray : MonoBehaviour {
	public MeshArray array;
	public GameObject prefabProx;
	public GameObject prefabAnt;
	
	public void Prox(){
		ChangeModel(prefabProx);
	}
	
	public void Ant(){
		ChangeModel(prefabAnt);
	}
	
	private void ChangeModel(GameObject pref){
		#if UNITY_EDITOR
		GameObject obj = PrefabUtility.InstantiatePrefab(pref) as GameObject;
		obj.transform.position = transform.position;
		Selection.activeGameObject = obj;
		if(array){
			ObjArray o = obj.GetComponent<ObjArray>();
			if(o) o.array = array;
			obj.transform.SetParent(array.transform,true);
			int num = array.objs.FindIndex(Find);
			array.objs[num] = obj;
			EditorUtility.SetDirty(array);
			array.UpdateArray();
		}
		UnityEditor.EditorApplication.delayCall +=()=>{
			DestroyImmediate(gameObject);
		};
		#endif
	}
	
	private bool Find(GameObject o){
		if (o == gameObject){
			return true;
		}
		else{
			return false;
		}	
	}
}
