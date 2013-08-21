using UnityEngine;
using System.Collections;

public class Tile {
	
	private bool northWall;
	private bool eastWall;
	private bool floor;
	
	// position.x = world x, position.y = world z
	// these world positions and grid positions are kept in sync by their setters.
	// providing Mathf.Floor(float f) floors properly, there should be no trouble
	private Vector2 worldPosition;
	private int gridPositionX;
	private int gridPositionZ;
	
	// This is used to paint the rooms as they are formed
	private int colour;
	
	// These store references to the prefabs in charge of the game geometry
	private GameObject floorObject;
	private GameObject northWallObject;
	private GameObject eastWallObject;
	private GameObject columnObject;
	
	// This is so the tiles can ask for their neighbours
	public NewWallGenerator container;

	// Use this for initialization
	void Start () {
		northWall = false;
		eastWall = false;
		floor = true;
		
		colour = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void BuildTileGeometry (GameObject parent) {
		// These use -worldPosition.y because of Blender's axis directions
		if (floor) {
			floorObject = GameObject.Instantiate(Resources.Load("NewWallFloor")) as GameObject;
			floorObject.transform.Translate(worldPosition.x, -worldPosition.y, 0);
			floorObject.transform.parent = parent.transform;
			floorObject.name = "floor: "+this.gridPositionX+", "+this.gridPositionZ;
		}
		
		if (northWall) {
			northWallObject = GameObject.Instantiate(Resources.Load("NewWallNorth")) as GameObject;
			northWallObject.transform.Translate(worldPosition.x, -worldPosition.y, 0);
			northWallObject.transform.parent = parent.transform;
		}
		
		if (eastWall) {
			eastWallObject = GameObject.Instantiate(Resources.Load("NewWallEast")) as GameObject;
			eastWallObject.transform.Translate(worldPosition.x, -worldPosition.y, 0);
			eastWallObject.transform.parent = parent.transform;
		}
		
		if (ShouldHaveColumn()) {
			columnObject = GameObject.Instantiate(Resources.Load("NewWallColumn")) as GameObject;
			columnObject.transform.Translate(worldPosition.x, -worldPosition.y, 0);
			columnObject.transform.parent = parent.transform;
		}
		
	}
	
	/**
	 * Getters and Setters and Hassers for a bunch of things
	 * */
	
	public void SetContainer (NewWallGenerator container) {
		this.container = container;
	}
	
	public NewWallGenerator GetContainer () {
		return container;
	}
	
	public void SetWorldPosition (Vector2 position) {
		this.worldPosition = position;
		gridPositionX = Mathf.FloorToInt(position.x)/2;
		gridPositionZ = Mathf.FloorToInt(position.y)/2;
	}
	
	public Vector2 GetWorldPosition () {
		return worldPosition;
	}
	
	public void SetGridPositionX (int gridPositionX) {
		this.gridPositionX = gridPositionX;
		this.worldPosition.x = (float)(gridPositionX * 2);
	}
	
	public void SetGridPositionZ (int gridPositionZ) {
		this.gridPositionZ = gridPositionZ;
		this.worldPosition.y = (float)(gridPositionZ * 2);
	}
	
	public int GetGridPositionX () {
		return gridPositionX;
	}
	
	public int GetGridPositionZ () {
		return gridPositionZ;
	}
	
	public void SetNorthWall (bool northWall) {
		this.northWall = northWall;
	}
	
	public bool GetNorthWall () {
		return northWall;
	}
	
	public void SetEastWall (bool eastWall) {
		this.eastWall = eastWall;
	}

	public bool GetEastWall () {
		return eastWall;
	}
	
	public void SetFloor (bool floor) {
		this.floor = floor;
	}
	
	public void SetColour (int colour) {
		this.colour = colour;
	}
	
	public int GetColour () {
		return colour;
	}
	
	public bool HasNorth () {
		Tile result = GetNorth();
		if (result != null) {
			return true;
		} else {
			return false;
		}
	}
	
	public Tile GetNorth () {
		Tile result = container.GetTileAtIndex(gridPositionX - 1, gridPositionZ);
		return result;
	}
	
	public bool HasEast () {
		Tile result = GetEast();
		if (result != null) {
			return true;
		} else {
			return false;
		}
	}
	
	public Tile GetEast () {
		Tile result = container.GetTileAtIndex(gridPositionX, gridPositionZ + 1);
		return result;
	}
	
	public bool HasSouth() {
		Tile result = GetSouth();
		if (result != null) {
			return true;
		} else {
			return false;
		}
	}
	
	public Tile GetSouth () {
		Tile result = container.GetTileAtIndex(gridPositionX + 1, gridPositionZ);
		return result;
	}
	
	public bool HasWest () {
		Tile result = GetWest();
		if (result != null) {
			return true;
		} else {
			return false;
		}
	}
	
	public Tile GetWest () {
		Tile result = container.GetTileAtIndex(gridPositionX, gridPositionZ - 1);
		return result;
	}
	
	public bool HasNorthEast () {
		//This assumes a square or rectangular building, i.e. no L shapes
		return (HasEast() && HasNorth());
	}
	
	public Tile GetNorthEast () {
		return GetNorth().GetEast();
	}
	
	public bool HasNorthWest () {
		return (HasNorth() && HasWest());
	}
	
	public Tile GetNorthWest () {
		return GetNorth().GetWest();
	}
	
	public bool HasSouthEast() {
		return (HasSouth() && HasEast());
	}
	
	public Tile GetSouthEast() {
		return GetSouth().GetEast();
	}
	
	public bool HasSouthWest() {
		return (HasSouth() && HasWest());
	}
	
	public Tile GetSouthWest() {
		return GetSouth().GetWest();
	}
	
	/**
	 * End of Getters and Setters and Hassers
	 * */
	
	public void MakeSolid () {
		SetNorthWall(true);
		SetEastWall(true);
		SetFloor(true);
	}
	
	private bool ShouldHaveColumn () {
		int wallCount = 0;
		
		if (northWall) wallCount++;
		if (eastWall) wallCount++;
		if ((HasEast()) && (GetEast().GetNorthWall())) wallCount++;
		if ((HasNorth()) && (GetNorth().GetEastWall())) wallCount++;
		
		// can change this to wallCount > 1 for no columns at the end of single walls
		if (wallCount > 0) return true;
		return false;
	}
	
}
