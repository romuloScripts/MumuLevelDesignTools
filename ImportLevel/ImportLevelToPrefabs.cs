using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;


public class ImportLevelToPrefabs : MonoBehaviour {

	public GameObject levelFbx;
	public MeshFilter[] prefabs;
	[HideInInspector,SerializeField]
	public Transform[] hierarchyObjs;

	public void CreateLevel(){
		if(!levelFbx) return;
		Transform[] objs = Instantiate<GameObject>(levelFbx,transform.position,transform.rotation).GetComponentsInChildren<Transform>();
		foreach (var item in hierarchyObjs) {
			if(item != transform)
				DestroyImmediate(item.gameObject);
		}
		hierarchyObjs = new Transform[objs.Length];
		string rootName = gameObject.name;
		AddPrefabs(objs,hierarchyObjs,true);
		SetPrefabs(objs,hierarchyObjs);
		DestroyImmediate(objs[0].gameObject);
		gameObject.name = rootName;
		EditorSceneManager.MarkAllScenesDirty();
	}

	public void UpdateLevel(){
		if(!levelFbx) return;
		Transform[] objs = Instantiate<GameObject>(levelFbx,transform.position,transform.rotation).GetComponentsInChildren<Transform>();
		Transform[] newhierarchy = new Transform[objs.Length];
		string rootName = gameObject.name;
		hierarchyObjs[0].name = objs[0].name;
		ReoderHierarchy(objs,newhierarchy,hierarchyObjs);
		Remove(hierarchyObjs);
		AddPrefabs(objs,newhierarchy,false);
		SetPrefabs(objs,newhierarchy);
		hierarchyObjs = newhierarchy;
		DestroyImmediate(objs[0].gameObject);
		gameObject.name = rootName;
		EditorSceneManager.MarkAllScenesDirty();
		
	}

	private void ReoderHierarchy(Transform[] objs,Transform[] newhierarchy,Transform[] oldObjs){
		for (int i = 0; i < objs.Length; i++) {
			for (int j = 0; j < oldObjs.Length; j++){
				if(oldObjs[j] != null && objs[i].name.Equals(oldObjs[j].name)){
					newhierarchy[i] = oldObjs[j];
					oldObjs[j] = null;
				}
			}
		}
	}

	private void Remove(Transform[] objs){
		foreach (var item in objs) {
			if(!item) continue;
			ImportLevelModifiers modifier = item.GetComponent<ImportLevelModifiers>();
			if(modifier && modifier.notRemove){
				DestroyImmediate(modifier);
				continue;
			}
			item.DetachChildren();
			DestroyImmediate(item.gameObject);
		}	
	}

	private void AddPrefabs(Transform[] objs,Transform[] newobjs,bool overide){
		newobjs[0] = transform;
		gameObject.name = objs[0].name;
		for (int i = 1; i < objs.Length; i++) {
			if(!overide && newobjs[i]!= null) continue;
			MeshFilter mesh = objs[i].GetComponent<MeshFilter>();
			if(!mesh){ 
				newobjs[i] = new GameObject(objs[i].name).transform;
			}else{
				GameObject prefab = getPrefab(mesh);
				if(prefab){
					GameObject o = PrefabUtility.InstantiatePrefab(prefab) as GameObject; 
					o.name = objs[i].name;
					newobjs[i] = o.transform;
				}
			}
		}
	}

	private void SetPrefabs(Transform[] objs,Transform[] newobjs){
		for (int i = 0; i < objs.Length; i++){
			if(!newobjs[i]) continue;
			ImportLevelModifiers modifier = newobjs[i].GetComponent<ImportLevelModifiers>();
			if(objs[i].parent !=null && !(modifier && !modifier.updateHierarchy)){
					newobjs[i].SetParent(newobjs[GetParentIndex(objs,objs[i].parent)],true);
			}
		}
		for (int i = 0; i < objs.Length; i++){
			if(!newobjs[i]) continue;
			ImportLevelModifiers modifier = newobjs[i].GetComponent<ImportLevelModifiers>();
			if(!(modifier && !modifier.updateTransform)){
				newobjs[i].position = objs[i].transform.position;
				newobjs[i].rotation = objs[i].transform.rotation;
				newobjs[i].localScale = objs[i].localScale;
			}
		}
	}

	private int GetParentIndex(Transform[] objs, Transform parent){
		for (int i = 0; i < objs.Length; i++) {
			if(objs[i] == parent){
				return i;
			}
		}
		return 0;
	}

	private GameObject getPrefab(MeshFilter mesh){
		foreach (var item in prefabs) {
			if(item !=null && item.sharedMesh.name == mesh.sharedMesh.name){
				return item.gameObject;
			}
		}
		return null;
	}
}
#endif