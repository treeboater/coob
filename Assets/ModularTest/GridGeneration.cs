using UnityEngine;
using System.Collections;

[System.Serializable]
public class IntegerArray {
	public int[] Z;
	public int this [int index] {
		get {
			return Z[index];	
		}
		set {
			Z[index] = value;
		}
	}
};

public class GridGeneration : MonoBehaviour {
	
	public GameObject parent;
	
	public IntegerArray[] X;
	
	public int SizeX;
	public int SizeZ;

	// Use this for initialization
	void Start () {
		X = new IntegerArray[SizeX];
		for (int i = 0; i < SizeX; i++) {
			X[i] = new IntegerArray();
			X[i].Z = new int[SizeZ];
			for (int j = 0; j < SizeZ; j++) {
				X[i][j] = 5;
			}
		}
		BuildOuterWall();
		CarveRooms();
		ArrangeGrid();
		
		FloorBuilder fb = gameObject.AddComponent<FloorBuilder>();
		int[,] grid = new int[SizeX,SizeZ];
		for (int i = 0; i < SizeX; i++) {
			for (int j = 0; j < SizeZ; j++) {
				grid[i,j] = X[i][j];
				// Debug.Log(i + ", " + j); // This checks out fine
			}
		}
		fb.BuildFloor(grid, parent);
	}
	
	int[] GenerateGridPoint() {
		int[] result = new int[2];
		result[0] = Random.Range (0, SizeX);
		result[1] = Random.Range (0, SizeZ);
		return result;
	}
	
	void BuildOuterWall() {
		for (int x = 0; x < SizeX; x++) {
			for (int z = 0; z < SizeZ; z++) {
				if ((x == 0) || (x == SizeX - 1)) {
					X[x][z] = 2;
				}
				if ((z == 0) || (z == SizeZ - 1)) {
					X[x][z] = 2;
				}
			}
		}
	}
	
	void CarveRooms() {
		int minDimention = SizeX<SizeZ?SizeX:SizeZ;
		int rooms = Random.Range(1>(minDimention/3)?1:(minDimention/3),minDimention/2);
		for (int i = 0; i < rooms; i++) {
			int[] point = GenerateGridPoint();
			int roomsize = Random.Range(1,minDimention/3);
			int minX = point[0] - roomsize;
			int maxX = point[0] + roomsize;
			int minZ = point[1] - roomsize;
			int maxZ = point[1] + roomsize;
			for (int x = minX; x < maxX; x++) {
				for (int z = minZ; z < maxZ; z++) {
					// Check if the tile is inside the grid
					if ((x >= 0) && (x < SizeX)) {
						if ((z >= 0) && (z < SizeZ)) {
							// Put walls on the edges
							if ((x == 0) || (x == SizeX-1) || (x == minX) || (x == maxX-1) || (z == 0) || (z == SizeZ-1) || (z == minZ) || (z == maxZ-1)) {
								/*if ((x == point[0]) && (GetNeighbouringWalls(x, z) == 2) && (roomsize > 1)) {
									X[x][z] = 3; // Make a door
								} else if ((z == point[1]) && (GetNeighbouringWalls(x, z) == 2) && (roomsize > 1)) {
									X[x][z] = 3; // Make a door
								} else {*/
									X[x][z] = 2; // Make a wall
								/*}*/
							}
						}
					}
				}
			}
		}
	}
	
	void ArrangeGrid() {
		IntegerArray[] temp = new IntegerArray[SizeX];
		for (int i = 0; i < SizeX; i++) {
			temp[i] = new IntegerArray();
			temp[i].Z = new int[SizeZ];
			for (int j = 0; j < SizeZ; j++) {
				temp[i][j] = X[i][j];
			}
		}
		for (int x = 0; x < SizeX; x++) {
			for (int z = 0; z < SizeZ; z++) {
				if (X[x][z] == 2) {
					switch (GetNeighbouringWalls(x, z)) {
					case 0:
						temp[x][z] = 5;
						break;
					case 1:
						temp[x][z] = 0;
						break;
					case 2:
						temp[x][z] = DetermineFlatOrCorner(x, z);
						if ((temp[x][z] == 4) && (IsOnEdge(x, z))) {
							temp[x][z] = 8;
						}
						break;
					case 3:
						temp[x][z] = 6;
						break;
					case 4:
						temp[x][z] = 2;
						break;
					}
				}
			}
		}
		X = temp;
	}
	
	int GetNeighbouringWalls (int x, int z) {
		int result = 0;
		if (((x + 1) < SizeX) && ((X[x + 1][z]) != 5)) result++;
		if (((x - 1) >= 0) && ((X[x - 1][z]) != 5)) result++;
		if (((z + 1) < SizeZ) && ((X[x][z + 1]) != 5)) result++;
		if (((z - 1) >= 0) && ((X[x][z - 1]) != 5)) result++;
		return result;
	}
	
	bool IsOnEdge(int x, int z) {
		if (x + 1 >= SizeX) return true;
		if (x - 1 < 0) return true;
		if (z + 1 >= SizeZ) return true;
		if (z - 1 < 0) return true;
		return false;
	}
	
	int DetermineFlatOrCorner (int x, int z) {
		if (((x + 1) < SizeX) && ((X[x + 1][z]) != 5)) {
			if (((x - 1) >= 0) && ((X[x - 1][z]) != 5)) {
				return 4;
			}
		}
		if (((z + 1) < SizeZ) && ((X[x][z + 1]) != 5)) {
			if (((z - 1) >= 0) && ((X[x][z - 1]) != 5)) {
				return 4;
			}
		}
		return 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
