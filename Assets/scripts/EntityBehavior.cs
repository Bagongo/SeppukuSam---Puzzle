using UnityEngine;
using System.Collections;

public class EntityBehavior : MonoBehaviour {

	public int[] currentPos;
	public int[] moveDirection;
	public int honorValue;
	public int scoreValue;
	public int nmbrOfMoves;
	public bool hasKnife;
	public bool canPickUp;
	public bool canDrop;

	public BoardMan boardMan;
	public GameObject[,] grid;
	public GameObject[,] items;
	public ScoreMan scoreMan;


	protected virtual void Awake()
	{
		boardMan = (BoardMan)FindObjectOfType(typeof(BoardMan));
		grid = boardMan.grid;
		items = boardMan.items;

		scoreMan = (ScoreMan)FindObjectOfType(typeof(ScoreMan));			
	}

	public void RequestMove()
	{
		for (int x=0; x < nmbrOfMoves; x++)
			boardMan.ElaborateMove(currentPos, moveDirection);
	}

	public void MoveEntity(int[] newPos)
	{
		currentPos = newPos;
		transform.position = grid[currentPos[0], currentPos[1]].transform.position;	
		LookForKnife();															
	}

	public void LookForKnife()
	{
		if(canPickUp && !hasKnife && items[currentPos[0], currentPos[1]] != null)
		{
			hasKnife = true;
			boardMan.PickUpKnife(currentPos);
		}
	}

}
