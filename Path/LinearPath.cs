using UnityEngine;
using System.Collections;

public class LinearPath : MonoBehaviour {

	public LinearPath dir;
	public LinearPath esq;
	
	public virtual void OnValidate(){
		ConectPoints();
	}
	
	public virtual void ConectPoints(){
		if(dir){
			dir.esq = this;
			dir.ConectPoints();	
		}
		if(esq){
			esq.dir = this;
			esq.ConectPoints();
		}
	}
	
	public virtual Vector3 calcPos(float i){
		return transform.position;
	}
}
