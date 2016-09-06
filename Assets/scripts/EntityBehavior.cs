using UnityEngine;
using System.Collections;

public class EntityBehavior : MonoBehaviour {

	//protect vars that don't need to be public
	public int[] currentPos;
	public int[] nextPos;
	public int[] moveDirection;
	public int movePriority;
	public int honorValue;
	public int scoreValue;
	public Color moveColor;
	public int nmbrOfMoves;
	public int movesCompleted = 0;
	public bool isMoving = false;
	public bool isMutant = false;
	public bool hasKnife;
	public bool canPickUp;
	public bool canDrop;
	public BoardMan boardMan;
	public GameObject[,] grid;
	public GameObject[,] items;
	public GameObject knifePrefab;
	public TurnMan turnMan;
	public ScoreMan scoreMan;
	public int gridW;
	public int gridH;
	public Castle castle;
	public float speed;

	protected GameObject knife;

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

	public virtual void FinalizeMovement()
	{
		isMoving = false;
		currentPos = nextPos;
		transform.position = grid[currentPos[0], currentPos[1]].transform.position;
		grid[currentPos[0], currentPos[1]].GetComponent<SpriteRenderer>().color = Color.white;
		LookForKnife();	
		SortNextMove();
	}

	public virtual void SortNextMove()
	{
		if(currentPos[1] == 1 && movesCompleted < nmbrOfMoves)
		{
			EraseRemainingMoves();

			if(turnMan.ContinueTurn())
				turnMan.AllEntitiesMoved();	
		}
		else if(currentPos[1] == 0)
		{
			castle.TakeIn(this);
		}
		else
		{
			turnMan.movesCleared++;
			movesCompleted++;

			if(movesCompleted < nmbrOfMoves)
				ElaborateMove();
			else 
				movesCompleted = 0;

			if(turnMan.ContinueTurn())
				turnMan.AllEntitiesMoved();													
		}
	}

	public void EraseRemainingMoves()
	{
		turnMan.movesCleared += (nmbrOfMoves - movesCompleted);
		movesCompleted = 0;
	}

	public void CompensateMoves(int howManyMovesToBeLeft)
	{
		movesCompleted = nmbrOfMoves - howManyMovesToBeLeft;
		turnMan.movesCleared += movesCompleted;
	}

    public IEnumerator SmoothMovement (Vector3 endPos)
    {
		float sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;

		while(sqrRemainingDistance > 0.001f)
        {
			transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
			sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;
			
            yield return null;
        }

        FinalizeMovement();
    }

	public bool IsInBoundsX(int[] requestedPos)
	{
		if(requestedPos[0] >= 0 && requestedPos[0] < gridW)
			return true;
		else 
			return false;
	}

	public int[] TryNeighbor(int[] requestedPos)
	{
		int[] availablePos;
		int at = Random.value < 0.5 ? 1 : -1;

		if(IsInBoundsX(new int[]{requestedPos[0] - at, requestedPos[1]}) && boardMan.entities[requestedPos[0] - at, requestedPos[1]] == null)
			availablePos = new int[] {requestedPos[0] - at, requestedPos[1]}; 
		else if(IsInBoundsX(new int[]{requestedPos[0] + at, requestedPos[1]}) && boardMan.entities[requestedPos[0] + at, requestedPos[1]] == null)
			availablePos = new int[] {requestedPos[0] + at, requestedPos[1]}; 
		else
			availablePos = new int[] {currentPos[0], currentPos[1]};

		return availablePos;
	}

	public virtual void ElaborateMove()
	{
		if(this is IMovable)
		{
			if(isMutant && transform.GetChild(0).GetComponent<MutatingNin>().EvaluateMutation())
				transform.GetChild(0).GetComponent<MutatingNin>().PrepareMutation();				
			else
			{
				LookForKnife();
				nextPos = GetComponent<IMovable>().EvaluateMovement();
				grid[nextPos[0], nextPos[1]].GetComponent<SpriteRenderer>().color = moveColor;
				boardMan.UpdateGrid(currentPos, nextPos);				
				Vector3 newPos = grid[nextPos[0], nextPos[1]].transform.position;
				isMoving = true;
				StartCoroutine(SmoothMovement(newPos));
			}
		}
		else 
		{
			Debug.Log("This entity can't make a move....");
		}
	}

	public void LookForKnife()
	{
		if(canPickUp && !hasKnife && items[currentPos[0], currentPos[1]] != null)
		{
			Destroy(items[currentPos[0], currentPos[1]]);
			items[currentPos[0], currentPos[1]] = null;
			hasKnife = true;
		}
	}

	public void DropKnife(int[] pos)
	{
		knife = Instantiate(knifePrefab, grid[pos[0], pos[1]].transform.position, Quaternion.identity) as GameObject; 
		items[ pos[0], pos[1]] = knife;	
	}

	public void GoToCastle()
	{
		nextPos = new int[]{currentPos[0], currentPos[1] - 1};
		boardMan.UpdateGrid(currentPos, nextPos);
		Vector3 newPos = grid[nextPos[0], nextPos[1]].transform.position;
		StartCoroutine(SmoothMovement(newPos));
	}

	public void EliminateEntity()
	{
		int[] posToEliminateAt = isMoving ? nextPos : currentPos;

		if(isMoving)
		{
			posToEliminateAt = nextPos;
			EraseRemainingMoves();
		}
		else
			posToEliminateAt = currentPos;

		if(hasKnife && canDrop)
			DropKnife(currentPos);

		Destroy(this.gameObject);	
		boardMan.RemoveFromGrid(posToEliminateAt);
	}
}
