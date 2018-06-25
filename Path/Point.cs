using UnityEngine;
using System.Collections;

public class Point : MonoBehaviour {

	public Point dir;
	public Point esq;
	
	void OnDrawGizmos(){
		Gizmos.color = Color.yellow;
		if(esq)
			Gizmos.DrawLine(transform.position, esq.transform.position);
		if(dir)
			Gizmos.DrawLine(transform.position, dir.transform.position);
	}
	
	[ContextMenu("AddPoint")]
	void AddPoint(){
		Point p = Instantiate(this,transform.position,transform.rotation) as Point;
		p.transform.position += Vector3.right*10;
		p.transform.parent = transform.parent;
		p.esq = this;
		p.dir = null;
		dir = p;
	}

	public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value){
         Vector3 ab = b - a;
         Vector3 av = value - a;
         return Vector3.Dot(av, ab)/ Vector3.Dot(ab, ab);
	}
	
}
