using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class searchAlgorithms : MonoBehaviour{
	
	private static float debugLineDist = 1000.0f;
	private static float debugLineCount = 16.0f;
	
	public static void aStar(Vector3 qPos, Vector3 pPos){
		List<Vector3> tempRays = prepareRays(qPos, pPos);
		
	}
	
	//prepare rays in a circle to use for search algorithms
	static List<Vector3> prepareRays(Vector3 qPos, Vector3 pPos){
		float limit = (debugLineCount/4)+1;
		List<Vector3> tempRays = new List<Vector3>();

		for (int j = 0; j < 4; j++){
			for (int i = 0; i < limit; i++){
				float dz = (limit-1)*debugLineDist;
				float dx = 0;
				if (i != 0 && i != (limit-1)){
					dx = Mathf.Cos((Mathf.PI/2)*(i/(limit-1)))*debugLineDist;
					dz = Mathf.Sin((Mathf.PI/2)*(i/(limit-1)))*debugLineDist;
				} else if (i == (limit-1)){
					dz = 0;
					dx = (limit-1)*debugLineDist;
				}
				if (j == 1){
					dz *= -1;
				} else if (j == 2){
					dz *= -1;
					dx *= -1;
				} else if (j == 3){
					dx *= -1;
				}
				Vector3 dir = new Vector3(qPos.x+dx, qPos.y, qPos.z+dz);
				tempRays.Add(dir);
			}
		}
		return tempRays;
	}	
	
	//an incredibly stupid search algorithm
	public static Vector3 stupidSearch(Vector3 qPos, Vector3 pPos){
		List<Vector3> greedyRays = prepareRays(qPos, pPos);
		
		Vector3 successor = qPos;
		float dist = Mathf.Infinity;
		foreach (Vector3 r in greedyRays){
			//Debug.DrawLine (qPos, r, Color.white);
			RaycastHit hit;	
			if (Physics.Linecast(qPos, r, out hit)){
    			float tempdist = Vector3.Distance(hit.point, pPos);
				//float tempdist = euclidDist(hit.point.x, hit.point.z, pPos.x, pPos.z);
				if (tempdist < dist){
					Vector3 hitPos = hit.point;
					Vector3 specScale = Vector3.Scale((hitPos-qPos), new Vector3(-0.01f, 0.0f, -0.01f));
					if (Physics.Linecast(hitPos+specScale, pPos, out hit) && 
						hit.collider.gameObject.name=="LastPosition") {		
						dist = tempdist;
						successor = r;
						//Debug.DrawLine (hitPos+specScale, pPos, Color.red);
					}
				}
    		}
		}
		//Debug.DrawLine (qPos, successor, Color.green);
		return successor;
	}
	//sqrt((p1-q1)^2+(p2 q2)^2)
	public static float euclidDist(float x1, float z1, float x2, float z2){
		return 	Mathf.Sqrt(Mathf.Pow((x1 - x2), 2)+Mathf.Pow((x2 - z2), 2));
	}
}
