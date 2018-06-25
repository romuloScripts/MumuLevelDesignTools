using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomEditor(typeof(ObjArray))]
public class ObjArrayEditor : Editor  {

	private static ObjArray aux;
	public override void OnInspectorGUI () {
		
		aux = (ObjArray)target;
		GUILayout.BeginHorizontal();
		base.OnInspectorGUI();
		if ( GUILayout.Button("Previous") ) {
			aux.Ant();
		}	
		if ( GUILayout.Button("Next") ) {
			aux.Prox();
		}
		GUILayout.EndHorizontal();
	}
}
