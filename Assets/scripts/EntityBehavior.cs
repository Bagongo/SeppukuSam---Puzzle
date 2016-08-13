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
	public TurnMan turnMan;
	public ScoreMan scoreMan;
	public int gridW;
	public int gridH;

	protected virtual void Awake()
	{
		boardMan = (BoardMan)FindObjectOfType(typeof(BoardMan));

		grid = boardMan.grid;
		gridW = boardMan.gridW;
		gridH = boardMan.gridH;

		items = boardMan.items;

		turnMan = (TurnMan)FindObjectOfType(typeof(TurnMan));
		scoreMan = (ScoreMan)FindObjectOfType(typeof(ScoreMan));			
	}

	public void RequestMove()
	{
		for (int x=0; x < nmbrOfMoves; x++)
			ElaborateMove(moveDirection);
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

	bool isInBounds(int[] requestedPos)
	{
		if(requestedPos[0] >= 0 && requestedPos[0] < gridW && requestedPos[1] >= 0)
			return true;
		else 
			return false;
	}

	public int[] EvaluateMovement(int[] dir)
	{
		int[] requestedPos = new int[]{currentPos[0]+dir[0], currentPos[1]+dir[1]};
		int[] newPos;

		if(isInBounds(requestedPos)) //in bounds xy....
		{
			if(boardMan.entities[requestedPos[0], requestedPos[1]] == null)
				newPos = new int[] {requestedPos[0], requestedPos[1]}; 
			else
			{
				int tryNeighborAt = Random.value <= .5 ? 1 : -1;

				if(isInBounds(new int[]{requestedPos[0] - tryNeighborAt, requestedPos[1]}) && boardMan.entities[requestedPos[0] - tryNeighborAt, requestedPos[1]] == null)
					newPos = new int[] {requestedPos[0] - tryNeighborAt, requestedPos[1]}; 
				else if(isInBounds(new int[]{requestedPos[0] + tryNeighborAt, requestedPos[1]}) && boardMan.entities[requestedPos[0] + tryNeighborAt, requestedPos[1]] == null)
					newPos = new int[] {requestedPos[0] + tryNeighborAt, requestedPos[1]}; 
				else
				{
					newPos = new int[] {currentPos[0], currentPos[1]};
				}	
			} 
		}
		else
		{
			if(moveDirection.Length > 0)
			{
				moveDirection[0] = - moveDirection[0];
				return EvaluateMovement(moveDirection);
			}

			newPos = new int[] {currentPos[0], currentPos[1]};	
		}

		return newPos;
	}

	public void ElaborateMove(int[] dir)
	{
		int[] newPos = EvaluateMovement(dir);

		boardMan.UpdateGrid(currentPos, newPos);

		MoveEntity(newPos);			
	}
}
