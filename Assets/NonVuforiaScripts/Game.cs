using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine.VR;
using System.Linq;
using Random=UnityEngine.Random;


public class Game : MonoBehaviour {
	private Socket _clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
	private byte[] _recieveBuffer = new byte[648];

	private System.Object threadLocker = new System.Object ();
	public Text countText;
	public int menu;

	public bool startFlag;
	public bool publicCorrectFlag;
	public bool publicIncorrectFlag;
	private bool correctFlag;
	private bool incorrectFlag;

	// Use this for initialization
	private const int NUM_KEYS = 12;
	private bool incorrectKey;
	private int last_i;
	private int actualMenu;
	private int section_index;
	private bool section_cantar;
	private GameObject[] keys;
	private GameObject redCircle;
	private GameObject greenCircle;
	private GameObject finger;
	private GameObject menuOp;
	private GameObject menu1;
	private List<GameObject> menu1s;
	private GameObject menuGl;
	private GameObject masterRight;
	private GameObject instruccion;
	private GameObject sign, sign2, sign3;
	private GameObject octopus;
	private GameObject octoPlay;
	private GameObject border;
	private GameObject upborder;
	private GameObject musicFingers;
	private GameObject notasHabladas;
	private GameObject happy;
	private GameObject Introfinger;
	private GameObject confused;
	private GameObject leftHand, rightHand;
	private GameObject leftAll, rightAll;
	private GameObject fingertipsPlay;
	private GameObject handFiller;
	private GameObject back, exit;
	private GameObject menu1chosen;
	private List<GameObject> menu1chosens;
	private List<GameObject> handFillers;
	private List<GameObject> octoPlays;
	private GameObject vbMaster;
	private GameObject vbMasterRight;
	private List<int> sequence, introSequence;
	private List<GameObject> menuPics;
	private List<GameObject> instrucciones;
	private List<GameObject> signs, signs2, signs3;
	private List<List<int> > allSequence;
	private List<GameObject> borders;
	private List<GameObject> Introfingers;
	private List<GameObject> upborders;
	private List<GameObject> subNotasHabladas;
	private List<GameObject> line;
	private List<GameObject> rightAlls, leftAlls;
	private List<GameObject> handSlaps;
	private GameObject handSlap;
	private List<List<GameObject> > fingertipsPlays;
	private string[] key_names;
	private char simulation;
	private GameObject[] visibleMenu;
	private bool flag, start, ini, flag_seq, is_piano, is_estirado, is_1, active_piano;
	private int rnd_num, num_seq, countSeq, menuActual, posActual, cont_piano, cont_estirado, cont_1;
	private int[,] chords;
	private MaterialPropertyBlock props;
	private GameObject sphere;
	private List<GameObject> gestures;
	private GameObject gesture;
	private Vector3 signPosition;
	private IEnumerator myThread;
	private bool canContinue;

	private Vector3 octopus_original_place;

	private void SetupServer(String ip_address)
	{

		try
		{
			_clientSocket.Connect(new IPEndPoint(IPAddress.Parse("192.168.43.208"), 9011));
			//_clientSocket.Connect(new IPEndPoint(IPAddress.Parse("192.168.0.38"), 9001));
			//_clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ip_address), 9015));
		}
		catch(SocketException ex)
		{
			Debug.Log(ex.Message);
		}
		_clientSocket.BeginReceive(_recieveBuffer,0,_recieveBuffer.Length,SocketFlags.None,new AsyncCallback(ReceiveCallback),null);

	}

	void Start () {
		canContinue = false;
		active_piano = false;
		simulation = '*';
		is_piano = false;
		is_estirado = false;
		is_1 = false;
		cont_estirado = 0;
		cont_1 = 1;
		cont_piano = 0;
		incorrectKey = false;
		countText.text = "0";
		section_index = 0;
		section_cantar = false;
		countSeq = 0;
		flag = false;
		correctFlag = false;
		incorrectFlag = true;
		flag_seq = false;
		start = false;
		ini = true;
		last_i = 0;
		//num_seq = -1;
		num_seq = 0;
		posActual = 0;
		IEnumerator myThread =  null;
		key_names = new string[NUM_KEYS] {"Do", "DoSos", "Re", "ReSos", "Mi", "Fa", "FaSos", "Sol", "SolSos", "La", "LaSos", "Si"};
		menuPics = new List<GameObject>{};
		handFillers = new List<GameObject>{ };
		Introfingers = new List<GameObject>{ };
		sequence = new List<int>{};
		introSequence = new List<int>{5,5,6,8,10,5,5,4,3,1};
		instrucciones = new List<GameObject>{ };
		octoPlays = new List<GameObject>{ };
		borders = new List<GameObject>{ };
		upborders = new List<GameObject>{ };
		signs = new List<GameObject>{ };
		signs2 = new List<GameObject>{ };
		signs3 = new List<GameObject>{ };
		gestures = new List<GameObject>{ };
		menu1s = new List<GameObject>{ };


		StartCoroutine (Checho());
		//Debug.Log( mod(-1,10) );

		subNotasHabladas = new List<GameObject>{ };
		rightAlls = new List<GameObject>{ };
		leftAlls = new List<GameObject>{ };
		keys = new GameObject[NUM_KEYS];
		visibleMenu = new GameObject[3];
		menuActual = -1;
		chords = new int[2,3]{ {0,4,7}, {7,9,11} };

		exit = GameObject.Find ("ImageTarget/Exit");
		back = GameObject.Find ("ImageTarget/Back");


		back.SetActive (false);

		handSlaps = new List<GameObject>{ };
		handSlap = GameObject.Find ("ImageTarget/Mano slap");
		foreach (Transform child in handSlap.transform) {
			handSlaps.Add (child.gameObject);
		}

		menu1chosens = new List<GameObject>{ };
		menu1chosen = GameObject.Find ("ImageTarget/Menu1chosen");
		foreach (Transform child in menu1chosen.transform) {
			menu1chosens.Add (child.gameObject);
		}

		handFiller = GameObject.Find ("ImageTarget/HandFiller");
		foreach (Transform child in handFiller.transform) {
			handFillers.Add (child.gameObject);
		}

		gesture = GameObject.Find ("ImageTarget/Gestures");
		foreach (Transform child in gesture.transform) {
			gestures.Add (child.gameObject);
		}

		menu1 = GameObject.Find ("ImageTarget/Menu1");
		foreach (Transform child in menu1.transform) {
			menu1s.Add (child.gameObject);
		}
		deactivateMenu ();

		Introfinger = GameObject.Find ("ImageTarget/IntroRight");
		foreach (Transform child in Introfinger.transform) {
			Introfingers.Add (child.gameObject);
		}

		rightHand = GameObject.Find ("ImageTarget/RightHandT");
		leftHand = GameObject.Find ("ImageTarget/LeftHandT");

		rightHand.SetActive (false);
		leftHand.SetActive (false);

		rightAll = GameObject.Find ("ImageTarget/HandsPlays");
		//leftAll = GameObject.Find ("ImageTarget/HandsPlaysLeft);

		foreach (Transform child in rightAll.transform) {
			rightAlls.Add (child.gameObject);
		}


		allSequence = new List<List<int> >{ new List<int>{10, 8, 6}, new List<int>{0,1,3}, new List<int>{6,6,4,9}, new List<int>{3,1,4}, new List<int>{6,6,4,9} };
		notasHabladas = GameObject.Find ("ImageTarget/Notas habladas");
		foreach (Transform child in notasHabladas.transform) {
			subNotasHabladas.Add (child.gameObject);
		}

		happy = GameObject.Find ("ImageTarget/Happy");
		confused = GameObject.Find ("ImageTarget/Confused");
		confused.SetActive (false);

		border = GameObject.Find ("ImageTarget/Borders");
		foreach (Transform child in border.transform) {
			borders.Add (child.gameObject);
		}

		upborder = GameObject.Find ("ImageTarget/UpBorders");
		foreach (Transform child in upborder.transform) {
			upborders.Add (child.gameObject);
		}

		sign = GameObject.Find ("ImageTarget/Sign");
		signPosition =  sign.transform.position;
		foreach (Transform child in sign.transform) {
			signs.Add (child.gameObject);
		}

		sign2 = GameObject.Find ("ImageTarget/Sign2");
		foreach (Transform child in sign2.transform) {
			signs2.Add (child.gameObject);
		}
		sign2.SetActive (false);

		sign3 = GameObject.Find ("ImageTarget/Sign3");
		foreach (Transform child in sign3.transform) {
			signs3.Add (child.gameObject);
		}
		sign3.SetActive (false);

		octopus = GameObject.Find ("ImageTarget/Octopus");
		octoPlay = GameObject.Find ("ImageTarget/OctoPlays");
		foreach (Transform child in octoPlay.transform) {
			octoPlays.Add (child.gameObject);
		}

		instruccion = GameObject.Find ("ImageTarget/Instrucciones");
		foreach (Transform child in instruccion.transform) {
			instrucciones.Add (child.gameObject);
		}


		/*foreach (Transform child in fingertipsPlay.transform) {
			line.Add (child.gameObject);
		}
		fingertipsPlays.Add (line);
		line.Clear ();

		fingertipsPlay = GameObject.Find ("ImageTarget/FingertipsPlays/Fingertips2");
		foreach (Transform child in fingertipsPlay.transform) {
			line.Add (child.gameObject);
		}
		fingertipsPlays.Add (line);
		line.Clear ();

		fingertipsPlay = GameObject.Find ("ImageTarget/FingertipsPlays/Fingertips3");
		foreach (Transform child in fingertipsPlay.transform) {
			line.Add (child.gameObject);
		}
		fingertipsPlays.Add (line);
		line.Clear ();*/

		line = new List<GameObject>{ };
		fingertipsPlays = new List<List<GameObject> >{ };
		fingertipsPlay = GameObject.Find ("ImageTarget/FingertipsPlays");
		foreach (Transform child in fingertipsPlay.GetComponentInChildren<Transform> ()) {
			line = new List<GameObject>{};
			foreach (Transform subchild in child){
				line.Add (subchild.gameObject);
			}
			fingertipsPlays.Add (line);
		}



		/*if (fingertipsPlays.Count == 3)
			GetComponent<AudioSource> ().Play ();*/

		menuGl = GameObject.Find ("ImageTarget/GlobalMenu");

		sphere = GameObject.Find ("ImageTarget/Sphere");
		greenCircle = GameObject.Find ("ImageTarget/Green circle");
		redCircle = GameObject.Find ("ImageTarget/Red circle");
		finger = GameObject.Find ("ImageTarget/GlobalMenu/Hand");
		vbMaster = GameObject.Find ("ImageTarget");
		vbMasterRight = GameObject.Find ("ImageTargetRight");
		masterRight = GameObject.Find ("ImageTargetRight/MasterR");
		//finger.SetActive (false);
		for (int i = 0; i < NUM_KEYS; i++) {
			keys [i] = GameObject.Find ("ImageTarget/VirtualButton"+key_names[i]+"/"+key_names[i]);
		}
		sequence.Add (Random.Range (0, NUM_KEYS));
		//Final(0);
		//finger.GetComponent<Modify>().Move(0.12f * (float)sequence[0],0.0f,0.0f);
		//StartCoroutine (Play ());
		//Final(0);
		VRSettings.enabled = true;
		SetupServer (PlayerPrefs.GetString("Ip"));
	}

