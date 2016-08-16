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
	public float speed = 10f;

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

	public void FinalizeMovement()
	{
		currentPos = nextPos;
		transform.position = grid[currentPos[0], currentPos[1]].transform.position;
		LookForKnife();	

		if(GetComponent<PlayerBehavior>())
		{
			boardMan.playerPos = currentPos;
			turnMan.NpcsAndEnemiesMove();
		}
		else if(currentPos[1] < 1)
		{
			castle.TakeIn(this);
		}
		else
		{
			turnMan.movesCompleted++;

			if(turnMan.ContinueTurn())
				turnMan.AllEntitiesMoved();													
		}
	}

    protected IEnumerator SmoothMovement (Vector3 endPos)
    {
		float sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;

		while(sqrRemainingDistance > float.Epsilon)
        {
			transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
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

	public bool isInBounds(int[] requestedPos)
	{
		if(requestedPos[0] >= 0 && requestedPos[0] < gridW && requestedPos[1] >= 0)
			return true;
		else 
			return false;
	}

	public int[] EvaluateMovement()
	{
		int[] requestedPos = new int[]{currentPos[0]+moveDirection[0], currentPos[1]+moveDirection[1]};
		int[] newPos;

		if(isInBounds(requestedPos))
		{
			if(boardMan.entities[requestedPos[0], requestedPos[1]] == null || requestedPos[1] == 0)
				newPos = requestedPos;
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
			if(moveDirection[0] != 0)
			{
				moveDirection[0] = - moveDirection[0];
				return EvaluateMovement();
			}

			newPos = new int[] {currentPos[0], currentPos[1]};	
		}

		return newPos;
	}

	public virtual void ElaborateMove()
	{
		nextPos = EvaluateMovement();
		boardMan.UpdateGrid(currentPos, nextPos);				
		Vector3 newPos = grid[nextPos[0], nextPos[1]].transform.position;
		StartCoroutine(SmoothMovement(newPos));
	}

	public void RequestMove()
	{
		for(int i=0; i<nmbrOfMoves; i++)
		{
			turnMan.movesInitiated++;
			ElaborateMove();
		}
	}


	public void GoToCastle()
	{
//		turnMan.entsToCastle++;
//		Vector3 posInCastle = new Vector3(transform.position.x, castle.transform.position.y, 0);
//		StartCoroutine(SmoothMovement(posInCastle));

		moveDirection = new int[]{0, -1};
		turnMan.movesInitiated++;
		ElaborateMove();

	}

	public void EliminateEntity()
	{
		if(hasKnife && canDrop)
			boardMan.DropKnife(currentPos);

		boardMan.RemoveFromGrid(currentPos);
		Destroy(this.gameObject);	
	}
}
