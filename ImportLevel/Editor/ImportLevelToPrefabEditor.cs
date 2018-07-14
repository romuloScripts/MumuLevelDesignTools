using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ImportLevelToPrefabs))]
public class ImportLevelToPrefabEditor : Editor {

	private ImportLevelToPrefabs p;

	private void OnEnable () {
		p = (ImportLevelToPrefabs) target;
	}
	
	public override void OnInspectorGUI () {
		DrawDefaultInspector();
		GUILayout.Space(20);
		if(p.hierarchyObjs != null && p.hierarchyObjs.Length >0){ 
			if(GUILayout.Button("Update Level"))
				p.UpdateLevel();
		}else if(GUILayout.Button("Create Level")){
			p.CreateLevel();
		}
 	}
}