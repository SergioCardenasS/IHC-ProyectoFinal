using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint : MonoBehaviour {

	// Use this for initialization
	private GameObject cylinderRed;
	private GameObject cylinderGreen;
	void Start () {
		cylinderRed = GameObject.Find ("ImageTarget/Red circle/Cylinder");
		cylinderGreen = GameObject.Find ("ImageTarget/Green circle/Cylinder");
	}

	public void paintRed(bool activate){
		cylinderRed.GetComponent<PaintRed> ().ColorRed (activate);
	}

	public void paintGreen(bool activate){
		cylinderGreen.GetComponent<PaintGreen> ().ColorGreen (activate);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