	private void ReceiveCallback(IAsyncResult AR)
	{
		//Check how much bytes are recieved and call EndRecieve to finalize handshake
		int recieved = _clientSocket.EndReceive(AR);
		string result;
		if (recieved <= 0) {
			return;
		}
		//Copy the recieved data into new buffer , to avoid null bytes
		byte[] recData = new byte[recieved];
		result = System.Text.Encoding.ASCII.GetString(_recieveBuffer);
		//Debug.Log(result);
		//Debug.Log(result.Length);

		/*if(result[0]== 'p' & is_piano)
		{
			cont_piano += 1;
			cont_estirado = 0;
			cont_1 = 0;
			if(cont_piano>100)
			{
				active_piano = true;
				is_piano = false;
				is_estirado = false;
				cont_piano = 0;
				cont_1 = 0;
				cont_estirado = 0;
			}
		}
		else if(result[0]=='p')
		{
			cont_estirado = 0;
			cont_1 = 0;
		}
		else if(result[0] == '1' & !is_estirado)
		{
			//GameObject.Find("ImageTarget/Incorrect").GetComponent<AudioSource>().Play();
			cont_1 += 1;
			if(cont_1>105)// segundos
			{
				is_1 = true;
			}
			//Debug.Log("LLEGO 1");
		}
		else if(result[0] == 'e' & !is_1)
		{
			cont_estirado += 1;
			if(cont_estirado>105)// segundos
			{
				is_estirado = true;
			}
		}*/

		if(result[0]== 'q')
		{
			Debug.Log("Opcion 1");
		}
		else if(result[0]== 'w')
		{
			Debug.Log("Opcion 2");
		}
		else if(result[0]== 'e')
		{
			Debug.Log("Opcion 3");
		}

		if(result[0]== 'r')
		{
			Debug.Log("Opcion 4");
		}

		simulation = result[0];
		
				

		//Start receiving again
		_clientSocket.BeginReceive(_recieveBuffer,0,_recieveBuffer.Length,SocketFlags.None,new AsyncCallback(ReceiveCallback),null);
	}

	private void SendData(byte[] data)
	{
		SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
		socketAsyncData.SetBuffer(data,0,data.Length);
		_clientSocket.SendAsync(socketAsyncData);
	}

	public void activateHandSlaps(){
		for (int i = 0; i < handSlaps.Count; i++) {
			handSlaps[i].SetActive (true);
		}
	}

	public void deactivateHandSlaps(){
		for (int i = 0; i < handSlaps.Count; i++) {
			handSlaps[i].SetActive (false);
		}
	}

	public void activateMenu(){
		for (int i = 0; i < menu1s.Count; i++) {
			menu1s [i].SetActive (true);
		}
	}

	public void deactivateMenu(){
		for (int i = 0; i < menu1s.Count; i++) {
			menu1s [i].SetActive (false);
		}
	}

	public void deactivateHands(){
		for (int i = 0; i < handFillers.Count; i++) {
			handFillers [i].SetActive (false);
		}
	}

	public void activateHands(){
		for (int i = 0; i < handFillers.Count; i++) {
			handFillers [i].SetActive (true);
		}
	}

	public void activateMenuOne(int i){
		menu1s [i].SetActive (true);
	}

	public void deactivateMenuOne(int i){
		menu1s [i].SetActive (false);
	}

	public void activateMenuOneChosen(int i){
		menu1chosens [i].SetActive (true);
	}

	public void deactivateMenuOneChosen(int i){
		menu1chosens [i].SetActive (false);
	}

