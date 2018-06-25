using UnityEngine;
using System.Collections;
using System;

public class BezierCurve : MonoBehaviour{
	
	public Transform p1,p2,p3,p4;

	[Space(), Header("Editor"), Range(0.001f,1)]
	public float step=0.01f;
	public bool drawGizmos=true;
	
	public void OnValidate(){
		step = Mathf.Clamp(step,0.001f,1);
	}

	public Vector3 getPos(float u){
		return getPos(p1.position,p2.position,p3.position,p4.position, u);
	}

	public static Vector3 getPos(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float u) {
		float n = 1-u;
		return p1*n*n*n + p2*3*u*n*n + p3*3*u*u*n + p4*u*u*u;
	}

	GameObject newTransform(string nome){
		GameObject g = new GameObject(nome);
		g.transform.parent = transform;
		g.transform.localPosition = Vector3.zero;
		return g;
	}
	
	void Verifiy(){
		if(p1 == null){
			p1 = newTransform("p1").transform;
			p1.position+=Vector3.left*5;
		}
		if(p2 == null){
			p2 = newTransform("p2").transform;
			p2.position+=Vector3.left*5;
			p2.position+=Vector3.up*7;
			p2.parent = p1;
		}
		if(p3 == null){
			p3 = newTransform("p3").transform;
			p3.position+=Vector3.right*5;
			p3.position+=Vector3.up*7;
		}
		if(p4 == null){
			p4 = newTransform("p4").transform;
			p4.position+=Vector3.right*5;
			p3.parent = p4;
		}
	}
	
	
	void OnDrawGizmos(){												
		Verifiy();
		if(!drawGizmos) return;
		float u=0;
		float i=0;
		Vector3 puantes;
		Vector3 Pu = getPos(p1.position,p2.position,p3.position,p4.position, u);
		Gizmos.DrawLine(p2.position,p1.position);
		Gizmos.DrawLine(p3.position,p4.position);
		while(u != 1){
			puantes = Pu;	
			u = Mathf.Lerp(u,1,i);
			Pu = getPos(p1.position,p2.position,p3.position,p4.position, u);
			if (u != 0.0f) {
				Gizmos.color = Color.green;
				Gizmos.DrawLine (Pu, puantes);
			}
			i += step;
		}
	}
	
	void OnDrawGizmosSelected(){
		Gizmos.DrawLine(p2.position,p1.position);
		Gizmos.DrawLine(p3.position,p4.position);
	}
}


