using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorBuilder : MonoBehaviour {
	
	public GameObject[,] tiles;
	public List<int[][]> rooms;
	
	private int[,] grid;
	private List<GameObject> triggers = new List<GameObject>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void BuildFloor (int[,] grid, GameObject parent) {
		this.grid = grid;
		DefineRooms();
		RandomlyMakeDoors();
		tiles = new GameObject[grid.GetLength(0), grid.GetLength(1)];
		for (int x = 0; x < grid.GetLength(0); x++) {
			for (int z = 0; z < grid.GetLength(1); z++) {
				GameObject tile = Instantiate(Resources.Load(GetPrefabName(grid[x,z]))) as GameObject;
				tile.transform.Translate(x*4, z*4, 0); // (x, z, y) because of Blenders rotation
				tile.transform.Rotate(0.0f, 0.0f, GetRotation(x, z));
				tile.transform.parent = parent.transform;
				tiles[x,z] = tile;
				if ((x % 4 == 0) && (z % 4 == 0)) {
					GameObject light = new GameObject("Light " + x + "," + z);
					Light l = light.AddComponent<Light>();
					l.type = LightType.Point;
					l.intensity = 0.5f;
					l.range = 30f;
					l.transform.Translate((x*4f)+2, 4f, -(z*4f)-2);
					l.renderMode = LightRenderMode.ForcePixel;
					l.transform.parent = parent.transform;
				}
				if (GetPrefabName(grid[x,z]).Equals("WallDoor")){
					GameObject trig = GameObject.CreatePrimitive(PrimitiveType.Cube);
					trig.name = "pathTrigger";
					trig.transform.position = tile.transform.position + new Vector3(0, 1.0f, 0);
					trig.collider.isTrigger = true;
					trig.renderer.material.color = new Color(0, 0, 0, 0);
					Rigidbody trigBody = trig.AddComponent<Rigidbody>();
					trigBody.useGravity = false;
					trigBody.isKinematic = true;
					triggers.Add(trig);
				}
			}
		}
	}
	
	float GetRotation (int x, int z) {
		int tileCode = grid[x,z];
		if (tileCode == 0) {
			if (x + 1 < grid.GetLength(0)) {
				if (grid[x + 1,z] != 5) {
					return -90f;
				}
			} 
			if (x - 1 >= 0) {
				if (grid[x - 1,z] != 5) {
					return 90f;
				}
			}
		} else if (tileCode == 1) {
			
			if (((x + 1 < grid.GetLength(0)) && (grid[x + 1,z] == 5)) || (x + 1 >= grid.GetLength(0))) {
				if (((z + 1 < grid.GetLength(1)) && (grid[x,z + 1] == 5)) || (z + 1 >= grid.GetLength(1))) {
					return 90f;
				}
			}
			if (((z + 1 < grid.GetLength(1)) && (grid[x,z + 1] == 5)) || (z + 1 >= grid.GetLength(1))) {
				if (((x - 1 >= 0) && (grid[x - 1,z] == 5)) || (x - 1 < 0)) {
					return 180f;
				}
			}
			if (((x - 1 >= 0) && (grid[x - 1,z] == 5)) || (x - 1 < 0)) {
				if (((z - 1 >= 0) && (grid[x,z - 1] == 5)) || (z - 1 < 0)) {
					return 270f;
				}
			}
			
		} else if ((tileCode == 4) || (tileCode == 3) || (tileCode == 7) || (tileCode == 8)) {
			if (x + 1 < grid.GetLength(0)) {
				if (grid[x + 1,z] != 5) {
					return 90f;
				}
			}
			return 0.0f;
		} else if ((tileCode == 6) || (tileCode == 9)) {
			if (x + 1 < grid.GetLength(0)) {
				if (grid[x + 1,z] == 5) {
					return 0f;
				}
			} else {
				return 0f;
			}
			if (x - 1 >= 0) {
				if (grid[x - 1,z] == 5) {
					return 180f;
				}
			} else {
				return 180f;
			}
			if (z + 1 < grid.GetLength(1)) {
				if (grid[x,z + 1] == 5) {
					return 90f;
				}
			} else {
				return 90f;
			}
			if (z - 1 >= 0) {
				if (grid[x,z - 1] == 5) {
					return 270f;
				}
			} else {
				return 270f;
			}
		}
		return 0.0f;
	}
	
	void DefineRooms() {
		rooms = new List<int[][]>();
		for (int x = 0; x < grid.GetLength(0); x++) {
			for (int z = 0; z < grid.GetLength(1); z++) {
				if ((grid[x,z] == 5) && (!IsPartOfAnyExistingRoom(x, z))) {
					rooms.Add(FloodFillFindRoom(x, z));
				}
			}
		}
	}
	
	int[][] FloodFillFindRoom(int x, int z) {
		List<int[]> result = new List<int[]>();
		Stack<int[]> tileStack = new Stack<int[]>();
		tileStack.Push(new int[] {x, z});
		while (tileStack.Count != 0) {
			int[] tile = tileStack.Pop();
			result.Add(tile);
			if (grid[tile[0],tile[1]] == 5) {
				if ((IsValidTile(tile[0] + 1, tile[1])) && (!ListContainsTile(result, tile[0] + 1, tile[1]))) {
					tileStack.Push(new int[] {tile[0] + 1, tile[1]});
				}
				if ((IsValidTile(tile[0] - 1, tile[1])) && (!ListContainsTile(result, tile[0] - 1, tile[1]))) {
					tileStack.Push(new int[] {tile[0] - 1, tile[1]});
				}
				if ((IsValidTile(tile[0], tile[1] + 1)) && (!ListContainsTile(result, tile[0], tile[1] + 1))) {
					tileStack.Push(new int[] {tile[0], tile[1] + 1});
				}
				if ((IsValidTile(tile[0], tile[1] - 1)) && (!ListContainsTile(result, tile[0], tile[1] - 1))) {
					tileStack.Push(new int[] {tile[0], tile[1] - 1});
				}
			}
			
		}
		return result.ToArray();
	}
	
	bool ListContainsTile(List<int[]> list, int x, int z) {
		foreach (int[] tile in list) {
			if ((tile[0] == x) && (tile[1] == z)) return true;
		}
		return false;
	}
						
	bool IsValidTile(int x, int z) {
		if ((x >= 0) && (x < grid.GetLength(0)) && (z >= 0) && (z < grid.GetLength(1))) return true;
		else return false;
	}
	
	bool IsPartOfAnyExistingRoom(int x, int z) {
		for (int i = 0; i < rooms.Count; i++) { // For every room
			for (int j = 0; j < rooms[i].GetLength(0); j++) { // For every tile in room i
				if ((rooms[i][j][0] == x) && (rooms[i][j][1] == z)) {
					return true;
				}
			}
		}
		return false;
	}
	
	bool IsPartOfExistingRoomWithIndex(int index, int x, int z) {
		for (int j = 0; j < rooms[index].GetLength(0); j++) { // For every tile in room index
			if ((rooms[index][j][0] == x) && (rooms[index][j][1] == z)) {
				return true;
			}
		}
		return false;
	}
	
	int GetContainingRoom(int x, int z) {
		for (int i = 0; i < rooms.Count; i++) { // For every room
			for (int j = 0; j < rooms[i].GetLength(0); j++) { // For every tile in room i
				if ((rooms[i][j][0] == x) && (rooms[i][j][1] == z)) {
					return i;
				}
			}
		}
		return -1;
	}
	
	void RandomlyMakeDoors() {
		for (int i = 0; i < rooms.Count; i++) {
			int startTile = Random.Range(0, rooms[i].GetLength(0));
			int startX = rooms[i][startTile][0];
			int startZ = rooms[i][startTile][1];
			int x = startX;
			int z = startZ;
			if (grid[x,z] == 4) {
				grid[x,z] = 3;
				return;
			}
			while (grid[x,z] == 5) {
				x++;
			}
			if (grid[x,z] == 4) {
				grid[x,z] = 3;
			}
			
			x = startX;
			z = startZ;
			while (grid[x,z] == 5) {
				z++;
			}
			if (grid[x,z] == 4) {
				grid[x,z] = 3;
			}
			
			x = startX;
			z = startZ;
			while (grid[x,z] == 5) {
				x--;
			}
			if (grid[x,z] == 4) {
				grid[x,z] = 3;
			}
			
			x = startX;
			z = startZ;
			while (grid[x,z] == 5) {
				z--;
			}
			if (grid[x,z] == 4) {
				grid[x,z] = 3;
			}
		}
	}
	
	void IntelligentlyMakeDoors() {
		// This is to use the 'colouring' technique of Geoffs Dragon Maze implementation.
		// If a cell is red, it can be reached from the starting tile.
		// 0 is untouched, 1 is reached on first pass
		int[,] red = new int[grid.GetLength(0), grid.GetLength(1)];
		List<int[]> doorCandidateWalls = new List<int[]>();
		for (int x = 0; x < grid.GetLength(0); x++) {
			
		}
	}
	
	void MakeALightForEachRoom() {
		for (int i = 0; i < rooms.Count; i++) {
			float[] point = GetRoomCenterPoint(i);
			// Unfinished, so don't call this.
		}
	}
	
	float[] GetRoomCenterPoint(int roomIndex) {
		int[][] room = rooms[roomIndex];
		float totalX = 0;
		float totalZ = 0;
		for (int i = 0; i < room.GetLength(0); i++) {
			totalX += room[i][0];
			totalZ += room[i][1];
		}
		totalX /= room.GetLength(0);
		totalZ /= room.GetLength(0);
		return new float[] {totalX, totalZ};
	}
	
	bool CanReachAllRoomsFromTile(int x, int z) {
		
		List<int> finishedRooms = new List<int>();
		
		List<int[]> done = new List<int[]>();
		Stack<int[]> tileStack = new Stack<int[]>();
		 
		tileStack.Push(new int[] {x, z});
		
		while (tileStack.Count != 0) {
			
			int[] tile = tileStack.Pop();
			done.Add(tile);
			
			int roomIndex = GetContainingRoom(tile[0], tile[1]);
			if (!finishedRooms.Contains(roomIndex)) {
				finishedRooms.Add(roomIndex);
			}
			
			if ((grid[tile[0],tile[1]] == 5) || (grid[tile[0],tile[1]] == 3)) {
				if ((IsValidTile(tile[0] + 1, tile[1])) && (!ListContainsTile(done, tile[0] + 1, tile[1]))) {
					tileStack.Push(new int[] {tile[0] + 1, tile[1]});
				}
				if ((IsValidTile(tile[0] - 1, tile[1])) && (!ListContainsTile(done, tile[0] - 1, tile[1]))) {
					tileStack.Push(new int[] {tile[0] - 1, tile[1]});
				}
				if ((IsValidTile(tile[0], tile[1] + 1)) && (!ListContainsTile(done, tile[0], tile[1] + 1))) {
					tileStack.Push(new int[] {tile[0], tile[1] + 1});
				}
				if ((IsValidTile(tile[0], tile[1] - 1)) && (!ListContainsTile(done, tile[0], tile[1] - 1))) {
					tileStack.Push(new int[] {tile[0], tile[1] - 1});
				}
			}
			
		}
		if (finishedRooms.Count == rooms.Count) {
			return true;
		} else {
			return false;
		}
	}
	
	int GetNeighbouringWalls (int x, int z) {
		int result = 0;
		if (((x + 1) < grid.GetLength(0)) && ((grid[x + 1,z]) != 5)) result++;
		if (((x - 1) >= 0) && ((grid[x - 1,z]) != 5)) result++;
		if (((z + 1) < grid.GetLength(1)) && ((grid[x,z + 1]) != 5)) result++;
		if (((z - 1) >= 0) && ((grid[x,z - 1]) != 5)) result++;
		return result;
	}
	
	string GetPrefabName(int tileCode) {
		switch (tileCode) {
			case 0:
				return "WallCap";
			case 1:
				return "WallCorner";
			case 2:
				return "WallCross";
			case 3:
				return "WallDoor";
			case 4:
				return "WallFlat";
			case 5:
				return "WallFloor";
			case 6:
				return "WallTee";
			case 7:
				return "WallWindow";
			case 8:
				return "WallInteriorWindow";
			case 9:
				return "WallExteriorTee";
			}
		return null;
	}
	
}
