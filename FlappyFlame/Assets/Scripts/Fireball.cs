using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;


public class Fireball : MonoBehaviour{

    public float Force = 200f;
	public static GameObject menuPanelCanvas;
	public AudioSource menuSong;
	public AudioSource playSong;
	public AudioSource gameoverSong;

	public Sprite sprite1;
	public Sprite sprite2;
	public Sprite sprite3;
	public GameObject flame;

	private string [] files = {"/volume.dat","/type.dat","/score.dat","/firstplay.dat","/firstoption.dat"};

    private bool lose = false;
    private Rigidbody2D fireball;
	private Animator animation;

	float accelerometerUpdateInterval = 1.0f / 60.0f;
	float lowPassKernelWidthInSeconds = 1.0f;
	float shakeDetectionThreshold = 2.0f;
	float lowPassFilterFactor;
	Vector3 lowPassValue;

	public GameObject tutorial;
	public GameObject tutorial2;
	public GameObject button;

	void Start () {
		//inizializzo alcunve variabili e preparo l'ambiente per audio e comparsa/scomparsa del menu tipo di costume e preparazione allo shake
		float value = 1f;
		value = Load (0);
		AudioListener.volume = value;
		fireball = GetComponent<Rigidbody2D> ();
		animation = GetComponent<Animator> ();
		menuPanelCanvas = GameObject.Find ("MenuPanelCanvas");
		//se si preme il pulsante opzioni nel gameover metto in pausa altrimenti faccio partire il gioco e faccio scomparire il menu 
		if (!GameControl.hascomefrom) {
			Time.timeScale = 0;
			Pause.pause = true;
		} else {			
			Time.timeScale = 1;
			Pause.pause = false;
			menuPanelCanvas.gameObject.SetActive (false);
			menuSong.Stop ();
			gameoverSong.Stop ();
			playSong.Play ();
			//se è la prima volta che si gioca faccio comparire il tutorial
			if (GameControl.firsttime) {
				tutorial2.gameObject.SetActive (true);
				Time.timeScale = 0;
				button.gameObject.SetActive (false);
			}
		}
		changeSprite ((int)Load (1));
		lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
		shakeDetectionThreshold *= shakeDetectionThreshold;
		lowPassValue = Input.acceleration;
	}	

	void Update () {	
		//preparo ad ascoltare se avviene uno shake
		Vector3 acceleration = Input.acceleration;
		lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
		Vector3 deltaAcceleration = acceleration - lowPassValue;

		//se non si è già in pausa e avviene uno shake metto in pausa e mostro il menu
		if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold && !Pause.pause)
		{
			Time.timeScale = 0;
			Pause.pause = true;
			menuPanelCanvas.gameObject.SetActive (true);
			playSong.Stop ();
			gameoverSong.Stop ();
			menuSong.Play ();
		}

		//se è la prima volta che si fa pausa dopo aver eseguito lo shake del tutorial attivo il menu e salvo su file il fatto che il tutorial è già stato fatto una volta
		if ((Input.GetMouseButtonDown(0) || deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold) && Pause.first && Pause.listen) {
			Pause.first = false;
			tutorial.gameObject.SetActive (false);
			menuPanelCanvas.gameObject.SetActive (true);
			button.gameObject.SetActive (true);
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + files[4]);
			bf.Serialize (file, 1f);
			file.Close ();
		}

		//se è la prima volta che si gioca e viene fatto un tap nel tutorial faccio sparire il tutorial e parte il gioco
		if (Input.GetMouseButtonDown (0) && GameControl.firsttime) {
			tutorial2.gameObject.SetActive (false);
			Time.timeScale = 1;
			GameControl.firsttime = false;
			button.gameObject.SetActive (true);
		}

		// se non si ha perso e il gioco non è in pausa allora si applica una forza alla pallina
		if (!lose && !Pause.pause) {		
			//controllo di non aver cliccato sul pulsante	
			Rect bounds = new Rect (0, Screen.height -(Screen.width*0.09f), Screen.width*0.09f, Screen.height);
			if (Input.GetMouseButtonDown(0) && !bounds.Contains(Input.mousePosition)) {
				fireball.velocity = Vector2.zero;
				fireball.AddForce (new Vector2 (0, Force));
				animation.SetTrigger ("Flame");
			}
        }	
	}

	//se la pallina si scontra con qualcosa si perde e si chiama una funzione per gestire la cosa
	void OnCollisionEnter2D (){
		Debug.Log ("collision");
		fireball.velocity = Vector2.zero;
		fireball.freezeRotation = true;
		lose = true;
		Debug.Log ("chiamo metodo");
		GameControl.instance.FireballDied();
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
			return 1f;
		}
	}

	//metodo per cambiare tipo di costume alla pallina
	public void changeSprite(int type){
		switch (type) {
			case 0:
				animation.SetInteger ("Type", 0);
				flame.GetComponent<SpriteRenderer> ().sprite = sprite1;
				break;
			case 1:
				animation.SetInteger ("Type", 1);
				flame.GetComponent<SpriteRenderer> ().sprite = sprite2;
				break;
			case 2:
				animation.SetInteger ("Type", 2);
				flame.GetComponent<SpriteRenderer> ().sprite = sprite3;
				break;
			default:
				break;
		}
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + files[1]);
		bf.Serialize(file,(float) type);
		file.Close();
	}
}
