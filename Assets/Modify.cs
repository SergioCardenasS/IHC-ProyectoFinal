using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modify : MonoBehaviour {

	private Vector3 initialPosition;
	// Use this for initialization
	void Start () {
		initialPosition = transform.position;
	}

	public void Move(float x, float y, float z){
		transform.Translate(x,y,z,Space.World);
	}

	public void Set(Vector3 newPosition){
		transform.position = newPosition;
	}

	public void Reset(){
		transform.position = initialPosition;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
