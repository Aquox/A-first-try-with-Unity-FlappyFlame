using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour {

	public Text bestScoreText;
	public Slider slider;
	private string [] files = {"/volume.dat","/type.dat","/score.dat","/firstplay.dat","/firstoption.dat"};

	//carico alcuni valori da file il prima possibile per non rallentare transizioni tra le due scene e averli pronti per i menu o le azioni da intraprendere
	void Start () {
		float value = 1f;
		value = Load (0);
		AudioListener.volume = value;
		if ((int)Load (3) == 0) {
			GameControl.firsttime = true;
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(Application.persistentDataPath + files[3]);
			bf.Serialize(file, 1f);
			file.Close();
		}
		GameControl.hascomefrom = true;
	}

	//se schiaccio play attivo la scena del gioco
	public void play(){		
		SceneManager.LoadScene (1);			
	}

	//se vado sul volume carico lo slider
	public void setSlider(){
		float value = Load (0);
		slider.value = value;
	}

	//se vado sul best score carico il best score
	public void setScore(){
		int value =(int) Load (2);
		bestScoreText.text = "BEST SCORE: " + value.ToString ();
	}

	//se cambio volume lo salvo su file
	public void setVolume(float value){
		AudioListener.volume = value;
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + files[0]);
		bf.Serialize(file, value);
		file.Close();
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

	//se cambio costume della pallina allora lo salvo su file
	public void changeSprite(int type){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + files[1]);
		bf.Serialize(file,(float) type);
		file.Close();
	}

}
