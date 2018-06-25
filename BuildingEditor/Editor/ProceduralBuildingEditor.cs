using UnityEngine;
using System.Collections;
using UnityEditor;
//using System;
[CustomEditor(typeof(ProceduralBuilding))]
public class ProceduralBuildingEditor : Editor {

	private static ProceduralBuilding aux;

	 SerializedProperty floors;
	 SerializedProperty grounds;
	 SerializedProperty roofs;

	void OnEnable(){
	 	grounds= serializedObject.FindProperty("groundsData");
		floors= serializedObject.FindProperty("floorsData");
		roofs= serializedObject.FindProperty("roofsData");
	}
	public override void OnInspectorGUI () {
		
		aux = (ProceduralBuilding)target;
		
		EditorGUILayout.PropertyField(floors);
		EditorGUILayout.PropertyField(grounds);
		EditorGUILayout.PropertyField(roofs);
		serializedObject.ApplyModifiedProperties();
		if(!aux.floorsData)
			return;

		if(aux.floors == null || aux.floors.Count == 0 || aux.floors[0] == null){
			aux.UpdatePositions();
		}
		
		GUILayout.BeginHorizontal();
			if ( GUILayout.Button("Add Floor") ) {
				int id=0;
				if(aux.floors != null && aux.floors.Count != 0 && aux.floors[0] != null){ 
					id = aux.floors[aux.floors.Count-1].id;
				}
				aux.Add(id);
				Changed(aux);
			}
			if ( GUILayout.Button("Remove Floor") ) {
				aux.Remove();
				Changed(aux);
			}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
			if ( GUILayout.Button("Increase Size") ) {
				aux.Increase();
				Changed(aux);
			}
			if ( GUILayout.Button("Decrease Size") ) {
				aux.Decrease();
				Changed(aux);
			}
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
			if (GUILayout.Button("Previous Floor")){
				aux.Previous();
				Changed(aux);
			}	
			if (GUILayout.Button("Next Floor")){
				aux.Next();
				Changed(aux);
			}
		GUILayout.EndHorizontal();
		
		
		if ( GUILayout.Button("Update") ) {
			aux.UpdatePositions();
			Changed(aux);
		}	
		
		GUILayout.BeginHorizontal();
			aux.fillLeft = GUILayout.Toggle(aux.fillLeft,"Left");
			aux.fillRight = GUILayout.Toggle(aux.fillRight,"Right");
			aux.fillFront = GUILayout.Toggle(aux.fillFront,"Front");
			aux.fillBack = GUILayout.Toggle(aux.fillBack,"Back");
			aux.fillRoof = GUILayout.Toggle(aux.fillRoof,"Roof");
			aux.fillBaseGround = GUILayout.Toggle(aux.fillBaseGround,"Ground");
		GUILayout.EndHorizontal();
	}
	
	void Changed(ProceduralBuilding aux){
		EditorUtility.SetDirty(aux);
		if(aux.roof)
			EditorUtility.SetDirty(aux.roof);
		if(aux.groundFloor)
			EditorUtility.SetDirty(aux.groundFloor);
		foreach (var item in aux.floors){
			EditorUtility.SetDirty(item);
		}
	}
}
