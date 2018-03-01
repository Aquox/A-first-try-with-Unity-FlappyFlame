using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalagmites : MonoBehaviour {

	//se entro nel box invisibile dopo le stalagmiti chiamo il metodo per aumentare lo score
	private void OnTriggerEnter2D(Collider2D box){
		if (box.GetComponent <Fireball> () != null) {
			GameControl.instance.FireballScored ();
		}
	}

}
