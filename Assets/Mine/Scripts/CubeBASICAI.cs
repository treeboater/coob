using UnityEngine;
using System;
using System.Collections.Generic;

public class CubeBASICAI : MonoBehaviour {
	public Transform player;
	public Transform queen;
	public float moveSpeed = 5.0f;
	public float minDist = 5.0f;
	public float debugLineDist = 1.0f;
	public float debugLineCount = 32.0f;
	private Vector3 lastPos;
	GameObject marker;
	public NewWallGenerator nwg;
	public Vector2 startSearch;
	public Vector2 endSearch;
	private List<Vector3> mapPath = new List<Vector3>();
	// Use this for initialization
	void Start () {
		lastPos = queen.position;
		createMarker();
		
		//JumpPointSearch jps = new JumpPointSearch(nwg.GetTileAtIndex(2, 2), nwg.GetTileAtIndex(5, 3));
		
	}
	
	void createMarker(){
        marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
		marker.name = "LastPosition";
		marker.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		marker.renderer.material.color = new Color(1, 1, 1, 0);
		marker.transform.position = new Vector3(0, -100, 0);
		marker.collider.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.inputString == "f"){
			Tile tempStart = nwg.GetTileAtIndex((int)startSearch.x, (int)startSearch.y);
			Tile tempEnd = nwg.GetTileAtIndex((int)endSearch.x, (int)endSearch.y);
			
			JumpPointSearch.aStarSearch(tempStart, tempEnd);
		}
		//movement();
		debugRays();
		//randomRotation();
		drawPath();
	}
	
	void hullRays(Vector3 dir){
		Debug.DrawLine(queen.position, dir, Color.blue);
	}
	
	void OnTriggerEnter(Collider trigCollision) {
		if (trigCollision.collider.tag.Equals("First Person Controller")){
			mapPath.Add(trigCollision.collider.transform.position);
		}
		print(trigCollision.collider.name);
	}
	
	void drawPath(){
		for (int i = 0; i < mapPath.Count;i++){
			if (i == 0){
				Debug.DrawLine(queen.position, mapPath[i], Color.red);
			} else if (i == mapPath.Count-1){
				Debug.DrawLine(mapPath[i], player.position, Color.red);
			} else {
				Debug.DrawLine(mapPath[i-1], mapPath[i], Color.red);	
			}
		}
	}
	
	void movement(){
		Vector3 rayDirection = player.position;
     	RaycastHit hit;
		
		//Debug.DrawLine (queen.position, rayDirection, Color.magenta);
   	 	if (Physics.Linecast(queen.position, rayDirection, out hit) && hit.collider.gameObject.name=="First Person Controller") {		
			lastPos = player.position;
			if(Vector3.Distance(queen.position, player.position) >= minDist){
    			queen.position = Vector3.MoveTowards(queen.position, player.position, moveSpeed);
			}
    	} else {
			marker.transform.position = lastPos;
			Vector3 m = searchAlgorithms.stupidSearch(queen.position, lastPos);
			//hullRays(m);
			m += new Vector3(0, -m.y, 0);
			//if (Vector3.Distance(queen.position, player.position) >= minDist)
				//queen.position = Vector3.MoveTowards(queen.position, m, moveSpeed);
			//move(m);
		}
		if (queen.position.y < 2){
			queen.position += new Vector3(0, 0.05f, 0);				
		} else if (queen.position.y < 2){
			queen.position += new Vector3(0, -0.05f, 0);	
		}
	}
	
	void debugRays(){
		float qx = queen.position.x;
		float qy = queen.position.y;
		float qz = queen.position.z;
			Debug.DrawLine (queen.position, new Vector3(qx, qy, qz+1), Color.blue);
			Debug.DrawLine (queen.position, new Vector3(qx, qy, qz-1), Color.cyan);
		
			Debug.DrawLine (queen.position, new Vector3(qx, qy+1, qz), Color.green);
			Debug.DrawLine (queen.position, new Vector3(qx, qy-1, qz), Color.yellow);
		
			Debug.DrawLine (queen.position, new Vector3(qx+1, qy, qz), Color.red);
			Debug.DrawLine (queen.position, new Vector3(qx-1, qy, qz), Color.magenta);
	}
	
	void randomRotation(){
		float rndX = UnityEngine.Random.Range(0, 3);
		float rndY = UnityEngine.Random.Range(0, 3);
		float rndZ = UnityEngine.Random.Range(0, 3);
		queen.transform.Rotate(new Vector3(rndX, rndY, rndZ));	
	}
	
	void move(Vector3 positionTo){
		//queen.transform.LookAt(player);
		if(Vector3.Distance(queen.position, positionTo) >= minDist){
    		queen.position += (positionTo-queen.position)*(moveSpeed/10000);
		}
	}
}
