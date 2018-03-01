using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalagmitesPool : MonoBehaviour {

	public int stalagmitePoolSize = 5;
	public GameObject stalagmitePrefab;
	public float spawnRate = 10f;

	private GameObject[] stalagmites;
	private Vector2 objectPoolPosition = new Vector2 (-15f, -25f);
	private float timeSinceLastSpawned;
	private float spawnXPosition = 4f;
	private float [] spawnYposition = new float[7] {0f,0.4f,0.8f,1.2f,1.6f,2.0f,2.4f};
	private int currentStalagmite = 0;
	private bool first = true;

	//creo 5 stalagmiti
	void Start () {
		stalagmites = new GameObject[stalagmitePoolSize];
		for (int i = 0; i < stalagmitePoolSize; i++) {
			stalagmites [i] = (GameObject)Instantiate (stalagmitePrefab, objectPoolPosition, Quaternion.identity);
		}
	}
	
	//posiziono la stalagmite corrente dopo un deltaTime che supera spawnRate nella posizione spawnXposition ad altezza spawnYposition
	void Update () {
		timeSinceLastSpawned = timeSinceLastSpawned + Time.deltaTime;
		if (first) {
			timeSinceLastSpawned = 20f;
			first = false;
		}
		if (GameControl.instance.gameOver == false && timeSinceLastSpawned >= spawnRate) {
			timeSinceLastSpawned = 0;
			stalagmites [currentStalagmite].transform.position = new Vector2 (spawnXPosition, spawnYposition[Random.Range (0,7)]);
			currentStalagmite++;
			if (currentStalagmite >= stalagmitePoolSize) {
				currentStalagmite = 0;
			}
		}
	}
}
