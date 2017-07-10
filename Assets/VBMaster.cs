using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VBMaster : MonoBehaviour, IVirtualButtonEventHandler {

	public int menu;
	public bool habla;
	public bool userTurn, userChord;
	private GameObject master;
	private GameObject incorrect;
	private VirtualButtonBehaviour[] vbs;
	private int index;
	private bool[] decisions;
	private bool flag;
	// Use this for initialization
	void Start () {
		index = 0;
		flag = false;
		decisions = new bool[12]{false,false,false,false,false,false,false,false,false,false,false,false};
		/*VirtualButtonBehaviour[]*/ vbs = GetComponentsInChildren < VirtualButtonBehaviour> ();
		for (int i = 0; i < vbs.Length; ++i) {
			vbs [i].RegisterEventHandler (this);
		}
		incorrect = GameObject.Find ("ImageTarget/Incorrect");
		master = GameObject.Find ("ImageTarget/Master");
	}
	
	// Update is called once per frame

	public void UnregisterButtons(){
		for (int i = 0; i < vbs.Length; ++i) {
			vbs [i].UnregisterEventHandler (this);
		}
	}

	public void RegisterButtons(){
		for (int i = 0; i < vbs.Length; ++i) {
			vbs [i].RegisterEventHandler (this);
		}
	}

	public void OnTrackingFound(){
		if(!flag) master.GetComponent<Game>().Final(0);
		flag = true;
	}

	public void OnButtonPressed (VirtualButtonAbstractBehaviour vb){
		switch (vb.VirtualButtonName) {
		case "VirtualButtonDo":
			index = 0;
			break;
		case "VirtualButtonDoSos":
			index = 1;
			break;
		case "VirtualButtonRe":
			index = 2;
			break;
		case "VirtualButtonReSos":
			index = 3;
			break;
		case "VirtualButtonMi":
			index = 4;
			break;
		case "VirtualButtonFa":
			index = 5;
			break;
		case "VirtualButtonFaSos":
			index = 6;
			break;
		case "VirtualButtonSol":
			index = 7;
			break;
		case "VirtualButtonSolSos":
			index = 8;
			break;
		case "VirtualButtonLa":
			index = 9;
				break;
		case "VirtualButtonLaSos":
			index = 10;
			break;
		case "VirtualButtonSi":
			index = 11;
			break;
		}

		if (menu != 1) {
			
			if (!userTurn) {
				//master.GetComponent<Game> ().octoTurn (0);

			} else {
				if (index == 0) {
					//master.GetComponent<Game> ().goBack ();
				}
				if (index == 9) {
					//master.GetComponent<Game> ().Choose (index);
				}
				decisions [index] = true;
				master.GetComponent<Game> ().MasterColor (index, true);
				master.GetComponent<Game> ().checkCorrect (index);
				master.GetComponent<Game> ().soundKey (index, habla);
			}

			/*if (userChord) {
				if (master.GetComponent<Game> ().checkCorrect (decisions)) {
					for (int i = 0; i < 12; i++) {
						if (decisions [i])
							master.GetComponent<Game> ().MasterColor (i, true);
						master.GetComponent<Game> ().soundKey (index);
					}
				}
			}*/
		} else {
			//if(index < 4)
				//master.GetComponent<Game> ().Choose (index);
		}


		//if (menu == 110 || menu == 111) {
			//if (master.GetComponent<Game> ().checkCorrect (index)) {
			//	master.GetComponent<Game> ().MasterColor (index, true);
			//} else
			//	master.GetComponent<Game> ().ColorRed (index, true);
		//} else if (menu == 10) {
			//master.GetComponent<Game> ().MasterColor (index, true);
		//} else if (menu == 1 || menu == 11 || menu % 10 == 9 || true) {
			//master.GetComponent<Game> ().Choose (3);
			//master.GetComponent<AudioSource>().Play();

			/*if (index == 7 && menu % 100 != 99) {
				master.GetComponent<Game> ().MasterColor (index, true);
				master.GetComponent<Game> ().leftArrow ();
			}*/
			/*else if (index == 5)
				master.GetComponent<Game> ().Choose ();
			else if (index == 9)
				master.GetComponent<Game> ().rightArrow ();*/
		/*} else if (menu == 12) {
			if (master.GetComponent<Game> ().checkCorrect (decisions)) {
				for (int i = 0; i < 12; i++) {
					if (decisions [i])
						master.GetComponent<Game> ().MasterColor (i, true);
				}
				master.GetComponent<AudioSource> ().Play ();
			} else {
				for (int i = 0; i < 12; i++) {
					if (decisions [i])
						master.GetComponent<Game> ().ColorRed (i, true);
				}
			}
		} else if (menu == 13) {
			if (master.GetComponent<Game> ().checkCorrect (index)) {
				master.GetComponent<Game> ().nextKey(index*2);
				master.GetComponent<Game> ().MasterColor (index, true);
			} else
				master.GetComponent<Game> ().ColorRed (index, true);
		}*/
		
		

	}

	public void OnButtonReleased (VirtualButtonAbstractBehaviour vb){
		switch (vb.VirtualButtonName) {
			case "VirtualButtonDo":
				index = 0;
				break;
			case "VirtualButtonDoSos":
				index = 1;
				break;
			case "VirtualButtonRe":
				index = 2;
				break;
			case "VirtualButtonReSos":
				index = 3;
				break;
			case "VirtualButtonMi":
				index = 4;
				break;
			case "VirtualButtonFa":
				index = 5;
				break;
			case "VirtualButtonFaSos":
				index = 6;
				break;
			case "VirtualButtonSol":
				index = 7;
				break;
			case "VirtualButtonSolSos":
				index = 8;
				break;
			case "VirtualButtonLa":
				index = 9;
				break;
			case "VirtualButtonLaSos":
				index = 10;
				break;	
			case "VirtualButtonSi":
				index = 11;
				break;
			}
		if (userTurn) {
			master.GetComponent<Game> ().ColorRed (index, false);
			if(master.GetComponent<Game> ().checkCorrect (index)){
				master.GetComponent<Game> ().decision (master.GetComponent<Game> ().checkCorrect (index));
			}
			decisions [index] = false;
		}
		/*if (menu == 110 || menu == 111 || menu == 13) {
			//master.GetComponent<Game> ().checkCorrect (index);
			master.GetComponent<Game> ().decision (master.GetComponent<Game> ().checkCorrect (index));
		}*/

		
	}

	void OnGUI(){}
}
