using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
[CustomEditor(typeof(Floor))]
public class FloorEditor : Editor{

	private static Floor aux;
	public override void OnInspectorGUI(){
		
		aux = (Floor)target;
		if(aux.building){ 
			GUILayout.BeginHorizontal();
			if ( GUILayout.Button("Previous")){
				aux.Previous();
				//EditorUtility.SetDirty(aux);
			}
			if ( GUILayout.Button("Next")){
				aux.Next();
				//EditorUtility.SetDirty(aux);
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			aux.left = GUILayout.Toggle(aux.left,"Left");
			aux.right= GUILayout.Toggle(aux.right,"Right");
			aux.front = GUILayout.Toggle(aux.front,"Front");
			aux.back = GUILayout.Toggle(aux.back,"Back");
			GUILayout.EndHorizontal();
		}else{
			base.OnInspectorGUI();
		}	
	}
}