	public IEnumerator Checho(){
		while(true){
			if(simulation != '*'){
				if(menuActual == 1){
					if(simulation == '0') activateMenuAnt(); //Anterior menu
					if(simulation == '2') activateMenuSig(); //Siguiente menu
					if(simulation == '1') chooseMenu(); //Elegir menu
					if(simulation == 'x') Application.Quit(); //Sair de la aplicacion
				}
				if(menuActual == 10 || menuActual == 11){
					if(simulation == 'u'){ //Color pulpo rosado es correcto
						ColorPinkPulpo(section_index);
						canContinue = true;
					}
					if(simulation == 'e'){
						yield return speak(45, 2.5f); //es con mano derecha
						decision(false);
					}
					if(simulation == 'i'){
						yield return speak(45, 2.5f); //es con mano izquierda
						decision(false);
					}
					if(simulation == 'p'){
						yield return speak(39, 2.0f); //No es dedo correcto
						decision(false);
					}
					if(simulation == 'q'){
						yield return speak(23, 3.0f); //No es buena posicion
						decision(false);
					}
					if(simulation == 'b'){
						yield return speak(9, 6.5f); //Bien
						decision(false);
					}
					if(simulation == 'n') ColorOriginalPulpo(section_index); //Menos rosado
					if(simulation == 'd') ColorGrayPulpo(section_index); //Pulpo gris
					else{ //Seccion mano barra
						deactivateAllHandFiller();
						if(simulation == '0') activateHandFiller(0); //0% barra mano
						if(simulation == '1') activateHandFiller(1); //25% barra mano
						if(simulation == '2') activateHandFiller(2); //50% barra mano
						if(simulation == '3') {
							activateHandFiller(3); //75% barra mano
							yield return speak (9, 6.5f); //Bien, pero puedes mejorar
							canContinue = true;
						}
						if(simulation == '4') {
							activateHandFiller(4); //100% barra mano
							canContinue = true;
						}
						if(simulation == '-') deactivateAllHandFiller(); //Adios mano.
					}
				}
				if(menuActual % 10 == 9){
					if(simulation == 'r') canContinue = true; //Bien
					if(simulation == 'w') yield return speak (39,2.0f); //Dedo incorrecto
					if(simulation == 'l') yield return speak(18,3.0f); //Intenta el gesto
				}
				if(simulation == 'a'){ //Retroceder
					goBack();
				}
				if(simulation == 'i'){ //Ir a informacion
					Choose (9);
				}
			}
			
			simulation = '*';
			yield return null;
		}
		
	}

	public void activateHandFiller(int i){
		handFillers [i].SetActive (true);
	}

	public void deactivateHandFiller(int i){
		handFillers [i].SetActive (false);
	}

	public void deactivateAllHandFiller(){
		for(int i = 0; i < handFillers.Count; i++){
			deactivateHandFiller(i);
		}
	}

	public IEnumerator change(bool ayuda = false){
		//Voz le indica que hacer
		if (menu != 1) {
			if (ayuda) {
				yield return speak (19, 3.0f); //Intenta llenar la mano
			}


			handFillers [0].SetActive (true);
			if(menuActual != 13) fingertipsPlays [section_index] [2].SetActive(true);
			yield return new WaitForSeconds (1.0f);
			if(menuActual != 13) fingertipsPlays [section_index] [2].SetActive(false);
			handFillers [0].SetActive (false);

			handFillers [1].SetActive (true);
			if(menuActual != 13) fingertipsPlays [section_index] [1].SetActive(true);
			if(menuActual != 13) fingertipsPlays [section_index] [2].SetActive(true);
			yield return new WaitForSeconds (0.5f);
			if(menuActual != 13) fingertipsPlays [section_index] [1].SetActive(false);
			if(menuActual != 13) fingertipsPlays [section_index] [2].SetActive(false);
			handFillers [1].SetActive (false);

			handFillers [2].SetActive (true);
			if(menuActual != 13) fingertipsPlays [section_index] [0].SetActive(true);
			yield return new WaitForSeconds (0.5f);
			if(menuActual != 13) fingertipsPlays [section_index] [0].SetActive(false);
			handFillers [2].SetActive (false);

			handFillers [3].SetActive (true);
			if(menuActual != 13) fingertipsPlays [section_index] [0].SetActive(true);
			yield return new WaitForSeconds (1.0f);
			if(menuActual != 13) fingertipsPlays [section_index] [0].SetActive(false);
			handFillers [3].SetActive (false);

			handFillers [4].SetActive (true);
			yield return new WaitForSeconds (0.5f);
			handFillers [4].SetActive (false);

			handFillers [3].SetActive (true);
			yield return new WaitForSeconds (1.0f);
			handFillers [3].SetActive (false);


			if(menuActual != 13) fingertipsPlays [section_index] [0].SetActive(true);

			if(ayuda){
				while(!canContinue){
					yield return null;
				}
				canContinue = false;
			}
			

			//Voz le indica el resultado
			if (ayuda) {
				yield return speak (9, 6.5f); //Bien, pero puedes mejorar
			}

			//fingertipsPlays [section_index] [2].SetActive(false);
			if(menuActual != 13) fingertipsPlays [section_index] [0].SetActive(false);
		}

	}

	public IEnumerator waitForPosition(int index){
		octoPlays [index].SetActive (true);
		yield return speak (28, 4.0f);
		ColorPinkPulpo (index);
		yield return new WaitForSeconds (3.0f);

		while(!canContinue){
			yield return null;
		}
		canContinue = false;


		yield return new WaitForSeconds(1.0f);
		/*if (index == 0) { //Se equivoca
			
			yield return speak (10, 2.0f);
			ColorGrayPulpo (index);
			yield return new WaitForSeconds (2.0f);
			ColorPinkPulpo (index);
			yield return new WaitForSeconds (2.0f);
		}*/

		ColorOriginalPulpo (index);



		//Voz que le indica que esta bien
	}

	int mod(int x, int m) {
    return (x%m + m)%m;
	}

	public IEnumerator firstSection(){
		allSequence = new List<List<int> >{ new List<int>{10, 8, 6}, new List<int>{0,1,3}, new List<int>{6,6,4,9}, new List<int>{3,1,4}, new List<int>{6,6,4,9} };

		activateMenuOneChosen (menuActual % 10);
		//yield return new WaitForSeconds (2.0f);
		//deactivateMenuOneChosen (menuActual % 10);

		line = new List<GameObject>{ };
		fingertipsPlays = new List<List<GameObject> >{ };
		fingertipsPlay = GameObject.Find ("ImageTarget/FingertipsPlays");
		foreach (Transform child in fingertipsPlay.GetComponentInChildren<Transform> ()) {
			line = new List<GameObject>{};
			foreach (Transform subchild in child){
				line.Add (subchild.gameObject);
			}
			fingertipsPlays.Add (line);
		}
		yield return speak (29, 8.0f);
		for (int i = last_i; i < allSequence.Count; i++) {
			last_i = i;
			yield return speak (3, 3.2f);

			yield return octoTurnCR (1.5f, i, true);
			section_cantar = true;


			//upborders [sequence [num_seq]].SetActive (false);
			borders [sequence [num_seq]].SetActive (false);
			signs [sequence [num_seq]].SetActive (false);
			vbMaster.GetComponent<VBMaster> ().userTurn = false;
			vbMasterRight.GetComponent<VBMasterRight> ().userTurn = false;
			yield return speak (2, 4.2f);

			//Voz que le indica primero como ponerse

			octopus.SetActive (false);
			happy.SetActive (false);
			confused.SetActive (false);

			if (i == 0 || i == 1 || i == 2)
				yield return speak (40,2.5f); //Posiciona mano derecha
			if (i == 3 || i == 4)
				yield return speak (41,2.5f);

			yield return speak(47,5.5f); //Mas rosado
			yield return waitForPosition (i);

			//Mano
			yield return speak (19, 3.0f); //Intenta llenar la mano
			yield return speak(51, 2.5f); //Posiciona tus dedos como mis tentaculos
			while(!canContinue){
				yield return null;
			}
			canContinue = false;
			//Fin mano

			//yield return change (true);


			octoPlays [i].SetActive (false);

			octopus.SetActive (true);
			happy.SetActive (true);
			confused.SetActive (false);

			/*rightAlls [i].SetActive (true);
			yield return new WaitForSeconds (3.0f);
			rightAlls [i].SetActive (false);*/

			//upborders [sequence [num_seq]].SetActive (true);
			borders [sequence [num_seq]].SetActive (true);
			signs [sequence [num_seq]].SetActive (true);

			vbMaster.GetComponent<VBMaster> ().userTurn = true;
			vbMasterRight.GetComponent<VBMasterRight> ().userTurn = true;
			vbMaster.GetComponent<VBMaster> ().habla = true;
			yield return humanPlay ();
		}
		last_i = 0;
	}

