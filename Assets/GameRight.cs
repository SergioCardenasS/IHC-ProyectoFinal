using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRight : MonoBehaviour {
	private System.Object threadLocker = new System.Object ();
	//public Text countText;

	// Use this for initialization
	private const int NUM_KEYS = 13;
	private GameObject[] keys;
	//private GameObject redCircle;
	//private GameObject greenCircle;
	//private GameObject finger;
	private GameObject vbMaster;
	private List<int> sequence;
	private string[] key_names;
	private bool flag, start, ini;
	private int rnd_num, countSeq;
	public int num_seq;
	private MaterialPropertyBlock props;
	void Start () {
		//countText.text = "0";
		countSeq = 0;
		flag = false;
		start = false;
		ini = true;
		num_seq = 0;
		key_names = new string[NUM_KEYS] {"DoR", "DoSosR", "ReR", "ReSosR", "MiR", "FaR", "FaSosR", "SolR", "SolSosR", "LaR", "LaSosR", "SiR", "DoOpR"};
		sequence = new List<int>{};
		keys = new GameObject[NUM_KEYS];
		//greenCircle = GameObject.Find ("ImageTargetRight/Green circle");
		//redCircle = GameObject.Find ("ImageTargetRight/Red circle");
		//finger = GameObject.Find ("ImageTargetRight/finger2");
		vbMaster = GameObject.Find ("ImageTargetRight");
		//finger.SetActive (false);
		for (int i = 0; i < NUM_KEYS; i++) {
			keys [i] = GameObject.Find ("ImageTargetRight/VirtualButton"+key_names[i]+"/"+key_names[i]);
		}
		sequence.Add (Random.Range (0, NUM_KEYS));
		//finger.GetComponent<Modify>().Move(0.12f * (float)sequence[0],0.0f,0.0f);
		//StartCoroutine (Play ());
	}



	public void decision(bool passed){
		if (passed)
			num_seq++;
		/*else {
			if (ini) {
				num_seq = 0;
			} else
				num_seq = -1;
		}*/
	}

	public bool checkCorrect(int index){
		lock(threadLocker){
			if (num_seq > -1 && num_seq < sequence.Count && sequence [num_seq] == index) {
				//num_seq++;
				return true;
			} else {

				//num_seq = -1;
				return false;
			}
		}
	}

	IEnumerator finalResult(bool result){
		if (result) {
			for (int j = 0; j < 3; j++) {
				for (int i = 0; i < NUM_KEYS; i++) {
					ColorGreen (i, true);
					soundKey (i);
				}
				yield return new WaitForSeconds (1.0f);
				for (int i = 0; i < NUM_KEYS; i++) {
					ColorGreen (i, false);
				}
				yield return new WaitForSeconds (1.0f);
			}
		} else {
			for (int j = 0; j < 3; j++) {
				for (int i = 0; i < NUM_KEYS; i++) {
					ColorRed (i, true);
					soundKey (i);
				}
				yield return new WaitForSeconds (1.0f);
				for (int i = 0; i < NUM_KEYS; i++) {
					ColorRed (i, false);
				}
				yield return new WaitForSeconds (1.0f);
			}
		}
	}

	public void soundKey(int index){
		keys [index].GetComponent<AudioSource> ().Play ();
	}

	public void ColorRed(int index, bool activate){
		props = new MaterialPropertyBlock ();
		if (activate)
			props.SetColor ("_Color", Color.red);
		else
			props.SetColor ("_Color", Color.white);
		keys[index].GetComponent<Renderer> ().SetPropertyBlock (props);
	}

	public void ColorGreen(int index, bool activate){
		props = new MaterialPropertyBlock ();
		if (activate)
			props.SetColor ("_Color", Color.green);
		else
			props.SetColor ("_Color", Color.white);
		keys[index].GetComponent<Renderer> ().SetPropertyBlock (props);
	}

	public void ColorYellow(int index, bool activate){
		props = new MaterialPropertyBlock ();
		if (activate)
			props.SetColor ("_Color", Color.yellow);
		else
			props.SetColor ("_Color", Color.white);
		keys[index].GetComponent<Renderer> ().SetPropertyBlock (props);
	}

	public void ColorSienna(int index, bool activate){
		props = new MaterialPropertyBlock ();
		if (activate)
			props.SetColor ("_Color", new Color(160.0f/255.0f,82.0f/255.0f,45.0f/255.0f)); 
		else
			props.SetColor ("_Color", Color.white);
		keys[index].GetComponent<Renderer> ().SetPropertyBlock (props);
	}

	public void ColorBlue(int index, bool activate){
		props = new MaterialPropertyBlock ();
		if (activate)
			props.SetColor ("_Color", Color.blue); 
		else
			props.SetColor ("_Color", Color.white);
		keys[index].GetComponent<Renderer> ().SetPropertyBlock (props);
	}

	public void ColorGray(int index, bool activate){
		props = new MaterialPropertyBlock ();
		if (activate)
			props.SetColor ("_Color", Color.gray); 
		else
			props.SetColor ("_Color", Color.white);
		keys[index].GetComponent<Renderer> ().SetPropertyBlock (props);
	}

	public void ColorOrange(int index, bool activate){
		props = new MaterialPropertyBlock ();
		if (activate)
			props.SetColor ("_Color", new Color(1.0f, 165.0f/255.0f, 0.0f)); 
		else
			props.SetColor ("_Color", Color.white);
		keys[index].GetComponent<Renderer> ().SetPropertyBlock (props);
	}

	public void ColorSkyBlue(int index, bool activate){
		props = new MaterialPropertyBlock ();
		if (activate)
			props.SetColor ("_Color", new Color(135.0f/255.0f, 206.0f/255.0f, 235.0f/255.0f)); 
		else
			props.SetColor ("_Color", Color.white);
		keys[index].GetComponent<Renderer> ().SetPropertyBlock (props);
	}

	public void MasterColor(int index, bool activate){
		if (index == 0 || index == 1 || index == 12)
			ColorYellow (index, activate);
		else if (index == 2 || index == 3)
			ColorGreen (index, activate);
		else if (index == 4)
			ColorBlue (index, activate);
		else if (index == 5 || index == 6)
			ColorSkyBlue (index, activate);
		else if (index == 7 || index == 8)
			ColorGray (index, activate);
		else if (index == 9 || index == 10)
			ColorOrange (index, activate);
		else if (index == 11)
			ColorSienna (index, activate);
	}

	void setCountText(){
		//countText.text = countSeq.ToString ();
	}

	IEnumerator PlaySequence(){
		/*redCircle.GetComponent<Paint> ().paintRed (true);
		greenCircle.GetComponent<Paint> ().paintGreen (false);
		finger.SetActive (true);*/
		vbMaster.GetComponent<VBMasterRight> ().UnregisterButtons ();
		countSeq = 0;

		for (int i = 0; i < sequence.Count; i++) {
			/*finger.GetComponent<Modify> ().Reset ();
			finger.GetComponent<Modify>().Set(keys[sequence[i]].transform.position);
			finger.GetComponent<Modify>().Move(0.75f,0.2f,-3.0f);*/

			MasterColor (sequence [i], true);
			keys [sequence [i]].GetComponent<AudioSource> ().Play ();
			yield return new WaitForSeconds (0.5f);
			countSeq++;
			setCountText ();
			MasterColor (sequence [i], false);
			//finger.GetComponent<Modify> ().Move (0.0f,0.0f,-2.6f);
			yield return new WaitForSeconds (0.5f);
		}

		vbMaster.GetComponent<VBMasterRight> ().RegisterButtons ();
		/*finger.SetActive(false);
		greenCircle.GetComponent<Paint> ().paintGreen (true);
		redCircle.GetComponent<Paint> ().paintRed (false);*/
		//yield return new WaitForSeconds (3.0f);
	}

	IEnumerator Play(){
		for (int i = 0; i < 10; i++) {
			yield return new WaitForSeconds (3.5f);
			num_seq = 0;
			rnd_num = Random.Range (0, NUM_KEYS);
			sequence.Add (rnd_num);
			yield return PlaySequence();
			while (num_seq != sequence.Count) {
				if (num_seq == -1)
					goto finale;
				yield return null;
			}
			GetComponent<AudioSource> ().Play ();
			//PlaySequence ();
		}
		finale:
		if (num_seq == -1) {
			yield return finalResult (false);
		} else {
			yield return finalResult (true);
		}
		flag = false;
		sequence.Clear ();
		sequence.Add (Random.Range (0, NUM_KEYS));
		start = false;
	}

	IEnumerator Repeat(){
		num_seq = 0;
		ini = true;
		yield return new WaitForSeconds (1.0f);
		while (num_seq < 1) {
			yield return PlaySequence ();
			yield return new WaitForSeconds (2.0f);
		}
		GetComponent<AudioSource> ().Play ();
		flag = true;
		ini = false;
	}

	public void loadMusic(int music){
		sequence.Clear ();
		if (music == 0) {
			sequence = new List<int>{11,9,7,9,11,11,11,9,9,9,11,2,2,11,9,7,9,11,11,11,11,9,9,11,9,7};
		} else if (music == 1) {
			sequence = new List<int>{2,7,7,7,2,4,4,2,11,11,9,9,7,2,7,7,7,2,4,4,2,11,11,9,9,7,2,2,7,7,7,2,2,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,2,4,4,2,11,11,9,9,7};
		} else {
		}
	}

	public void loadTutorial(){
		sequence.Clear();
		//sequence = new List<int>{2,0,4,9,7,5,11};
		sequence = new List<int>{0,1,2,3,4,5,6,7,8,9,10,11};
	}

	// Update is called once per frame
	void Update () {
		//keys [0].GetComponent<Renderer> ().material.color = Color.red;
		//ColorRed (Random.Range(0,1),true);
		/*if (!start) {
			StartCoroutine (Repeat ());
			start = true;
		}
		else if (flag && start) {
			StartCoroutine (Play ());
			flag = false;
		}*/
	}
}