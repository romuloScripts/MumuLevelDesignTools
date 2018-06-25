using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[SelectionBase]
public class ProceduralBuilding : MonoBehaviour {

	public BuildingsData floorsData;
	public BuildingsData groundsData;
	public BuildingsData roofsData;
	
	public int buildingSize;
	public Floor groundFloor;
	public Floor roof;
	public List<Floor> floors;
	
	public bool fillLeft = true;
	public bool fillFront = true;
	public bool fillRight;
	public bool fillBack;
	public bool fillRoof=true;
	public bool fillBaseGround=true;

	const int maxSize=3;
			
	public void Add(int id) {		
		Floor obj = CreateFloor(id,floorsData.floor);
		floors.Add(obj);
		UpdatePositions();
	}
	
	public void Replace(int id, Floor floor,bool selectObj = true) {
		Floor obj=null;
		int i = floors.IndexOf(floor);
		if(i>0){
			if(id < 0 || id >= floorsData.floor.Length) return;
			obj = CreateFloor(id,floorsData.floor);
			floors[i] = obj;
		}else if(floor == groundFloor){
			if(id < 0 || id >= groundsData.floor.Length) return;
			obj = CreateFloor(id,groundsData.floor);
			groundFloor = obj;
		}else if(floor == roof) {
			if(id < 0 || id >= roofsData.floor.Length) return;
			obj = CreateFloor(id,roofsData.floor);
			roof = obj;
		}
		DestroyImmediate(floor.gameObject);
		UpdatePositions();
#if UNITY_EDITOR
		if(selectObj && obj != null)
			Selection.activeGameObject = obj.gameObject;
#endif
	}
	
	public void FitPos(Floor obj, int i) {
		obj.transform.position = floors[i].transform.position;
		float width = obj.transform.position.y - obj.meshRenderer.bounds.min.y;
		Vector3 pos = obj.transform.position;
		pos.y = floors[i].meshRenderer.bounds.max.y + width;
		obj.transform.position = pos;	
	}
	
	public void Remove() {
		if(floors.Count <= 1) return;
		DestroyImmediate(floors[floors.Count-1].gameObject);
		floors.RemoveAt(floors.Count-1);
		UpdatePositions();
	}
	
	public void AdjustIniEnd(int idGround, int idRoof) {
		
		if(fillBaseGround && groundsData){
			if(!groundFloor){
				groundFloor = CreateFloor(idGround,groundsData.floor);
			}
			groundFloor.transform.parent = transform;
			groundFloor.ConstructFloor(fillLeft,fillRight,fillFront,fillBack);
			PosIni();
		}else if(groundFloor){
			DestroyImmediate(groundFloor.gameObject);
		}
		
		if(fillRoof && roofsData){
			if(!roof){
				roof = CreateFloor(idRoof,roofsData.floor);
			}
			roof.transform.parent = transform;
			roof.ConstructFloor(fillLeft,fillRight,fillFront,fillBack);
			FitPos(roof,floors.Count-1);	
		}else if(roof){
			DestroyImmediate(roof.gameObject);
		}	
	}
	
	public void PosIni() {

		groundFloor.transform.position = floors[0].transform.position;
		float width = groundFloor.meshRenderer.bounds.max.y - groundFloor.transform.position.y;
		Vector3 pos = groundFloor.transform.position;
		pos.y = floors[0].meshRenderer.bounds.min.y - width;
		groundFloor.transform.position = pos;
	}
	
	public void UpdatePositions() {
		if(floors == null || floors.Count == 0 || floors[0] == null){
			if(!floorsData) return;
			Floor obj = CreateFloor(0,floorsData.floor);
			floors = new List<Floor>();
			floors.Add(obj);
		}
		
		for (int i = 0; i < floors.Count; i++){
			if(i == 0){
				floors[0].transform.position = transform.position;
				/* floors[0].transform.eulerAngles = new Vector3(floors[0].transform.eulerAngles.x,
										transform.eulerAngles.y,floors[0].transform.eulerAngles.z); */
			}else{
				FitPos(floors[i],i-1);
			}
			floors[i].ConstructFloor(fillLeft,fillRight,fillFront,fillBack);
			floors[i].transform.parent = transform;
		}
		AdjustIniEnd(0,0);
	}
	
	public void Increase(){
		buildingSize++;
		Mathf.Clamp(buildingSize,0,maxSize);
		UpdateSize();	
	}
	
	public void Decrease(){
		buildingSize--;
		Mathf.Clamp(buildingSize,0,maxSize);
		UpdateSize();	
	}
	
	public void UpdateSize() {
		List<int> idsmeios = new List<int>();
		
		foreach (var item in floors) {
			idsmeios.Add(item.id);
			DestroyImmediate(item.gameObject);
		}
		floors.Clear();
		for (int i = 0; i < idsmeios.Count; i++){
			Add(idsmeios[i]);
		}
		int idtopo=0;
		int idterreo=0;
		if(roof){
			idtopo = roof.id;
			DestroyImmediate(roof.gameObject);
		}
		if(groundFloor){
			idterreo = groundFloor.id;
			DestroyImmediate(groundFloor.gameObject);
		}
		AdjustIniEnd(idterreo,idtopo);
	}
	
	public Floor CreateFloor(int id,BuildingSize[] andares){
#if UNITY_EDITOR
		Floor obj = PrefabUtility.InstantiatePrefab(andares[id].GetSize(buildingSize)) as Floor;//Instantiate(andares[id].GetTamanho(tamanhoPredio),transform.position,transform.rotation) as Andar;
		obj.transform.position = transform.position;
		obj.transform.eulerAngles = new Vector3(obj.transform.eulerAngles.x,transform.eulerAngles.y,obj.transform.eulerAngles.z);
		obj.id = id;
		obj.transform.localScale = transform.lossyScale;
		//obj.floorType = tipo;
		obj.building = this;
		return obj;
#else
		return null;
#endif
	}
	
	public void Next(){
		for (int i = 0; i < floors.Count; i++){
			Replace(floors[i].id+1,floors[i],false);
		}
	}
	
	public void Previous(){
		for (int i = 0; i < floors.Count; i++){
			Replace(floors[i].id-1,floors[i],false);
		}
	}
}
