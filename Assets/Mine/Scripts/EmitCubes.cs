using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmitCubes : MonoBehaviour {
	
	public Transform queen;
	public Transform player;
	
	public int force;
	public int life;
	private int cubeCount;
	public int cubeSpawnCount;
	List<GameObject> cubes = new List<GameObject>();
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		clearCubes();
		
		if (Input.inputString == "e"){
			cubes.RemoveAll(item => item);
			clearCubes();
			cubeCount = 0;
		}
		bool emit = false;
		if (Input.inputString == "q"){
			emit = !emit;
			cubeCount = cubes.Count + cubeSpawnCount;
		}
		if (emit){
			for(int i = cubes.Count; i < cubeCount;i++){
				cubes.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
				cubes[i].AddComponent("Rigidbody");
				cubes[i].rigidbody.useGravity = false;
				cubes[i].rigidbody.mass = 0.5f;
				cubes[i].renderer.material.color = new Color(0f, 0.5f, 0.65f, 1f);
				float tempSize = Random.Range(0.5f, .95f);
				cubes[i].transform.localScale = new Vector3(
					tempSize,
					tempSize,
					tempSize
					);
				
				cubes[i].transform.position = new Vector3(
					queen.transform.position.x+UnityEngine.Random.Range(-2, 2),
					queen.transform.position.y+UnityEngine.Random.Range(0, 2), 
					queen.transform.position.z+UnityEngine.Random.Range(-2, 2)
					);
				Destroy(cubes[i], life+UnityEngine.Random.Range(0, 10));
				cubes[i].collider.name = "attackers";
				cubes[i].layer = 2;
			}
		}
		
		clearCubes();
		
		foreach (GameObject c in cubes){
			
			//cubes[i].transform.LookAt(player);
				
			/*improved force method*/
			//c.rigidbody.AddForce(rule2(c.transform.position));
			
			c.rigidbody.AddForce(Vector3.Scale(subtractVector(player.position, c.transform.position) * force, new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime)));			
			
			/*Old tranlastion method*/
			//cubes[i].transform.position += subtractVector(player.position, cubes[i].transform.position);
		
			//cubes[i].transform.position += rule2(cubes[i].transform.position, i);
			//cubes[i].transform.position += altRule2(cubes[i].transform.position, i);
		
			//cubes[i].transform.position += new Vector3(0, 0.1f, 0);
		}
	}
	
	void clearCubes(){
		for (int i = 0; i < cubes.Count; i++){
			if (cubes[i].gameObject == null){
				cubes.RemoveAt(i);
			}
		}
	}
	
	Vector3 rule2(Vector3 boid){
		Vector3 c = new Vector3(0, 0, 0);
		for(int i = 0; i < cubeCount; i++){
				if (Vector3.Distance(boid, cubes[i].transform.position) < 5){
					float d = Vector3.Distance(boid, cubes[i].transform.position);
					c.x += d;
					c.y += d;
					c.z += d;
				}
		}
		return c;
	}
	
	Vector3 altRule2(Vector3 boid, int j){
		Vector3 c = new Vector3(0, 0, 0);
		for(int i = 0; i < cubeCount; i++){
			if (i != j){
				float d = Vector3.Distance(boid, cubes[i].transform.position);
				c.x = 1f/(1+(d*d))+0.01f;
				c.y = 1f/(1+(d*d))+0.01f;
				c.z = 1f/(1+(d*d))+0.01f;
			}
		}
		return c;
	}
	
	Vector3 subtractVector(Vector3 a, Vector3 b){
		Vector3 c = new Vector3(0, 0, 0);
		c.x = a.x-b.x;
		c.y = a.y-b.y;
		c.z = a.z-b.z;
		return c;
	}
}
