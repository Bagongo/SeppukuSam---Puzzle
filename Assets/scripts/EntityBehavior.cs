using UnityEngine;
using System.Collections;

public class EntityBehavior : MonoBehaviour {

	public int[] currentPos;
	public int[] nextPos;
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
	public Castle castle;
	public float moveTime = 1f;

	protected virtual void Awake()
	{
		boardMan = (BoardMan)FindObjectOfType(typeof(BoardMan));
		grid = boardMan.grid;
		gridW = boardMan.gridW;
		gridH = boardMan.gridH;
		items = boardMan.items;

		turnMan = (TurnMan)FindObjectOfType(typeof(TurnMan));
		scoreMan = (ScoreMan)FindObjectOfType(typeof(ScoreMan));
		castle = (Castle)FindObjectOfType(typeof(Castle));			
	}

	public void RequestMove()
	{
		for (int x=0; x < nmbrOfMoves; x++)
			ElaborateMove(moveDirection);
	}

	public void FinalizeMovement()
	{
		currentPos = nextPos;
		transform.position = grid[currentPos[0], currentPos[1]].transform.position;
		LookForKnife();	

		if(GetComponent<PlayerBehavior>())
		{
			boardMan.playerPos = currentPos;
			turnMan.OtherEntitiesMove();
			GetComponent<PlayerBehavior>().Attack();
		}														
	}

	//Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    protected IEnumerator SmoothMovement (Vector3 endPos)
    {
		//Vector3 endPos = grid[newPos[0], newPos[1]].transform.position;
		float sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;

		while(sqrRemainingDistance > float.Epsilon)
        {
			transform.position = Vector3.MoveTowards(transform.position, endPos, 2 * Time.deltaTime);
			sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;
			
            yield return null;
        }

        FinalizeMovement();
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
			if(boardMan.entities[requestedPos[0], requestedPos[1]] == null || boardMan.entities[requestedPos[0], requestedPos[1]].tag == "Player")
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
		nextPos = EvaluateMovement(dir);
		boardMan.UpdateGrid(currentPos, nextPos);				
		Vector3 newPos = grid[nextPos[0], nextPos[1]].transform.position;
		StartCoroutine(SmoothMovement(newPos));
	}

	public void GoToCastle()
	{
		boardMan.RemoveFromGrid(currentPos);
		Vector3 newPos = grid[nextPos[0], nextPos[1]].transform.position;
		StartCoroutine(SmoothMovement(newPos));
	}

	public void EliminateEntity()
	{
		Destroy(this.gameObject);	
	}
}