	public IEnumerator secondSection(){

		activateMenuOneChosen (menuActual % 10);
		//yield return new WaitForSeconds (2.0f);
		//deactivateMenuOneChosen (menuActual % 10);
		section_cantar = false;
		vbMaster.GetComponent<VBMaster> ().habla = false;
		vbMaster.GetComponent<VBMaster> ().userTurn = false;
		vbMasterRight.GetComponent<VBMasterRight> ().userTurn = false;
		//yield return speak (4, 2.2f);
		yield return speak (42,6.5f);
		yield return Music (0);


	}

	public IEnumerator thirdSection(){
		activateMenuOneChosen (menuActual % 10);
		//yield return new WaitForSeconds (2.0f);
		//deactivateMenuOneChosen (menuActual % 10);

		section_cantar = false;
		vbMaster.GetComponent<VBMaster> ().habla = false;
		vbMaster.GetComponent<VBMaster> ().userTurn = false;
		vbMasterRight.GetComponent<VBMasterRight> ().userTurn = false;
		//yield return speak (4, 2.2f);
		yield return speak(12,8.0f);

		yield return Music2 (0);
	}

	public IEnumerator fourthSection(){
		activateMenuOneChosen (menuActual % 10);
		//yield return new WaitForSeconds (2.0f);
		//deactivateMenuOneChosen (menuActual % 10);

		vbMaster.GetComponent<VBMaster> ().userTurn = true;
		vbMasterRight.GetComponent<VBMasterRight> ().userTurn = true;
		section_cantar = false;
		vbMaster.GetComponent<VBMaster> ().habla = false;

		yield return speak (24,8.0f);
		while (true) {
			yield return change ();
		}
		yield return null;
	}

	public void decision(bool passed){
		if (passed) {

			confused.SetActive (false);
			happy.SetActive (true);

			/*sign.GetComponent<Rigidbody> ().useGravity = false;
			sign.transform.position = signPosition;*/

			//upborders [sequence [num_seq]].SetActive (false);
			borders [sequence [num_seq]].SetActive (false);
			signs [sequence [num_seq]].SetActive (false);
			num_seq++;
			if (num_seq < sequence.Count) {
				signs [sequence [num_seq]].SetActive (true);
				borders [sequence [num_seq]].SetActive (true);
				//upborders [sequence [num_seq]].SetActive (true);
			}
		} else {
			happy.SetActive (false);
			confused.SetActive (true);

			incorrectKey = true;
			//octoTurnWrong (section_index, section_cantar);
			//sign.GetComponent<Rigidbody> ().useGravity = true;
		}
		/*else {
			if (ini) {
				num_seq = 0;
			} else
				num_seq = -1;
		}*/
	}

	/*public void changeFlagCorrect(){
		correctFlag = !correctFlag;
	}

	public void changeFlagIncorrect(){
		incorrectFlag = !incorrectFlag;
	}*/

	public bool checkCorrect(int index){
		lock(threadLocker){
			if (num_seq > -1 && num_seq < sequence.Count && sequence [num_seq] == index) {
				/*while (correctFlag == publicCorrectFlag && incorrectFlag == publicIncorrectFlag) {
					yield return null;
				}*/

				/*if (correctFlag != publicCorrectFlag) {
					correctFlag = publicCorrectFlag;
				} else if (incorrectFlag != publicIncorrectFlag) {
					incorrectFlag = publicIncorrectFlag;
					return false;
				} else
					return false;*/
				//num_seq++;
				return true;
			} else {
				
				//num_seq = -1;
				return false;
			}
		}
	}

	/*public bool checkCorrectUni(int index){
		if(!ocupado){
	}
	}*/
		
