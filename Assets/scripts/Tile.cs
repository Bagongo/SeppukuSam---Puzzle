using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public BoardMan boardMan;

	void Start () {

		boardMan = FindObjectOfType<BoardMan>();
	}
	
	void OnMouseDown () {

			Debug.Log(transform.position.x + " " + transform.position.y);

	}
}
