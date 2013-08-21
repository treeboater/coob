using UnityEngine;
using System.Collections;

public class GridCell : MonoBehaviour {
	// Pointers to neighbouring cells
	private GridCell up;
	private GridCell right;
	private GridCell down;
	private GridCell left;
	public int x;
	public int y;
	// If the cell is a wall (for big block-based walls)
	private bool solid;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public GridCell () {
		solid = false;
	}
	
	// Getters
	public GridCell getUp /*drink the cup*/() {
		return up;
	}
	
	public GridCell getRight /*don't fight*/() {
		return right;
	}
	
	public GridCell getDown /*BOW CHIKA CHIKA*/() {
		return down;
	}
	
	public GridCell getLeft /*get eft*/	() {
		return left;
	}
	
	public bool isSolid () {
		return solid;
	}
	
	// Setters
	public void setUp (GridCell up) {
		this.up = up;
	}
	
	public void setRight (GridCell right) {
		this.right = right;
	}
	
	public void setDown (GridCell down) {
		this.down = down;
	}
	
	public void setLeft (GridCell left) {
		this.left = left;
	}
	
	public void setSolid (bool solid) {
		this.solid = solid;
	}
	
	// Hassers
	public bool hasUp () {
		return up == null ? false : true;
	}
	
	public bool hasRight () {
		return right == null ? false : true;
	}
	
	public bool hasDown () {
		return down == null ? false : true;
	}
	
	public bool hasLeft() {
		return left == null ? false : true;
	}
	
	
}
