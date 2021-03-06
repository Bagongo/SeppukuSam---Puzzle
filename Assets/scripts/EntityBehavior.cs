﻿using UnityEngine;
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
	public TurnMan turnMan;
	public ScoreMan scoreMan;
	public int gridW;
	public int gridH;
	public Castle castle;
	public float speed;
	public float[] speedModifiers;
	public Animator anim;
	public SpriteRenderer sprtRend;
	public GameObject bloodSpray;

	protected GameObject knife;
	protected float cachedSpeed;
	protected float prevSpeed;
	protected Vector3 posOffset = Vector3.zero;

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
		anim = GetComponent<Animator>();
		sprtRend = GetComponent<SpriteRenderer>();

		cachedSpeed = speed;
	}

	public virtual void FinalizeMovement()
	{
		isMoving = false;
		speed = cachedSpeed;

		if(anim && nextPos[1] != 0)
			anim.SetBool("isMoving", isMoving);

		currentPos = nextPos;
		transform.position = grid[currentPos[0], currentPos[1]].transform.position + posOffset;
		grid[currentPos[0], currentPos[1]].GetComponent<SpriteRenderer>().color = Color.white;
		sprtRend.sortingOrder = - currentPos[1];
		LookForKnife();	
		SortNextMove();
	}

	public virtual void SortNextMove()
	{
		if(currentPos[1] == 1 && movesCompleted < nmbrOfMoves)
			EraseRemainingMoves();

		else if(currentPos[1] == 0)
			castle.TakeIn(this);

		else
		{
			turnMan.movesCleared++;
			movesCompleted++;

			if(movesCompleted < nmbrOfMoves)
				ElaborateMove();
			else 
				movesCompleted = 0;											
		}
	}

	public void EraseRemainingMoves()
	{
		turnMan.movesCleared += (nmbrOfMoves - movesCompleted);
		turnMan.movesToDelete += (nmbrOfMoves - movesCompleted);
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
//			sprtRend.sortingOrder = Mathf.RoundToInt(transform.position.y * 100);
			
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

	float SpeedModder(float variationFactNeg, float variationFactPos)
	{
		float newSpeed;

		if(nmbrOfMoves < 2)
		{
			newSpeed = Random.Range(speed - speed/variationFactNeg, speed + speed/variationFactPos);
			prevSpeed = newSpeed;
		}
		else 
		{
			newSpeed = speed;
		}

		return newSpeed;
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
				//speed = SpeedModder(speedModifiers[0], speedModifiers[1]);
				isMoving = true;

				if(anim)
					anim.SetBool("isMoving", isMoving);

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
			GameObject animKnife = Instantiate(boardMan.knifePrefab, grid[currentPos[0], currentPos[1]].transform.position, Quaternion.identity) as GameObject;
			animKnife.GetComponent<Knife>().StartPopUpAnim();			 
		}
	}

	public void DropKnife(int[] pos)
	{
		knife = Instantiate(boardMan.knifePrefab, grid[pos[0], pos[1]].transform.position, Quaternion.identity) as GameObject; 
		items[ pos[0], pos[1]] = knife;	
	}

	public void GoToCastle()
	{
		nextPos = new int[]{currentPos[0], currentPos[1] - 1};
		boardMan.UpdateGrid(currentPos, nextPos);
		Vector3 newPos = grid[nextPos[0], nextPos[1]].transform.position;
		isMoving = true;

		//speed =  SpeedModder(speedModifiers[0]/2, speedModifiers[1]/2) * 1.2f;

		if(anim)
			anim.SetBool("isMoving", isMoving);

		StartCoroutine(SmoothMovement(newPos));
	}

	public void RemoveEntity()
	{
		if(isMoving)
			EraseRemainingMoves();

		if(hasKnife && canDrop)
			DropKnife(currentPos); // @posToElminateAt???

		int[] posToEliminateAt = isMoving ? nextPos : currentPos;

		boardMan.RemoveFromGrid(posToEliminateAt);
	}

	void CreateRandomBloodGush()
	{
		Quaternion tempRotation = bloodSpray.transform.rotation;
		Quaternion rotation;
		Vector3 position;

		if(Random.value < .5f)
		{
			rotation = Quaternion.Inverse(tempRotation);
			rotation = new Quaternion(rotation.x, rotation.y, rotation.z, 0);
			position = new Vector3(transform.position.x + 0.3f, transform.position.y, transform.position.x);
		}
		else
		{
			rotation = tempRotation;
			position = transform.position;
		}

		GameObject blood = Instantiate(bloodSpray, position, rotation) as GameObject;
		blood.transform.parent = transform;
		blood.transform.position = new Vector3(blood.transform.position.x, blood.transform.position.y, -1);

		float gravity = blood.GetComponent<ParticleSystem>().gravityModifier;
		blood.GetComponent<ParticleSystem>().gravityModifier = Random.Range(gravity - 1, gravity + 1);
	}

	IEnumerator FadeOut()
	{
		Color color = GetComponent<SpriteRenderer>().color;
		float tempOpacity = color.a;

		while(tempOpacity > 0)
		{
			tempOpacity -= speed * Time.deltaTime * 2;
			color = new Color(color.r, color.g, color.b, tempOpacity);
			GetComponent<SpriteRenderer>().color = color;

			yield return null; 
		}
	}

	public void StartFading()
	{
		StartCoroutine("FadeOut");
	}

	public void DyingAnim()
	{
		anim.SetTrigger("die");
		CreateRandomBloodGush();
	}

	public void DestroyEntity()
	{
		Destroy(this.gameObject);
	}
}
