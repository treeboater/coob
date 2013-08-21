using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Room {
	// Holds a north-east co-ordinate and a south-west co-ordinate
	public int[] coords;
	public Room (int NEX, int NEZ, int SWX, int SWZ) {
		coords = new int[4];
		coords[0] = NEX;
		coords[1] = NEZ;
		coords[2] = SWX;
		coords[3] = SWZ;
	}
	public int this [int index] {
		get {
			return coords[index];	
		}
		set {
			coords[index] = value;
		}
	}
};

public class NewWallGenerator : MonoBehaviour {
	
	public GameObject parent;
	
	private int sizeX;
	private int sizeZ;
	
	private Tile[,] floor;
	
	private int nextColour;
	
	// For gooby blob room expander
	private Queue<Tile> ExpansionQueue;
	
	private static NewWallGenerator instance;

    public static NewWallGenerator Instance 
    {
         get { return instance ?? (instance = new NewWallGenerator()); }
    }
	
	private NewWallGenerator()
    {
          //private constructor makes it where this class can only be created by itself
    }
	
	// Use this for initialization
	void Start () {
		sizeX = 16;
		sizeZ = 16;
		floor = new Tile[sizeX, sizeZ];
		nextColour = 1;
		ExpansionQueue = new Queue<Tile>();
		
		InitializeTiles();
		ExpandRooms();
		ArrangeWalls();
		BuildFloorGeometry();
		LogTileColours();
		print("HEY");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	
	public int getSizeX(){
		return sizeX;
	}
	
	public int getSizeZ(){
		return sizeZ;
	}
	
	private void InitializeTiles () {
		for (int x = 0; x < floor.GetLength(0); x++) {
			for (int z = 0; z < floor.GetLength(1); z++) {
				floor[x, z] = new Tile();
				floor[x, z].SetContainer(this);
				floor[x, z].SetWorldPosition(new Vector2(x*2, z*2));
				floor[x, z].MakeSolid();
			}
		}
	}
	
	/*
	 * Getters and Setters
	 * */
		
	public Tile GetTileAtIndex (int x, int z) {
		if ((x < sizeX) && (x >= 0)) {
			if ((z < sizeZ) && (z >= 0)) {
				return floor[x, z];
			}
		}
		return null;
	}
	
	
	
	/**
	 * End of Getters and Setters
	 * */
	
	// This assumes that the transform parent has been set
	private void BuildFloorGeometry () {
		for (int x = 0; x < floor.GetLength(0); x++) {
			for (int z = 0; z < floor.GetLength(1); z++) {
				floor[x, z].BuildTileGeometry(parent);
			}
		}
	}
	
	// Makes gooby blob rooms
	private void ExpandRooms () {
		ExpansionQueue.Enqueue(floor[4, 5]);
		ExpansionQueue.Enqueue(floor[10, 3]);
		ExpansionQueue.Enqueue(floor[11, 10]);
		
		while (ExpansionQueue.Count > 0) {
			ExpandRoomOnTile(ExpansionQueue.Dequeue());
		}
	}
	
	// Core part of gooby blob room maker
	private void ExpandRoomOnTile (Tile tile) {
		if (tile.GetColour() == 0) {
			// New room
			int newColour = nextColour;
			tile.SetColour(newColour);
			nextColour++;
		}
		
		// Expanding clockwise from NorthWest
		if (tile.HasNorthWest() && (tile.GetNorthWest().GetColour() == 0)) {
			tile.GetNorthWest().SetColour(tile.GetColour());
			ExpansionQueue.Enqueue(tile.GetNorthWest());
		}
		if (tile.HasNorth() && (tile.GetNorth().GetColour() == 0)) {
			tile.GetNorth().SetColour(tile.GetColour());
			ExpansionQueue.Enqueue(tile.GetNorth());
		}
		if (tile.HasNorthEast() && (tile.GetNorthEast().GetColour() == 0)) {
			tile.GetNorthEast().SetColour(tile.GetColour());
			ExpansionQueue.Enqueue(tile.GetNorthEast());
		}
		if (tile.HasEast() && (tile.GetEast().GetColour() == 0)) {
			tile.GetEast().SetColour(tile.GetColour());
			ExpansionQueue.Enqueue(tile.GetEast());
		}
		if (tile.HasSouthEast() && (tile.GetSouthEast().GetColour() == 0)) {
			tile.GetSouthEast().SetColour(tile.GetColour());
			ExpansionQueue.Enqueue(tile.GetEast());
		}
		if (tile.HasSouth() && (tile.GetSouth().GetColour() == 0)) {
			tile.GetSouth().SetColour(tile.GetColour());
			ExpansionQueue.Enqueue(tile.GetSouth());
		}
		if (tile.HasSouthWest() && (tile.GetSouthWest().GetColour() == 0)) {
			tile.GetSouthWest().SetColour(tile.GetColour());
			ExpansionQueue.Enqueue(tile.GetSouthWest());
		}
		if (tile.HasWest() && (tile.GetWest().GetColour() == 0)) {
			tile.GetWest().SetColour(tile.GetColour());
			ExpansionQueue.Enqueue(tile.GetWest());
		}
	}
	
	private void BoxExpandRooms () {
		List<Room> rooms = new List<Room>();
		rooms.Add(new Room(4, 5, 4, 5));
		// THIS
	}
	
	private bool CanExpandRoom (Room room) {
		return false;
		// AND THIS
	}
	
	private bool IsClearBetweenTiles (int x1, int z1, int x2, int z2) {
		if (x1 > x2) {
			// AND THIS
		} else {
			// AND EVEN THIS
		}
		return false;
	}
	
	private void ArrangeWalls () {
		//This assumes tiles have been painted with colours
		for (int x = 0; x < sizeX; x++) {
			for (int z = 0; z < sizeZ; z++) {
				Tile tile = floor[x, z];
				int thisTileColour = tile.GetColour();
				if (tile.HasNorth() && (thisTileColour == tile.GetNorth().GetColour())) {
					tile.SetNorthWall(false);
				}
				if (tile.HasEast() && (thisTileColour == tile.GetEast().GetColour())) {
					tile.SetEastWall(false);
				}
			}
		}
	}
	
	public void LogTileColours () {
		string result = "";
		for (int x = 0; x < sizeX; x++) {
			for (int z = 0; z < sizeZ; z++) {
				result += floor[x, z].GetColour();
			}
			result += "\n";
		}
		Debug.Log(result);
	}
	
}
