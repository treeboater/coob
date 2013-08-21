using UnityEngine;
using System.Collections;

public class AdditionalCaracterControl : MonoBehaviour {
	
	
	public float walkSpeed = 7;
	public float runSpeed = 20;
	
	
	public CharacterMotor chMotor;
	
	// Use this for initialization
	void Start () {
		chMotor =  GetComponent<CharacterMotor>();
	}
	
	// Update is called once per frame
	void Update () {
		//var speed = walkSpeed;
    	/*if ((Input.GetKey("left shift") || Input.GetKey("right shift")) && chMotor.grounded)
        {
            speed = runSpeed;       
        }
		
		chMotor.movement.maxForwardSpeed = speed;*/
	}
}
