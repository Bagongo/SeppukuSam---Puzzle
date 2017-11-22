using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public BoardMan boardMan;
	public SpriteRenderer sprtRend;

	void Awake()
	{
		boardMan = FindObjectOfType<BoardMan>();
	}
		
	void OnMouseDown() {

			Debug.Log(transform.position.x + " " + transform.position.y);

	}
}
