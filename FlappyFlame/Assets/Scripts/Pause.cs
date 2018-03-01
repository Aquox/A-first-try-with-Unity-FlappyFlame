using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Pause : MonoBehaviour {

	public static bool pause;
	public AudioSource menuSong;
	public AudioSource playSong;
	public GameObject gameOverText;

	public GameObject tutorial;
	public GameObject button;


	private string [] files = {"/volume.dat","/type.dat","/score.dat","/firstplay.dat","/firstoption.dat"};
	public static bool first = false;
	public static bool listen = false;

	//se è la prima volta che si mette in pausa scrivo su file che è avvenuta la prima volta
	void Start () {
		if ((int)Load (4)==0) {
			first = true;
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + files[4]);
			bf.Serialize (file, 1f);
			file.Close ();
		}
	}

	public void onPause(){
		Debug.Log ("pausa");
		pause = !pause;
		//se sono in pausa faccio partire il gioco altrimenti metto in pausa
		if (!pause) {
			Time.timeScale = 1;
			Fireball.menuPanelCanvas.gameObject.SetActive (false);
			menuSong.Stop();
			playSong.Play();
		} else if (pause) {
			gameOverText.SetActive (false);
			Time.timeScale = 0;
			playSong.Stop();
			menuSong.Play();
			//se è la prima pausa allora faccio vedere il tutorial altrimenti faccio vedere il menu
			if (first) {
				tutorial.gameObject.SetActive (true);
				button.gameObject.SetActive (false);
				listen = true;
			} else {
				Fireball.menuPanelCanvas.gameObject.SetActive (true);
			}
		}
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
