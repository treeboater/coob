using UnityEngine;
using System.Collections;

public class CrazyColours : MonoBehaviour {
	
	public Transform seeker;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Color specialColor = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f), 1);
		seeker.light.color = specialColor; // Random.Range(0, 1
		seeker.light.range = Random.Range(1.0f, 15.0f);
		seeker.light.intensity = Random.Range(1.0f, 15.0f);
		seeker.renderer.material.color = specialColor;
	}
}
