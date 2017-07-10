using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMaster : MonoBehaviour {

	private bool start;
	private GameObject gesture;
	private GameObject instruccion;
	private GameObject master;
	private GameObject bubble;
	private int actualMenu;
	private List<GameObject> gestures;
	private List<GameObject> instrucciones;
	private List<GameObject> bubbles;
	private List<float> times;
	private List<int> whatToSay;
	// Use this for initialization

	void Start () {
		start = false;
		actualMenu = 0;
		times = new List<float>{4.0f, 4.2f, 5.0f, 4.5f}; //3.5f
		whatToSay = new List<int>{7, 32, 15, 26}; //21

		gestures = new List<GameObject>{ };
		gesture = GameObject.Find ("Ayuda/Gestures");
		foreach (Transform child in gesture.transform) {
			gestures.Add (child.gameObject);
		}

		master = GameObject.Find ("ImageTarget/Master");

		instrucciones = new List<GameObject>{ };
		instruccion = GameObject.Find ("ImageTarget/Instrucciones");
		foreach (Transform child in instruccion.transform) {
			instrucciones.Add (child.gameObject);
		}

		bubbles = new List<GameObject>{ };
		bubble = GameObject.Find ("Ayuda/bubbles");
		foreach (Transform child in bubble.transform) {
			bubbles.Add (child.gameObject);
		}

		activateBubble(0);

	}

	int mod(int x, int m) {
    return (x%m + m)%m;
	}

	public void activateMenuSig(){
		deactivateBubble(actualMenu);
		activateBubble(mod(actualMenu+1,3));
		actualMenu = mod(actualMenu+1,3);

	}

	public void activateMenuAnt(){
		deactivateBubble(actualMenu);
		activateBubble(mod(actualMenu-1,3));
		actualMenu = mod(actualMenu-1,3);
	}

	public void activateBubble(int bubble){
		bubbles[bubble].SetActive(true);
	}
	public void deactivateBubble(int bubble){
		bubbles[bubble].SetActive(false);
	}


	public void activateBubbles(){
		for(int i = 0; i < bubbles.Count; i++){
			bubbles [i].SetActive (true);
		}
	}

	public void deactivateBubbles(){
		for(int i = 0; i < bubbles.Count; i++){
			bubbles [i].SetActive (false);
		}
	}

	public IEnumerator speak(int index, float time){
		//vbMaster.GetComponent<VBMaster> ().UnregisterButtons ();
		//sign.SetActive (false);

		instrucciones [index].GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (time);

		//sign.SetActive (true);
		//vbMaster.GetComponent<VBMaster> ().RegisterButtons ();
	}

	public IEnumerator presentCR(int i, bool afterIntro = true){
		deactivateBubbles ();
		gestures [i].SetActive (true);
		yield return speak (whatToSay[i], times[i]);
		yield return new WaitForSeconds (2.0f);
		gestures [i].SetActive (false);
		yield return new WaitForSeconds (1.0f);

		if (afterIntro)
			activateBubbles ();
	}

	public void present(int i){
		StartCoroutine (presentCR (i));
	}


	IEnumerator helpMeCR(){
		yield return null;
		//GetComponent<AudioSource> ().Play ();

		/*deactivateBubble ();

		yield return speak (8, 3.5f);

		for (int i = 0; i < times.Count; i++) {
			yield return presentCR (i,false);
		}

		activateBubble ();*/


		//yield return new WaitForSeconds(5.0f);
		/*gestures [0].SetActive (true);

		gestures [0].SetActive (false);

		yield return new WaitForSeconds(2.0f);
		gestures [1].SetActive (true);
		yield return new WaitForSeconds (2.0f);
		gestures [1].SetActive (false);

		yield return new WaitForSeconds(2.0f);
		gestures [2].SetActive (true);
		yield return new WaitForSeconds (2.0f);
		gestures [2].SetActive (false);

		yield return new WaitForSeconds (2.0f);*/
	}

	public void helpMe(){
		StartCoroutine (helpMeCR ());
	}
	// Update is called once per frame
	void Update () {
		
	}
}
