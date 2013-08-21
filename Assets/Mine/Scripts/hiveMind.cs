using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class hiveMind : MonoBehaviour {
	
	public Transform queen;
	
	public Transform[] seekers;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		linkSeekers(true);
	}
	
	void linkSeekers(bool debug){
     	RaycastHit hit;
		for (int i = 0; i < seekers.Length; i++){
			for(int j = i; j < seekers.Length; j++){
				if (Physics.Linecast(seekers[i].position, seekers[j].position, out hit) && hit.collider.gameObject.name.Substring(0, 6) == "Seeker") {
					Debug.DrawLine(seekers[i].position, seekers[j].position, Color.white);
				} else {
					Debug.DrawLine(seekers[i].position, seekers[j].position, Color.black);
				}
			}
		}
	}
}
