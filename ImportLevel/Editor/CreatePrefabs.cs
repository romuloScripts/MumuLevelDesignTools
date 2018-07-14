using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreatePrefabs : ScriptableWizard{
	
	public Object folder;
	public bool ApplyPrefab = true;
	public bool DestroyInstance=true;
	public bool CreateInEmptyObj=false;
	
	[MenuItem("GameObject/CreatePrefabs")]
	public static void CriarPrefab(){
		ScriptableWizard.DisplayWizard<CreatePrefabs>(
			"CreatePrefabs", "Create");
	}
	
	void OnWizardCreate () {	
		if(!folder) return;
		
		string path = AssetDatabase.GetAssetPath (folder);
		GameObject[] objs = Selection.gameObjects;
		foreach (var obj in objs){
			GameObject instance = Instantiate(obj,obj.transform.position,obj.transform.rotation) as GameObject;
			instance.name = obj.name;
			if(CreateInEmptyObj){
				GameObject newObj = new GameObject(obj.name);
				newObj.transform.position = obj.transform.position;
				instance.transform.parent = newObj.transform;
				PrefabUtility.CreatePrefab(path+"/"+newObj.name+".prefab",newObj,
					ApplyPrefab?ReplacePrefabOptions.ConnectToPrefab:ReplacePrefabOptions.Default);
			}else{
				PrefabUtility.CreatePrefab(path+"/"+instance.name+".prefab",instance,
				   	ApplyPrefab?ReplacePrefabOptions.ConnectToPrefab:ReplacePrefabOptions.Default);
			}
			
			if(DestroyInstance)
				DestroyImmediate(instance);
		}
	}
}
