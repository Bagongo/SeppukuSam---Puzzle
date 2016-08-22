using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayerMoves {left, right, still, knife};

public class PlayerBehavior : EntityBehavior {

	public int movesCollected = 0;
	public List <PlayerMoves> moves;
	public bool playerBlocked = false;


	void Update () {

		if(!playerBlocked)
		{
			if(Input.GetKeyDown("left"))
				MovesCollector(PlayerMoves.left);
			else if(Input.GetKeyDown("right"))
				MovesCollector(PlayerMoves.right);
			else if (Input.GetKeyDown("space"))
				MovesCollector(PlayerMoves.still);
			else if(Input.GetKeyDown("up"))
			{  
				if(hasKnife)
				{
					hasKnife = false;
					MovesCollector(PlayerMoves.knife);
				}
				else
					Debug.Log("Doesn't have knife...");
			}

		}
	}

	public void MovesCollector(PlayerMoves move)
	{
		moves.Add(move);

		if(moves.Count >= nmbrOfMoves)
			StartCoroutine("ExecuteMoves");
	}

	public void MovesCollector(int moveIdx)
	{
		moves.Add((PlayerMoves)moveIdx);

		if(moves.Count >= nmbrOfMoves)
			StartCoroutine("ExecuteMoves");
	}

	public IEnumerator ExecuteMoves()
	{
		foreach(PlayerMoves m in moves)
		{		
			ParseCommand(m);

			while(playerBlocked)
				yield return new WaitForSeconds(0.01f);
		}

		movesCollected = 0;
		moves = new List<PlayerMoves>();
		StopCoroutine("ExecuteMoves");
	}

	void ParseCommand(PlayerMoves move)
	{	
		playerBlocked = true;
										
		switch(move)
		{
			case PlayerMoves.still:
				moveDirection = new int[]{0,0};
				break;
			case PlayerMoves.knife:
				hasKnife = false;				
				boardMan.KnifeImpact();
				moveDirection = new int[]{0,0};
				break;
			case PlayerMoves.left:
				moveDirection = new int[]{-1,0,};
				break;
			case PlayerMoves.right:
				moveDirection = new int[]{1,0,};
				break;
		}

		ElaborateMove();
	}

	public override void ElaborateMove()
	{
		int[] requestedPos = new int[]{currentPos[0]+moveDirection[0], currentPos[1]};

		if(IsInBoundsX(requestedPos))
			nextPos = requestedPos;
		else
		{
			Debug.Log("invalid move requested...");
			nextPos = currentPos;
		}

		boardMan.UpdateGrid(currentPos, nextPos);				
		Vector3 newPos = grid[nextPos[0], nextPos[1]].transform.position;
		StartCoroutine(SmoothMovement(newPos));
	}

	public override void Attack(int[] targetPos)
	{

		//Debug.Log("player Attacking....");
		//animation here or something else...
		if(boardMan.entities[targetPos[0], targetPos[1]] != null)
		{
			EntityBehavior target = boardMan.entities[targetPos[0], targetPos[1]].GetComponent<EntityBehavior>();
			scoreMan.HonorAndScoreUpdater(target, true);
			target.EliminateEntity();
		}

		if(turnMan.ContinueGame())			
			turnMan.ResolveFirstRow();
		else
		{
			//turnMan.GameOver();
			turnMan.ResolveFirstRow();
		}
	}

}
