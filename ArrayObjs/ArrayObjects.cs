using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ArrayObjects: MonoBehaviour {

	public Renderer obj;
	public Vector3 distance;
	[HideInInspector]
	public int xn, zn, yn;

	#if UNITY_EDITOR
	[Button("Create Prefabs")]
	public void CreateCells() {
		Transform parent = transform.parent;
		transform.parent = null;
		Quaternion rot = transform.rotation;
		transform.rotation = Quaternion.identity;
		DestroyAll();
		AddX(transform.localScale.x,obj);
		transform.rotation = rot;
		transform.parent = parent;
	}

	void AddX(float width, Renderer obj){

		float xSize = obj.bounds.size.x;
		xn = (int)(width/xSize);
		Vector3 pos = transform.position;
		//float y = pos.y;
		float z = pos.z;
		for (int i = 0; i < xn; i++) {
			Renderer newobj = PrefabUtility.InstantiatePrefab (obj) as Renderer;
			newobj.transform.SetParent(transform,true);
			pos.z = z + newobj.bounds.max.z - newobj.transform.position.z;
			pos.x += newobj.bounds.max.x - newobj.transform.position.x;
			pos.x += distance.x;
			newobj.transform.position = pos;
			pos.x =newobj.bounds.max.x;
			AddZ(transform.localScale.z,obj,newobj);
			AddY(transform.localScale.y,obj,newobj);
		}
	}

	void AddZ(float width,Renderer obj,Renderer rendIni){	

		float size = obj.bounds.size.z;
		zn = (int)(width/size)-1;
		Vector3 pos = rendIni.transform.position;
		pos.z = rendIni.bounds.max.z;

		for (int i = 0; i < zn; i++) {
			Renderer newobj = PrefabUtility.InstantiatePrefab (obj) as Renderer;
			newobj.transform.SetParent(transform,true);
			pos.z += newobj.bounds.max.z - newobj.transform.position.z;
			pos.z+= distance.z;
			newobj.transform.position = pos;
			pos.z =newobj.bounds.max.z;
			AddY(transform.localScale.y,obj,newobj); 
		}
	}

	void AddY(float width,Renderer obj,Renderer rendIni){

		float size = obj.bounds.size.y;
		yn = (int)(width/size)-1;
		Vector3 pos = rendIni.transform.position;
		pos.y =rendIni.bounds.max.y;

		for (int i = 0; i < yn; i++) {
			Renderer newobj = PrefabUtility.InstantiatePrefab (obj) as Renderer;
			newobj.transform.SetParent(transform,true);
			pos.y += newobj.bounds.min.y - newobj.transform.position.y;
			pos.y+= distance.y;
			newobj.transform.position = pos;
			pos.y =newobj.bounds.max.y;
		}
	}

	[Button("Destroy All")]
	void DestroyAll(){
		Renderer[] cells = GetComponentsInChildren<Renderer>();
		foreach (var item in cells) {
			if(item && item.gameObject != this.gameObject)
				DestroyImmediate (item.gameObject);
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.blue;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube(new Vector3(0.5f,0.5f,0.5f),Vector3.one);

	}
	#endif
}
