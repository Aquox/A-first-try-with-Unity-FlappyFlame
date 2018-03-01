using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GameControl : MonoBehaviour {

	public static GameControl instance;
	public GameObject gameOverText;
	public Text scoreText;
	public Text bestScoreText;
	public Text lastBestScoreText;
	public bool gameOver = false;
	public float scrollSpeed = -1.5f;
	public static bool hascomefrom = false;
	public static bool firsttime = false;
	public Slider slider;
	public AudioSource playSong;
	public AudioSource gameoverSong;

	private string [] files = {"/volume.dat","/type.dat","/score.dat","/firstplay.dat","/firstoption.dat"};

	private int score = 0;

	//controllo di avere un'unica istanza della classe
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}
	

	void Update () {
		//se clicco mentre sono morto allora faccio ripartire la scena altrimenti se clicco sul pulsante opzioni allora faccio ripartire la scena che però sarà messa in pausa e con il menu visualizzato
		Rect bounds = new Rect (0, Screen.height -(Screen.width*0.09f), Screen.width*0.09f, Screen.height);
		//Pause.pause = false;
		if (gameOver == true && Input.GetMouseButtonDown (0) && !bounds.Contains(Input.mousePosition)) {
			Debug.Log ("chiamo scena 1");
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}else if (gameOver == true && Input.GetMouseButtonDown (0)){
			Debug.Log ("chiamo scena 2");
			hascomefrom = false;
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	//aumento e visualizzo lo score a schermo
	public void FireballScored(){
		if (gameOver) {
			return;
		}
		score++;
		scoreText.text = "SCORE: " + score.ToString ();
	}

	//se si muore allora carico una scritta e se lo score è migliore del best score lo salvo su file
	public void FireballDied(){		
		Debug.Log ("metodo");
		playSong.Stop ();
		gameoverSong.Play ();
		lastBestScoreText.text = "LAST BEST SCORE: " + (int)Load(2);
		gameOverText.SetActive (true);
		gameOver = true;
		hascomefrom = true;
		if ((int)Load (2) < score) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + files[2]);
			bf.Serialize (file, (float)score);
			file.Close ();
		}
	}

	//salvo su file il valore del volume alla chiamata
	public void setVolume(float value){
		AudioListener.volume = value;
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + files[0]);
		bf.Serialize(file, value);
		file.Close();
	}

	//carico il valore del volume da file e setto lo slider presente nel menu
	public void setSlider(){
		float value = Load (0);
		slider.value = value;
	}

	//aprendo il menu best score carico il risultato da file e lo visualizzo
	public void setScore(){
		int value =(int) Load (2);
		bestScoreText.text = "BEST SCORE: " + value.ToString ();
	}

	//metodo per caricare da file un dato
	public float Load(int typefile){
		if (File.Exists (Application.persistentDataPath + files[typefile])) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open(Application.persistentDataPath + files[typefile], FileMode.Open);
			float value = (float)bf.Deserialize (file);
			file.Close ();
			return value;
		} else {
			if (typefile == 0) {
				return 1f;
			} else {
				return 0f;
			}
		}
	}
}
