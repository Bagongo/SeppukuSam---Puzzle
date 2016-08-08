using UnityEngine;
using System.Collections;

public enum Direction {left, right, still};

public class PlayerBehavior : MonoBehaviour {

	public int[] currentPos;

	private BoardMan boardMan;
	private GameObject[,] grid;

	// Use this for initialization
	void Start () {

		boardMan = FindObjectOfType<BoardMan>();
		grid = boardMan.grid;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown("space"))
			 MovePLayer(boardMan.PLayerMove(currentPos, Direction.still));
		else if(Input.GetKeyDown("left"))
			MovePLayer(boardMan.PLayerMove(currentPos, Direction.left));
		else if(Input.GetKeyDown("right"))
			MovePLayer(boardMan.PLayerMove(currentPos, Direction.right));
	}

	void MovePLayer(int[] moveTo)
	{
		currentPos = moveTo;
		transform.position = grid[moveTo[0], moveTo[1]].transform.position;

		Debug.Log(currentPos[0]);
	}

}
