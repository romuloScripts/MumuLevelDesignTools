using UnityEngine;
using System.Collections;

[SelectionBase]
public class Floor : MonoBehaviour {

	public int id;
	public ProceduralBuilding building;
	public Renderer meshRenderer;
	public bool left, right, front, back;
	public GameObject frontObj;
	public GameObject leftObj;

	public GameObject rightObj;
	public GameObject backObj;

	public void ConstructFloor(bool left, bool right, bool front, bool back){
		
		left = left || this.left;
		right = right || this.right;
		front = front || this.front;
		back = back || this.back;
		
		if(leftObj){
			if(rightObj)
				DestroyImmediate(rightObj);
			if(left && right){
				leftObj.transform.localScale = new Vector3(1,1,1);
				rightObj = Instantiate(leftObj,leftObj.transform.position,leftObj.transform.rotation) as GameObject;
				rightObj.transform.parent = leftObj.transform.parent;
				rightObj.transform.localScale = new Vector3(-1,1,1);
			}else if(right){
				leftObj.transform.localScale = new Vector3(-1,1,1);
			}else if(left){
				leftObj.transform.localScale = new Vector3(1,1,1);
			}
		}
		
		if(frontObj){
			if(backObj)
				DestroyImmediate(backObj);
			if(front && back){
				frontObj.transform.localScale = new Vector3(1,1,1);
				backObj = Instantiate(frontObj,frontObj.transform.position,frontObj.transform.rotation) as GameObject;
				backObj.transform.parent = frontObj.transform.parent;
				backObj.transform.localScale = new Vector3(1,1,-1);
			}else if(back){
				frontObj.transform.localScale = new Vector3(1,1,-1);
			}else if(front){
				frontObj.transform.localScale = new Vector3(1,1,1);
			}
		}
	}	
	
	public void Next(){
		GetComponentInParent<ProceduralBuilding>().Replace(id+1,this);
	}

	public void Previous(){
		GetComponentInParent<ProceduralBuilding>().Replace(id-1,this);
	}
}
