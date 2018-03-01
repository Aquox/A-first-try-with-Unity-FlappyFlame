using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeatingbackground : MonoBehaviour {

	private BoxCollider2D groundCollider;
	private float groundHorizontalLenght;

	//calcolo quanto lungo è il background
	void Start () {
		groundCollider = GetComponent<BoxCollider2D> ();
		groundHorizontalLenght = groundCollider.size.x;
	}
	
	//controllo se sono arrivato a circa 3/4 del background
	void Update () {
		if (transform.position.x < -groundHorizontalLenght) {
			RepositionBackground ();
		}
	}

	//posiziono il background di sinistra a destra dell'altro background
	private void RepositionBackground(){
		Vector2 groundOffset = new Vector2 (groundHorizontalLenght * 2f, 0);
		transform.position = (Vector2)transform.position + groundOffset;
	}
}
