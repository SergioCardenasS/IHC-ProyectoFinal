using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintRed : MonoBehaviour {

	private MaterialPropertyBlock props;

	// Use this for initialization
	void Start () {
		
	}

	public void ColorRed(bool activate){
		props = new MaterialPropertyBlock ();
		if (activate)
			props.SetColor ("_Color", Color.red);
		else
			props.SetColor ("_Color", Color.white);
		GetComponent<Renderer> ().SetPropertyBlock (props);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
