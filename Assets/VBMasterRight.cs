using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VBMasterRight : MonoBehaviour, IVirtualButtonEventHandler {

	public int menu;
	public bool userTurn;
	private GameObject master;
	private GameObject true_master;
	private GameObject help_master;
	private GameObject instruccion;
	//private GameObject incorrect;
	private VirtualButtonBehaviour[] vbs;
	private int index;
	private bool[] decisions;
	private bool speakAyuda;
	private List<GameObject> instrucciones;
	// Use this for initialization
	void Start () {
		index = 0;
		speakAyuda = false;
		decisions = new bool[13]{false,false,false,false,false,false,false,false,false,false,false,false,false};
		/*VirtualButtonBehaviour[]*/ vbs = GetComponentsInChildren < VirtualButtonBehaviour> ();
		for (int i = 0; i < vbs.Length; ++i) {
			vbs [i].RegisterEventHandler (this);
		}
		//incorrect = GameObject.Find ("ImageTarget/Incorrect");
		master = GameObject.Find ("ImageTargetRight/MasterR");
		true_master = GameObject.Find ("ImageTarget/Master");
		help_master = GameObject.Find ("ImageTarget/HelpMaster");

		instrucciones = new List<GameObject>{ };
		instruccion = GameObject.Find ("ImageTarget/Instrucciones");
		foreach (Transform child in instruccion.transform) {
			instrucciones.Add (child.gameObject);
		}
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

	public void OnButtonPressed (VirtualButtonAbstractBehaviour vb){
		switch (vb.VirtualButtonName) {
		case "VirtualButtonDoR":
			index = 0;
			break;
		case "VirtualButtonDoSosR":
			index = 1;
			break;
		case "VirtualButtonReR":
			index = 2;
			break;
		case "VirtualButtonReSosR":
			index = 3;
			break;
		case "VirtualButtonMiR":
			index = 4;
			break;
		case "VirtualButtonFaR":
			index = 5;
			break;
		case "VirtualButtonFaSosR":
			index = 6;
			break;
		case "VirtualButtonSolR":
			index = 7;
			break;
		case "VirtualButtonSolSosR":
			index = 8;			
			break;
		case "VirtualButtonLaR":
			index = 9;
			break;
		case "VirtualButtonLaSosR":
			index = 10;
			break;
		case "VirtualButtonSiR":
			index = 11;
			break;
		case "VirtualButtonDoOpR":
			index = 12;
			break;
		}

		//32 15 26

		decisions [index] = true;
		if (menu == 1) {
			if (index < 4) {
				//true_master.GetComponent<Game> ().Choose (index);

				if(index == 0) true_master.GetComponent<Game>().activateMenuAnt();
				if(index == 2) true_master.GetComponent<Game>().activateMenuSig();
				if(index == 1) true_master.GetComponent<Game> ().chooseMenu();
			}
				
			if (index == 12)
				true_master.GetComponent<Game> ().Choose (9);
			//if (index == 5) instrucciones[27].GetComponent<AudioSource>().Play();
			if (index == 5) {
				if (!speakAyuda)
					instrucciones [7].GetComponent<AudioSource> ().Play ();
				else
					help_master.GetComponent<HelpMaster> ().present (0);
			}
			if (index == 6) {
				if (!speakAyuda)
					instrucciones [32].GetComponent<AudioSource> ().Play ();
				else
					help_master.GetComponent<HelpMaster> ().present (1);
			}
			if (index == 7) {
				if (!speakAyuda)
					instrucciones [15].GetComponent<AudioSource> ().Play ();
				else
					help_master.GetComponent<HelpMaster> ().present (2);
			}
			if (index == 8) {
				if (!speakAyuda)
					instrucciones [26].GetComponent<AudioSource> ().Play ();
				else
					help_master.GetComponent<HelpMaster> ().present (3);
			}
			if (index == 9)
				speakAyuda = !speakAyuda;
			if (index == 11) {
				instrucciones[13].GetComponent<AudioSource>().Play();
				Application.Quit ();
			}
		} else if (menu % 10 != 9) {
			if (index == 0)
				true_master.GetComponent<Game> ().goBack ();
			if (index == 12)
				true_master.GetComponent<Game> ().Choose (9);
			//if (index == 5) instrucciones[27].GetComponent<AudioSource>().Play();
			if (index == 5) {
				if (!speakAyuda)
					instrucciones [7].GetComponent<AudioSource> ().Play ();
				else
					help_master.GetComponent<HelpMaster> ().present (0);
			}
			if (index == 6) {
				if (!speakAyuda)
					instrucciones [32].GetComponent<AudioSource> ().Play ();
				else
					help_master.GetComponent<HelpMaster> ().present (1);
			}
			if (index == 7) {
				if (!speakAyuda)
					instrucciones [15].GetComponent<AudioSource> ().Play ();
				else
					help_master.GetComponent<HelpMaster> ().present (2);
			}
			if (index == 8) {
				if (!speakAyuda)
					instrucciones [26].GetComponent<AudioSource> ().Play ();
				else
					help_master.GetComponent<HelpMaster> ().present (3);
			}
			if (index == 9)
				speakAyuda = !speakAyuda;
			if (index == 11) {
				instrucciones[13].GetComponent<AudioSource>().Play();
				Application.Quit ();
			}
		} else {
			if (index == 0)
				true_master.GetComponent<Game> ().goBack ();
		}
		/*if (menu == 110 || menu == 111) {
			if (index == 12) {
				master.GetComponent<GameRight> ().MasterColor (index, true);
				true_master.GetComponent<Game> ().goToOptions ();
			}
			else if (master.GetComponent<GameRight> ().checkCorrect (index)) {
				master.GetComponent<GameRight> ().MasterColor (index, true);
			} else
				master.GetComponent<GameRight> ().ColorRed (index, true);
		} else if (menu == 10) {
			master.GetComponent<GameRight> ().MasterColor (index, true);
		} else if (menu == 1 || menu == 11 || menu % 10 == 9) {
			if (menu % 100 != 99) {
				if (index == 1) {
					master.GetComponent<GameRight> ().MasterColor (index, true);
					true_master.GetComponent<Game> ().Choose ();
				} else if (index == 7) {
					master.GetComponent<GameRight> ().MasterColor (index, true);
					true_master.GetComponent<Game> ().rightArrow ();
				} else if (index == 12 && menu % 10 == 9) {
					true_master.GetComponent<Game> ().goBack ();
				}
			}
			else if (index == 12) {
				true_master.GetComponent<Game> ().goBack ();
			} 
		} else if (menu == 12) {
			if (index == 12) {
				master.GetComponent<GameRight> ().MasterColor (index, true);
				true_master.GetComponent<Game> ().goToOptions ();
			}
		} else if (menu == 13) {
			if (index == 12) {
				true_master.GetComponent<Game> ().goBack ();
			} 
			else if (master.GetComponent<GameRight> ().checkCorrect (index)) {
				master.GetComponent<GameRight> ().MasterColor (index, true);
			} else
				master.GetComponent<GameRight> ().ColorRed (index, true);
		}*/

		
		master.GetComponent<GameRight> ().soundKey (index);


	}

	public void OnButtonReleased (VirtualButtonAbstractBehaviour vb){
		switch (vb.VirtualButtonName) {
		case "VirtualButtonDoR":
			index = 0;
			break;
		case "VirtualButtonDoSosR":
			index = 1;
			break;
		case "VirtualButtonReR":
			index = 2;
			break;
		case "VirtualButtonReSosR":
			index = 3;
			break;
		case "VirtualButtonMiR":
			index = 4;
			break;
		case "VirtualButtonFaR":
			index = 5;
			break;
		case "VirtualButtonFaSosR":
			index = 6;
			break;
		case "VirtualButtonSolR":
			index = 7;
			break;
		case "VirtualButtonSolSosR":
			index = 8;
			break;
		case "VirtualButtonLaR":
			index = 9;
			break;
		case "VirtualButtonLaSosR":
			index = 10;
			break;	
		case "VirtualButtonSiR":
			index = 11;
			break;
		case "VirtualButtonDoOpR":
			index = 12;
			break;
		}

		master.GetComponent<GameRight> ().ColorRed (index, false);
		if (menu == 110 || menu == 111 || menu == 13) {
			//master.GetComponent<GameRight> ().checkCorrect (index);
			master.GetComponent<GameRight> ().decision (master.GetComponent<GameRight> ().checkCorrect (index));
		}
		decisions [index] = false;
	}

	void OnGUI(){}
}