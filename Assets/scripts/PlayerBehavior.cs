using UnityEngine;
using System.Collections;

public enum Direction {left, right, still};

public class PlayerBehavior : MonoBehaviour {

	public int[] currentPos;
	public int movesToPerform = 3;
	public int movesPerformed = 0;
	public float delayBetweenMoves = 0.3f;

	private BoardMan boardMan;
	private GameObject[,] grid;
	private Direction[] moves;
	private bool playerBlocked = false;

	// Use this for initialization
	void Start () {

		boardMan = FindObjectOfType<BoardMan>();
		grid = boardMan.grid;

		moves = new Direction[movesToPerform];
	}
	
	// Update is called once per frame
	void Update () {

//		if (Input.GetKeyDown("space"))
//			 MovePLayer(boardMan.PLayerMove(currentPos, Direction.still));
//		else if(Input.GetKeyDown("left"))
//			MovePLayer(boardMan.PLayerMove(currentPos, Direction.left));
//		else if(Input.GetKeyDown("right"))
//			MovePLayer(boardMan.PLayerMove(currentPos, Direction.right));

		if(!playerBlocked)
		{
			if (Input.GetKeyDown("space"))
				CollectMoves(Direction.still);
			else if(Input.GetKeyDown("left"))
				CollectMoves(Direction.left);
			else if(Input.GetKeyDown("right"))
				CollectMoves(Direction.right);
		}
	}

	void MovePlayer(int[] moveTo)
	{
		currentPos = moveTo;
		transform.position = grid[moveTo[0], moveTo[1]].transform.position;

		Debug.Log(currentPos[0]);
	}

	void CollectMoves(Direction dir)
	{
		moves[movesPerformed] = dir;
		movesPerformed++;

		if (movesPerformed >= movesToPerform)
		{
			playerBlocked = true;
			StartCoroutine("DelayedMovement");
		} 		
	}

	IEnumerator DelayedMovement()
	{		
			for(int x=0; x<=moves.Length-1; x++)
			{
				yield return new WaitForSeconds(delayBetweenMoves);
				MovePlayer(boardMan.PlayerMove(currentPos, moves[x]));
			}

			StopCoroutine("DelayedMovement");
			movesPerformed = 0;
			moves = new Direction[movesToPerform];
			playerBlocked = false;
	}
}
