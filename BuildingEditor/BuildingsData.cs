using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public struct BuildingSize{
	public Floor[] floorSizes;
	
	public Floor GetSize(int size){
		size = Mathf.Clamp(size,0,floorSizes.Length-1);
		return floorSizes[size];
	}
}

[CreateAssetMenu(fileName = "BuildingsData", menuName = "BuildingsData")]
public class BuildingsData : ScriptableObject {

	public BuildingSize[] floor;
}
