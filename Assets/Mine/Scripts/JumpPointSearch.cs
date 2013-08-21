using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpPointSearch : MonoBehaviour {

	public Tile[,] map;
	private static Tile theVeryStart;
	private static Tile theVeryEnd;

	public static void setVeryStart(Tile theStart){
		theVeryStart = theStart;
	}

	public static void setVeryEnd(Tile theEnd){
		theVeryEnd = theEnd;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public static void aStarSearch(Tile thisCell, Tile theEnd){
		//Well fuck this.
		theVeryEnd = theEnd;
		theVeryStart = thisCell;
		print("Here: "+ thisCell.GetWorldPosition().x+", "+thisCell.GetWorldPosition().y);
		LinkedList<Tile> theOpenList = new LinkedList<Tile>();
		LinkedList<Tile> theClosedList = new LinkedList<Tile>();
		theOpenList.AddFirst(thisCell);
		//int count = 0;
		while (true){
			theClosedList.AddLast(thisCell);
			Tile theCurrent = theOpenList.Last.Value;
			//theOpenList.Remove(theCurrent);
			theOpenList.AddLast(calculateBestNeighbor(theCurrent));
			//showPath(theOpenList);
			if (theOpenList.Last.Value.Equals(theVeryEnd) || theOpenList.Last.Value.Equals(theCurrent))
				break;
			//count++;
		}
		showPath(theOpenList);
	}
	
	private static void showPath(LinkedList<Tile> newPath){
		foreach (Tile tile in newPath){
			GameObject marker;
			marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
			marker.name = "pathway";
			marker.renderer.material.shader = Shader.Find("Diffuse");
			if (tile.Equals(theVeryStart))
				marker.renderer.material.color = new Color(0.0f, 1.0f, 0.0f, 1);
			else if (tile.Equals(theVeryEnd))
				marker.renderer.material.color = new Color(1.0f, 0.0f, 0.0f, 1);
			else
				marker.renderer.material.color = new Color(0.0f, 0.0f, 1.0f, 1);
			marker.transform.position = new Vector3(tile.GetWorldPosition().x, 1, tile.GetWorldPosition().y);
			marker.collider.isTrigger = true;
		}
	}

	private static Tile calculateBestNeighbor(Tile current){
		Tile[] neighbors = new Tile[8];
		for (int i = 0; i < 8; i++)
			neighbors[i] = current;
		print(current.GetWorldPosition().x+", "+current.GetWorldPosition().y);
		if (current.HasNorth())
			neighbors[0] = current.GetNorth();
		if (current.HasNorthEast())
			neighbors[1] = current.GetNorthEast();
		if (current.HasEast())
			neighbors[2] = current.GetEast();
		if (current.HasSouthEast())
			neighbors[3] = current.GetSouthEast();
		if (current.HasSouth())
			neighbors[4] = current.GetSouth();
		if (current.HasSouthWest())
			neighbors[5] = current.GetSouthWest();
		if (current.HasWest())
			neighbors[6] = current.GetWest();
		if (current.HasNorthWest())
			neighbors[7] = current.GetNorthWest();
		print("halfway");
		float shortest = Mathf.Infinity;
		int whichNBR = -1;
		int failedDistance = (NewWallGenerator.Instance.getSizeX()+1)*(NewWallGenerator.Instance.getSizeZ()+1);
		for (int i = 0; i < 8; i++){
			if (neighbors[i].Equals(current))continue;
			float distance = manDist(neighbors[i], theVeryStart) + manDist(neighbors[i], theVeryEnd);
			//if (neighbors[i].isSolid())
			//	distance = Mathf.Infinity;
			if (i == 0)
				if(current.GetNorthWall() /*&& !current.HasNorth()*/)
					distance = failedDistance;
			if (i == 1)
				if(neighbors[0].GetEastWall() && neighbors[2].GetNorthWall() /*&& !neighbors[0].HasEast() && !neighbors.HasEast()*/)
					distance = failedDistance;
			if (i == 2)
				if(current.GetEastWall() /*&& !current.HasEast()*/)
					distance = failedDistance;
			if (i == 3)
				if(neighbors[3].GetNorthWall() && neighbors[4].GetEastWall() /*&& !neighbors[3].HasNorth() && !neighbors[4].HasEast()*/)
					distance = failedDistance;	
			if (i == 4)
				if(neighbors[4].GetNorthWall() /*&& !neighbors[4].HasNorth()*/)
					distance = failedDistance;		
			if (i == 5)
				if(neighbors[5].GetEastWall() && neighbors[5].GetNorthWall() /*&& !neighbors[5].HasEast() && !neighbors[5].HasNorth()*/)
					distance = failedDistance;			
			if (i == 6)
				if(neighbors[6].GetEastWall() /*&& !neighbors[6].HasEast()*/)
					distance = failedDistance;
			if (i == 7)
				if(neighbors[6].GetNorthWall() && neighbors[7].GetEastWall() /*&& !neighbors[6].HasNorth() && !neighbors[7].HasEast()*/)
					distance = failedDistance;
			print(distance);
			if (neighbors[i].Equals(theVeryEnd))
				return neighbors[i];
			if (distance < shortest){
				shortest = distance;
				whichNBR = i;
			}
			
		}
		
		if (whichNBR != -1)
			return neighbors[whichNBR];
		else
			return current;
	}
	

	public LinkedList<Tile> identifySuccessors(Tile current, Tile start, Tile end){
		LinkedList<Tile> successors = new LinkedList<Tile>();
		LinkedList<Tile> neighbours = nodeNeighbours(current);

		foreach(Tile nbr in neighbours){
			int dX = Mathf.Max(Mathf.Min(nbr.GetGridPositionX() - current.GetGridPositionX(), -1), 1);
			int dY = Mathf.Max(Mathf.Min(nbr.GetGridPositionZ() - current.GetGridPositionZ(), -1), 1);

			Tile jumpPoint = jump(current.GetGridPositionZ(), current.GetGridPositionZ(), dX, dY, start, end);

			if (jumpPoint != null)
				successors.AddLast(jumpPoint);
		}
		return successors;
	}

	public Tile jump(int currentX, int currentY, int dX, int dY, Tile start, Tile end){
		int nextX = currentX + dX;
		int nextY = currentY + dY;

		//if(map[nextX, nextY].isSolid()) return null;

		if((nextX == end.GetGridPositionX()) && (nextY == end.GetGridPositionZ())) return end;

		return start;
	}

	public int clamp(int loc, int a, int b){
		return (loc * a * b);
	}

	public static float manDist(Tile start, Tile end){
		return Mathf.Abs(end.GetGridPositionX() - start.GetGridPositionX()) + Mathf.Abs(end.GetGridPositionZ() - start.GetGridPositionZ());
	}

	public LinkedList<Tile> nodeNeighbours(Tile current){
		LinkedList<Tile> result = new LinkedList<Tile>();
		if (current.HasNorth())
			result.AddLast(current.GetNorth());
		else if (current.HasNorthEast())
				result.AddLast(current.GetNorthEast());
		else if (current.HasEast())
			result.AddLast(current.GetEast());
		else if (current.HasSouthEast())
				result.AddLast(current.GetSouthEast());
		else if (current.HasSouth())
			result.AddLast(current.GetSouth());
		else if (current.HasSouthWest())
				result.AddLast(current.GetSouthWest());
		else if (current.HasWest())
			result.AddLast(current.GetWest());
		else if (current.HasNorthWest())
				result.AddLast(current.GetNorthWest());
		return result;
	}
}
