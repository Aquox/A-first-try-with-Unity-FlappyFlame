using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour {

	private Rigidbody2D background;

	//do velocità al background per farlo scorrere
	void Start () {
		background = GetComponent<Rigidbody2D> ();
		background.velocity = new Vector2 (GameControl.instance.scrollSpeed, 0);
	}
	
	//se si muore fermo il background
	void Update () {
		if (GameControl.instance.gameOver == true) {
			background.velocity = Vector2.zero;
		}
	}
}
