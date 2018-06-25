using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomEditor(typeof(MeshArray))]
public class MeshArrayEditor : Editor {

	private static MeshArray aux;
	public override void OnInspectorGUI () {
		
		aux = (MeshArray)target;
		if ( GUILayout.Button("Update") ) {
			Quaternion rot = aux.transform.rotation;
			aux.transform.rotation = Quaternion.identity;
			aux.UpdateArray();
			aux.transform.rotation = rot;
			EditorUtility.SetDirty(aux);
		}
		base.OnInspectorGUI();
	}
}