	public bool checkCorrect(bool[] index){
		lock (threadLocker) {
			if (sequence.Count == 2) {
				if (index [sequence [0]] && index [sequence [1]]) {
					num_seq++;
					return true;
				}
			} else if (sequence.Count == 3) {
				if (index [sequence [0]] && index [sequence [1]] && index [sequence [2]]) {
					num_seq++;
					return true;
				}
			}
			return false;
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

	public void soundKey(int index, bool hablado = false){
		if (!hablado)
			keys [index].GetComponent<AudioSource> ().Play ();
		else
			subNotasHabladas [index].GetComponent<AudioSource> ().Play ();
	}

	public void ColorRed(int index, bool activate){
		props = new MaterialPropertyBlock ();
		if (activate)
			props.SetColor ("_Color", Color.red);
		else
			if (index == 1 || index == 3 || index == 6 || index == 8 || index == 10) props.SetColor ("_Color", Color.black);
			else props.SetColor ("_Color", Color.white);
		keys[index].GetComponent<Renderer> ().SetPropertyBlock (props);
	}

	public void ColorGreen(int index, bool activate){
		props = new MaterialPropertyBlock ();
		if (activate)
			props.SetColor ("_Color", Color.green);
		else
			if (index == 1 || index == 3 || index == 6 || index == 8 || index == 10) props.SetColor ("_Color", Color.black);
			else props.SetColor ("_Color", Color.white);
		keys[index].GetComponent<Renderer> ().SetPropertyBlock (props);
	}

	public void ColorYellow(int index, bool activate){
		props = new MaterialPropertyBlock ();
		if (activate)
			props.SetColor ("_Color", Color.yellow);
		else
			if (index == 1 || index == 3 || index == 6 || index == 8 || index == 10) props.SetColor ("_Color", Color.black);
			else props.SetColor ("_Color", Color.white);
		keys[index].GetComponent<Renderer> ().SetPropertyBlock (props);
	}

	public void ColorSienna(int index, bool activate){
		props = new MaterialPropertyBlock ();
		if (activate)
			props.SetColor ("_Color", new Color(160.0f/255.0f,82.0f/255.0f,45.0f/255.0f)); 
		else
			if (index == 1 || index == 3 || index == 6 || index == 8 || index == 10) props.SetColor ("_Color", Color.black);
			else props.SetColor ("_Color", Color.white);
		keys[index].GetComponent<Renderer> ().SetPropertyBlock (props);
	}

	public void ColorBlue(int index, bool activate){
		props = new MaterialPropertyBlock ();
		if (activate)
			props.SetColor ("_Color", Color.blue); 
		else
			if (index == 1 || index == 3 || index == 6 || index == 8 || index == 10) props.SetColor ("_Color", Color.black);
			else props.SetColor ("_Color", Color.white);
		keys[index].GetComponent<Renderer> ().SetPropertyBlock (props);
	}

	public void ColorGray(int index, bool activate){
		props = new MaterialPropertyBlock ();
		if (activate)
			props.SetColor ("_Color", Color.gray);
		else 
			if (index == 1 || index == 3 || index == 6 || index == 8 || index == 10) props.SetColor ("_Color", Color.black);
			else props.SetColor ("_Color", Color.white);
		keys[index].GetComponent<Renderer> ().SetPropertyBlock (props);
	}

	public void ColorOrange(int index, bool activate){
		props = new MaterialPropertyBlock ();
		if (activate)
			props.SetColor ("_Color", new Color(1.0f, 165.0f/255.0f, 0.0f)); 
		else
			if (index == 1 || index == 3 || index == 6 || index == 8 || index == 10) props.SetColor ("_Color", Color.black);
			else props.SetColor ("_Color", Color.white);
		keys[index].GetComponent<Renderer> ().SetPropertyBlock (props);
	}

	public void ColorSkyBlue(int index, bool activate){
		props = new MaterialPropertyBlock ();
		if (activate)
			props.SetColor ("_Color", new Color(135.0f/255.0f, 206.0f/255.0f, 235.0f/255.0f)); 
		else
			if (index == 1 || index == 3 || index == 6 || index == 8 || index == 10) props.SetColor ("_Color", Color.black);
			else props.SetColor ("_Color", Color.white);
		keys[index].GetComponent<Renderer> ().SetPropertyBlock (props);
	}

	public void ColorGrayPulpo(int index){
		props = new MaterialPropertyBlock ();
		props.SetColor ("_Color", Color.gray);	
	octoPlays [index].transform.GetChild (0).gameObject.GetComponent<Renderer> ().SetPropertyBlock (props);
		
	}

	public void ColorPinkPulpo(int index){
		props = new MaterialPropertyBlock ();
	props.SetColor ("_Color", new Color(255.0f/255.0f, 105.0f/255.0f, 180.0f/255.0f));	
	octoPlays [index].transform.GetChild (0).gameObject.GetComponent<Renderer> ().SetPropertyBlock (props);

	}

	public void ColorOriginalPulpo(int index){
		props = new MaterialPropertyBlock ();
		props.SetColor ("_Color", new Color(242.0f/255.0f, 155.0f/255.0f, 242.0f/255.0f));	
	octoPlays [index].transform.GetChild (0).gameObject.GetComponent<Renderer> ().SetPropertyBlock (props);

	}

	public void MasterColor(int index, bool activate){
		if (index == 0 || index == 1)
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
		countText.text = countSeq.ToString ();
	}

	IEnumerator Introduction(){
		GetComponent<AudioSource> ().Play ();
		happy.SetActive (true);
		confused.SetActive (false);
		if(menu != 9) borders [sequence [num_seq]].SetActive (false);

		sign.SetActive (false);
		/*yield return new WaitForSeconds (5.0f);
		gestures [0].SetActive (true);
		yield return new WaitForSeconds (3.0f);
		gestures [0].SetActive (false);*/
		yield return speak (20,11.5f);

		yield return speak (50,5.0f); //Pon tu mano para ayuda
		yield return new WaitForSeconds (2.0f);
		gestures [1].SetActive (true);

		yield return speak (18,3.0f); //Intenta el gesto
		
		while(!canContinue){
			yield return null;
		}
		canContinue = false;

		gestures [1].SetActive (false);

		yield return speak (48,4.0f); //Asi vas a ayuda
		//yield return speak (6,3.5f);

		//GESTO 2
		yield return speak (43,4.0f); //Pon tu mano para ayuda
		yield return new WaitForSeconds (2.0f);
		gestures [2].SetActive (true);

		yield return speak (18,3.0f); //Intenta el gesto
		
		while(!canContinue){
			yield return null;
		}
		canContinue = false;

		gestures [2].SetActive (false);

		yield return speak (49,4.0f); //Asi vas a ayuda

		//FIN GESTO 2


		octopus.SetActive (false);
		happy.SetActive (false);
		confused.SetActive (false);
		octoPlays [0].SetActive (true);

		yield return speak (33, 5.5f);
		yield return pause (33, 0.0f);
		for (int i = 0; i < 5; i++) {
			
			Introfingers [i].SetActive(true);
			yield return unPause (33, 3.4f);
			yield return pause (33, 0.0f);
			//voz que dice que presione con el dedo correcto
			
			yield return speak (35,4.0f);
			borders[introSequence[i]].SetActive(true);
			
			while(!canContinue){
				yield return null;
			}
			canContinue = false;

			/*if (i == 0) { //Se equivoca
				yield return speak (39,2.0f);
				yield return new WaitForSeconds (2.0f);
			}*/

			// bien

			Introfingers [i].SetActive(false);
			borders [introSequence [i]].SetActive (false);
		}

		octoPlays [0].SetActive (false);

		yield return new WaitForSeconds (1.0f);
		octoPlays [3].SetActive (true);


		yield return speak (34, 5.5f);
		yield return pause (34, 0.0f);
		for (int i = 5; i < 10; i++) {
			yield return new WaitForSeconds (2.0f);
			Introfingers [i].SetActive(true);

			yield return unPause (34, 3.4f);
			yield return pause (34, 0.0f);
			//voz que dice que presione con el dedo correcto

			yield return speak (35,4.0f);
			borders[introSequence[i]].SetActive(true);
			
			while(!canContinue){
				yield return null;
			}
			canContinue = false;

			/*yield return new WaitForSeconds (2.0f);
			if (i == 0) {
				yield return speak (39,2.0f);
				yield return new WaitForSeconds (2.0f);
			}*/

			// bien

			yield return new WaitForSeconds (2.0f);
			Introfingers [i].SetActive(false);
			borders [introSequence [i]].SetActive (false);
		}

		
		yield return new WaitForSeconds (5.0f);

		octopus.SetActive (true);
		happy.SetActive (true);
		confused.SetActive (false);
		octoPlays [3].SetActive (false);
		goBack ();

	}
	IEnumerator PlaySequence(float time, bool flag = false, List<GameObject> lista = null, List<GameObject> handPos = null){
		vbMaster.GetComponent<VBMaster> ().UnregisterButtons ();
		countSeq = 0;

		for (int i = 0; i < sequence.Count; i++) {
			MasterColor (sequence [i], true);
			borders [sequence [i]].SetActive (true);
		fingertipsPlays [section_index][i].SetActive(true);
			//upborders [sequence [i]].SetActive (true);
			if(lista != null) lista [countSeq].SetActive (true);

			if(!flag)	keys [sequence [i]].GetComponent<AudioSource> ().Play ();
			else subNotasHabladas [sequence [i]].GetComponent<AudioSource> ().Play ();
			yield return new WaitForSeconds (time);

			if(lista != null) lista [countSeq].SetActive (false);
			countSeq++;
			MasterColor (sequence [i], false);
			borders [sequence [i]].SetActive (false);
		fingertipsPlays [section_index][i].SetActive(false);
			//upborders [sequence [i]].SetActive (false);
			yield return new WaitForSeconds (time);
		}

		vbMaster.GetComponent<VBMaster> ().RegisterButtons ();
	}

	IEnumerator PlaySequenceChords(bool flag = false, List<GameObject> lista = null, List<GameObject> handPos = null){
		vbMaster.GetComponent<VBMaster> ().UnregisterButtons ();

		for (int i = 0; i < sequence.Count; i++) {
			keys [sequence [i]].GetComponent<AudioSource> ().Play ();
			borders [sequence [i]].SetActive (true);
			//upborders [sequence [i]].SetActive (true);
		}

		yield return new WaitForSeconds (1.5f);
		
		for (int i = 0; i < sequence.Count; i++) {
			borders [sequence [i]].SetActive (false);
			//upborders [sequence [i]].SetActive (false);
		}
			

		vbMaster.GetComponent<VBMaster> ().RegisterButtons ();
	}
		
	IEnumerator humanPlay(){
		num_seq = 0;
		while (num_seq != sequence.Count) {
			/*if (num_seq == -1)
				goto finale;*/
			if (incorrectKey) {
			if (menuActual == 13 || menuActual == 12) {
				} else {
					yield return octoTurnWrong (section_index, section_cantar);

					octopus.SetActive (false);
					happy.SetActive (false);
					confused.SetActive (false);

					octoPlays [section_index].SetActive (true);	

					if(menuActual == 10 || menuActual == 11){
						if (section_index == 0 || section_index == 1 || section_index == 2)
							yield return speak (40,2.5f); //Posiciona mano derecha
						if (section_index == 3 || section_index == 4)
							yield return speak (41,2.5f);

					}
					

					yield return speak(47,5.5f); //Mas rosado.
					yield return waitForPosition (section_index);

					/*if(menuActual == 10 || menuActual == 11)	yield return change (true);
					else yield return change (false);*/

					yield return speak (19, 3.0f); //Intenta llenar la mano
					yield return speak(51, 2.5f); //Posiciona tus dedos como mis tentaculos
					while(!canContinue){
						yield return null;
					}
					canContinue = false;

					octoPlays [section_index].SetActive (false);

					octopus.SetActive (true);
					happy.SetActive (false);
					confused.SetActive (true);


				}
				
				incorrectKey = false;
			}
			yield return null;
		}
		GetComponent<AudioSource> ().Play ();
		vbMaster.GetComponent<VBMaster> ().userTurn = false;
		vbMasterRight.GetComponent<VBMasterRight> ().userTurn = false;
	}

	IEnumerator Play(){
		for (int i = 0; i < 10; i++) {
			yield return new WaitForSeconds (3.5f);
			num_seq = 0;
			//rnd_num = Random.Range (0, NUM_KEYS);
			//sequence.Add (rnd_num);
			//yield return PlaySequence();
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
		//sequence.Add (Random.Range (0, NUM_KEYS));
		start = false;
	}

	IEnumerator Repeat(){
		num_seq = 0;
		ini = true;
		yield return new WaitForSeconds (1.0f);
		while (num_seq < 1) {
			yield return PlaySequence (1.5f);
			yield return new WaitForSeconds (2.0f);
		}
		GetComponent<AudioSource> ().Play ();
		flag = true;
		ini = false;
	}

	public void loadChords(int chord){
		allSequence = new List<List<int>>{new List<int>{0,5}, new List<int>{6,11}, new List<int>{0,2,4},
			new List<int>{4,6,9}};
	}

	public void loadMusic(int music){
		sequence.Clear ();
		//masterRight.GetComponent<GameRight> ().loadMusic (music);
		if (music == 0) {
			allSequence = new List<List<int>>{new List<int>{11,9,7,9}, new List<int>{11,11,11}, new List<int>{9,9,11,9,7},
				new List<int>{11,9,7,9}, new List<int>{11,11,11}, new List<int>{9,9,9}, new List<int>{11,11,11},
				new List<int>{11,9,7,9}, new List<int>{11,11,11}, new List<int>{9,9,11,9,7}};

			line = new List<GameObject>{ };
			fingertipsPlays = new List<List<GameObject> >{ };
			fingertipsPlay = GameObject.Find ("ImageTarget/FingertipsMusic0");
			foreach (Transform child in fingertipsPlay.GetComponentInChildren<Transform> ()) {
				line = new List<GameObject>{};
				foreach (Transform subchild in child){
					line.Add (subchild.gameObject);
				}
				fingertipsPlays.Add (line);
		}
		} else if (music == 1) {
			sequence = new List<int>{2,7,7,7,2,4,4,2,11,11,9,9,7,2,7,7,7,2,4,4,2,11,11,9,9,7,2,2,7,7,7,2,2,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,2,4,4,2,11,11,9,9,7};
		} else {
		}
	}

	public void loadTutorial(){
		sequence.Clear();
		masterRight.GetComponent<GameRight> ().loadTutorial ();
		//sequence = new List<int>{2,0,4,9,7,5,11};
		sequence = new List<int>{0,1,2,3,4,5,6,7,8,9,10,11};
	}

	public void blockKeys(int[] indexes){
		for (int i = 0; i < indexes.Length; i++) {
			ColorGray (indexes[i], true);
		}
	}

	public void unblockKeys(int[] indexes){
		for (int i = 0; i < indexes.Length; i++) {
			ColorGray (indexes [i], false);
		}
	}

	public void Choose(int opcion){
		menu = menu * 10 + opcion;
	}

	public void chooseMenu(){
		menu = menu * 10 + actualMenu;
	}

	public IEnumerator nextKeyCR(int index){
		menuPics [index].SetActive (false);
		menuPics [index + 1].SetActive (true);
		yield return new WaitForSeconds (2.0f);
		menuPics [index + 1].SetActive (false);
		menuPics [index + 2].SetActive (true);
	}
		
	public void nextKey(int index){
		StartCoroutine (nextKeyCR (index));
	}	

	public IEnumerator speak(int index, float time){
		//vbMaster.GetComponent<VBMaster> ().UnregisterButtons ();
		//sign.SetActive (false);
		
		instrucciones [index].GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (time);
		
		//sign.SetActive (true);
		//vbMaster.GetComponent<VBMaster> ().RegisterButtons ();
	}
	
	public IEnumerator pause(int index, float time){
		//vbMaster.GetComponent<VBMaster> ().UnregisterButtons ();
		//sign.SetActive (false);
		
		instrucciones [index].GetComponent<AudioSource> ().Pause ();
		yield return new WaitForSeconds (time);
		
		//sign.SetActive (true);
		//vbMaster.GetComponent<VBMaster> ().RegisterButtons ();
	}

	public IEnumerator unPause(int index, float time){
		//vbMaster.GetComponent<VBMaster> ().UnregisterButtons ();
		//sign.SetActive (false);

		instrucciones [index].GetComponent<AudioSource> ().UnPause ();
		yield return new WaitForSeconds (time);

		//sign.SetActive (true);
		//vbMaster.GetComponent<VBMaster> ().RegisterButtons ();
	}

	public IEnumerator octoTurnWrong(int index, bool cantar = false){
		vbMaster.GetComponent<VBMaster> ().userTurn = false;
	vbMasterRight.GetComponent<VBMasterRight> ().userTurn = false;
		sign.SetActive (false);
		octopus.SetActive (false);
		happy.SetActive (false);
		confused.SetActive (false);
		octoPlays [index].SetActive (true);

		sequence = new List<int>{allSequence [index][num_seq]};
		yield return new WaitForSeconds (1.0f);

		yield return PlaySequence (1.5f, cantar);//, fingertipsPlays[index]);

		octoPlays [index].SetActive (false);
		octopus.SetActive (true);
		sign.SetActive (true);

		//happy.SetActive (true);
		//confused.SetActive (true);

		sequence = allSequence [index];

		borders [sequence [num_seq]].SetActive (true);
		//upborders [sequence [num_seq]].SetActive (true);

		vbMaster.GetComponent<VBMaster> ().userTurn = true;
	vbMasterRight.GetComponent<VBMasterRight> ().userTurn = true;
		//incorrectKey = false;
	}

	public IEnumerator octoTurnCR(float time, int index, bool cantar = false){

		vbMaster.GetComponent<VBMaster> ().userTurn = false;
	vbMasterRight.GetComponent<VBMasterRight> ().userTurn = false;
		sign.SetActive (false);
		octopus.SetActive (false);
		happy.SetActive (false);
		confused.SetActive (false);
		octoPlays [index].SetActive (true);

		sequence = allSequence [index];
		section_index = index;
		yield return new WaitForSeconds (1.0f);

		yield return PlaySequence (time, cantar);//, fingertipsPlays[index]);

		octoPlays [index].SetActive (false);
		octopus.SetActive (true);
		sign.SetActive (true);

		happy.SetActive (true);
		confused.SetActive (false);

		vbMaster.GetComponent<VBMaster> ().userTurn = true;
	vbMasterRight.GetComponent<VBMasterRight> ().userTurn = true;

		num_seq = 0;
		//upborders [sequence [num_seq]].SetActive (true);
		borders [sequence [num_seq]].SetActive (true);
		signs [sequence [num_seq]].SetActive (true);

	}

	public void octoTurn(int index){
		StartCoroutine (octoTurnCR (1.5f, index));
	}

	public IEnumerator octoMusic(int begin, int end){
		if (end >= allSequence.Count)
			end = allSequence.Count - 1;
		for (int i = begin; i < end; i++) {
			yield return octoTurnCR (0.5f, i);
			//upborders [sequence [num_seq]].SetActive (false);
			borders [sequence [num_seq]].SetActive (false);
			signs [sequence [num_seq]].SetActive (false);
		}
		
	}

	public void loadFingers(GameObject father){

		octoPlays.Clear ();
		foreach (Transform child in father.transform) {
			octoPlays.Add (child.gameObject);
		}
	}

	public IEnumerator Music(int index){
		loadMusic (index);
		musicFingers = GameObject.Find ("ImageTarget/Music" + index.ToString());
		loadFingers (musicFingers);

		rightAll = GameObject.Find ("ImageTarget/HandsMusic");
		rightAlls.Clear();
		foreach (Transform child in rightAll.transform) {
			rightAlls.Add (child.gameObject);
		}

		int tam = allSequence.Count / 3;
		for (int i = last_i; i < allSequence.Count; i += tam) {
			last_i = i;
			yield return octoMusic (i, i+tam);
			//sign.SetActive (false);
			//borders [sequence [num_seq]].SetActive (false);
			yield return new WaitForSeconds (2.0f);

			//borders [sequence [num_seq]].SetActive (true);
			sign.SetActive (true);

			for (int j = i; j < i + tam; j++) {
				yield return speak (3, 3.2f);
				yield return octoTurnCR (1.5f, j);

				//upborders [sequence [num_seq]].SetActive (false);
				borders [sequence [num_seq]].SetActive (false);
				signs [sequence [num_seq]].SetActive (false);
				vbMaster.GetComponent<VBMaster> ().userTurn = false;
			vbMasterRight.GetComponent<VBMasterRight> ().userTurn = false;

				yield return speak (2, 4.2f);

				octopus.SetActive (false);
				happy.SetActive (false);
				confused.SetActive (false);

				yield return waitForPosition (j);
				yield return change (true);

				octoPlays [j].SetActive (false);

				octopus.SetActive (true);
				happy.SetActive (true);
				confused.SetActive (false);


				//leftAll = GameObject.Find ("ImageTarget/HandsPlaysLeft);

				/*rightAlls [j].SetActive (true);
				yield return new WaitForSeconds (3.0f);
				rightAlls [j].SetActive (false);*/

				//upborders [sequence [num_seq]].SetActive (true);
				borders [sequence [num_seq]].SetActive (true);
				signs [sequence [num_seq]].SetActive (true);
				vbMaster.GetComponent<VBMaster> ().userTurn = true;
			vbMasterRight.GetComponent<VBMasterRight> ().userTurn = true;	

				yield return humanPlay ();
			}
		}
	last_i = 0;
	}

	public IEnumerator Music2(int index){
		loadMusic (index);
		musicFingers = GameObject.Find ("ImageTarget/Music" + index.ToString());
		loadFingers (musicFingers);

		rightAll = GameObject.Find ("ImageTarget/HandsMusic");
		rightAlls.Clear();
		foreach (Transform child in rightAll.transform) {
			rightAlls.Add (child.gameObject);
		}


		for (int i = last_i; i < allSequence.Count - 1; i += 2) {
			last_i = i;
			yield return octoTurnCR (1.0f, i);

			//upborders [sequence [num_seq]].SetActive (false);
			borders [sequence [num_seq]].SetActive (false);
			signs [sequence [num_seq]].SetActive (false);
			//sign.SetActive (false);
			//borders [sequence [num_seq]].SetActive (false);
			yield return new WaitForSeconds (2.0f);

			//borders [sequence [num_seq]].SetActive (true);
			sequence = allSequence [i + 1];


			yield return change ();



			sign.SetActive (true);
			//upborders [sequence [num_seq]].SetActive (true);
			borders [sequence [num_seq]].SetActive (true);
			signs [sequence [num_seq]].SetActive (true);

			yield return humanPlay ();
		}
		last_i = 0;
	}

	IEnumerator FinalCR(int index){

		yield return Introduction ();

		yield return speak (0, 6.2f);
		for (int i = 0; i < allSequence.Count; i++) {
			yield return speak (3, 3.2f);

			yield return octoTurnCR (1.5f, i, true);
			section_cantar = true;


			//upborders [sequence [num_seq]].SetActive (false);
			borders [sequence [num_seq]].SetActive (false);
			signs [sequence [num_seq]].SetActive (false);
			vbMaster.GetComponent<VBMaster> ().userTurn = false;
		vbMasterRight.GetComponent<VBMasterRight> ().userTurn = false;

			yield return speak (2, 4.2f);

			/*rightAlls [i].SetActive (true);
			yield return new WaitForSeconds (3.0f);
			rightAlls [i].SetActive (false);*/

			//upborders [sequence [num_seq]].SetActive (true);
			borders [sequence [num_seq]].SetActive (true);
			signs [sequence [num_seq]].SetActive (true);

			vbMaster.GetComponent<VBMaster> ().userTurn = true;
		vbMasterRight.GetComponent<VBMasterRight> ().userTurn = true;
			vbMaster.GetComponent<VBMaster> ().habla = true;
			yield return humanPlay ();
		}

		section_cantar = false;
		vbMaster.GetComponent<VBMaster> ().habla = false;

		yield return speak (4, 2.2f);

		yield return Music (0);

		yield return speak (4, 2.2f);

		yield return Music2 (0);


		yield return speak (5, 5.0f);
	}

	public void Final(int index){
		//StartCoroutine (FinalCR (index));
	}


	public void Choose(){
		if (menuActual == 1) {
			if (posActual == 0)
				menu = 10;
			else if (posActual == 1)
				menu = 11;
			else if (posActual == 2)
				menu = 12;
			else if (posActual == 3) {
				loadTutorial();
				menu = 13;
			}
		} else if (menuActual == 11) {
			if (posActual == 0) {
				menu = 110;
				loadMusic (0);
			} else if (posActual == 1) {
				menu = 111;
				loadMusic (1);
			}
		} else if (menuActual == 10) {
			
		} else if (menuActual % 10 == 9) {
			if (posActual == 0)
				menu = menu * 10 + 9;
			else if (posActual == 1) {
				//Poner codigo de repeticion,
				goBack();
			}
			else if (posActual == 2)
				menu = 1; 
		}
	}

	public void leftArrow(){
		Vector3 A = menuPics[posActual].transform.position - finger.transform.position;
		Vector3 B = menuPics[posActual - 1].transform.position - finger.transform.position;
		float angle = Vector3.Angle (A, B);
		posActual--;
		finger.transform.Rotate (0, 0, angle);
	}

	public void rightArrow(){
		Vector3 A = menuPics[posActual].transform.position - finger.transform.position;
		Vector3 B = menuPics [posActual + 1].transform.position - finger.transform.position;
		float angle = -1*Vector3.Angle (A, B);
		//finger.transform.Rotate (0, 0, angle);
		posActual++;
		finger.transform.Rotate(0,0,angle);
	}

	public void middleArrow(){
		Vector3 A = sphere.transform.position - finger.transform.position;
		Vector3 B = menuPics [0].transform.position - finger.transform.position;
		float angle = Vector3.Angle (A, B);
		//finger.transform.Rotate (0, 0, angle);
		finger.transform.Rotate(0,0,angle);
	}

	public void goBack(){
		if (menu % 10 == 9)
			flag_seq = true;
		menu = menu / 10;
		if (menu <= 0)
			menu = 1;
	}

	public void goToOptions(){
		menu = menu * 10 + 9;
	}

	public void loadMenu(){

		if (menuOp != null) {
			foreach (Transform child in menuOp.transform) {
				child.gameObject.SetActive (false);
			}
		}
		//soundKey (1);
		if(menuActual % 100 == 99) menuOp = GameObject.Find("ImageTarget/Voz");
		else if(menuActual % 10 == 9) menuOp = GameObject.Find ("ImageTarget/Opciones"); 
		else menuOp = GameObject.Find ("ImageTarget/Menu" + menuActual.ToString());

		if (menuActual == 1 || menuActual == 11) {
			blockKeys (new int[]{ 0, 1, 3, 4, 6, 7, 8, 10, 11 });
		} else {
			unblockKeys (new int[]{ 0, 1, 3, 4, 6, 7, 8, 10, 11 });
		}
		//soundKey (3);
		menuPics.Clear ();
		soundKey (1);
		foreach (Transform child in menuOp.transform) {
			GetComponent<AudioSource> ().Play ();
			child.gameObject.SetActive (true);
			menuPics.Add (child.gameObject);
		}

		if (menuActual != 110 && menuActual != 10 && menuActual != 12 && menuActual % 100 != 99 && menuActual != 13 && menuActual != 111) {
			/*foreach (Transform child in menuGl.transform) {
				child.gameObject.SetActive (true);
			}
			posActual = (menuPics.Count - 1) / 2;
			finger.transform.eulerAngles = new Vector3 (finger.transform.eulerAngles.x, 0.0f, 0.0f);*/
		} else {
			if (!flag_seq) {
				masterRight.GetComponent<GameRight> ().num_seq = 0;
				num_seq = 0;
			}
			flag_seq = false;
			if (menuActual == 13) {
				loadTutorial ();
				for(int i = 1; i < menuPics.Count; i++) {
					menuPics [i].SetActive (false);
				}
			}
			foreach (Transform child in menuGl.transform) {
				child.gameObject.SetActive (false);
			}
		}

		//middleArrow ();

	}

	public void activateMenuSig(){
		deactivateMenuOne(actualMenu);
		activateMenuOne(mod(actualMenu+1,4));
		actualMenu = mod(actualMenu+1,4);
	}

	public void activateMenuAnt(){
		deactivateMenuOne(actualMenu);
		activateMenuOne(mod(actualMenu-1,4));
		actualMenu = mod(actualMenu-1,4);
	}



	public void loadMenu2(){

		if (menuActual == 1) { //Menu Principal
		exit.SetActive(true);
			back.SetActive (false);
			exit.SetActive (true);
			last_i = 0;
			activateHandSlaps();
			activateMenuOne(actualMenu);
			//activateMenu ();
			sign.SetActive (true);
			deactivateHands ();
			borders [sequence [num_seq]].SetActive (false);
			signs [sequence [num_seq]].SetActive (false);
			octopus.SetActive (true);
			happy.SetActive (true);
			confused.SetActive (false);
			octoPlays [section_index].SetActive (false);
			section_cantar = false;
			vbMaster.GetComponent<VBMaster> ().habla = false;
			vbMaster.GetComponent<VBMaster> ().userTurn = false;
		vbMasterRight.GetComponent<VBMasterRight> ().userTurn = false;
			//instrucciones[44].GetComponent<AudioSource> ().Play ();
			instrucciones[27].GetComponent<AudioSource> ().Play ();

			//sign.SetActive (false);
			
		} else {
			deactivateMenu ();
			back.SetActive (true);
			exit.SetActive (false);
			deactivateHandSlaps();
			octopus.SetActive (true);
			happy.SetActive (true);
			confused.SetActive (false);
			sign.SetActive (true);
			deactivateHands ();
			if (myThread != null)
				StopCoroutine (myThread);
			
			if (menuActual == 10) {
				myThread = firstSection ();
				StartCoroutine (myThread);
			} else if (menuActual == 11) {
				myThread = secondSection ();
				StartCoroutine (myThread);
			} else if (menuActual == 12) {
				myThread = thirdSection ();
				StartCoroutine (myThread);
			} else if (menuActual == 13) {
				myThread = fourthSection ();
				StartCoroutine (myThread);
			} else if (menuActual % 10 == 9) {
				myThread = Introduction ();
				StartCoroutine (myThread);
			}
		}
		
		/*} else {
			if (!flag_seq) {
				masterRight.GetComponent<GameRight> ().num_seq = 0;
				num_seq = 0;
			}
			flag_seq = false;
			if (menuActual == 13) {
				loadTutorial ();
				for(int i = 1; i < menuPics.Count; i++) {
					menuPics [i].SetActive (false);
				}
			}
			foreach (Transform child in menuGl.transform) {
				child.gameObject.SetActive (false);
			}
		}*/

		//middleArrow ();

	}
	// Update is called once per frame
	void Update () {
		if (menu != menuActual) {
			menuActual = menu;
			vbMaster.GetComponent<VBMaster> ().menu = menu;
			vbMasterRight.GetComponent<VBMasterRight> ().menu = menu;
			loadMenu2 ();
		}
		if(active_piano)
		{
			//GameObject.Find("ImageTarget/Master").GetComponent<AudioSource>().Play();
			//finger2.GetComponent<AudioSource>().Play();
			//curvado.SetActive(false);
			is_piano = false;
			is_estirado = false;
			active_piano = false;
		}
		else if(is_1)
		{
			//GameObject.Find("ImageTarget/Instrucciones/Saludo").GetComponent<AudioSource>().Play();
			//IncorrectPos.GetComponent<AudioSource>().Play();
			//curvado.SetActive(true);
			is_1 = false;
			cont_1 = 0;
			cont_estirado = 0;
			cont_piano = 0;
			is_piano = true;
		}
		else if(is_estirado)
		{
			is_estirado = false;
			//curvado.SetActive(false);
			//IncorrectPos.GetComponent<AudioSource>().Play();
			//GameObject.Find("ImageTarget/VirtualButtonDo/Do").GetComponent<AudioSource>().Play();
			cont_estirado = 0;
			cont_1 = 0;
			cont_piano = 0;
			is_piano = true;
		}

		
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
