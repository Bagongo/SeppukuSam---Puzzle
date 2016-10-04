using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayerMoves {left, right, still, knife};

public class PlayerBehavior : EntityBehavior, IMovable, IAttacker {

	public int movesCollected = 0;
	public List <PlayerMoves> moves;
	public bool playerBlocked = false;


	void Update () {

		if(movesCollected < nmbrOfMoves)
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
		movesCollected++;

		if(moves.Count >= nmbrOfMoves)
			StartCoroutine("ExecuteMoves");
	}

	public void MovesCollector(int moveIdx)
	{
		moves.Add((PlayerMoves)moveIdx);
		movesCollected++;

		if(moves.Count >= nmbrOfMoves)
			StartCoroutine("ExecuteMoves");
	}

	public IEnumerator ExecuteMoves()
	{
		foreach(PlayerMoves m in moves)
		{	
			playerBlocked = true;	
			ParseCommand(m);

			while(playerBlocked)
				yield return new WaitForSeconds(0.01f);
		}

		movesCompleted = 0;
		movesCollected = 0;
		moves = new List<PlayerMoves>();
		StopCoroutine("ExecuteMoves");
	}

	void ParseCommand(PlayerMoves move)
	{										
		switch(move)
		{
			case PlayerMoves.still:
				moveDirection = new int[]{0,0};
				break;
			case PlayerMoves.knife:
				StartCoroutine(KnifeImpact());
				moveDirection = new int[]{0,0};
				return;
			case PlayerMoves.left:
				moveDirection = new int[]{-1,0,};
				break;
			case PlayerMoves.right:
				moveDirection = new int[]{1,0,};
				break;
		}

		ElaborateMove();
	}

	public int[] EvaluateMovement()
	{
		int[] requestedPos = new int[]{currentPos[0]+moveDirection[0], currentPos[1]};
		int[] newPos;
							
		if(IsInBoundsX(requestedPos))
			newPos = requestedPos;	
		else
			newPos = currentPos;	

		return newPos;
	}

	public override void ElaborateMove()
	{
		nextPos = EvaluateMovement();
		//boardMan.UpdateGrid(currentPos, nextPos);   reactivate in case player row considers player as board unity (null and recreate on each move)........				
		Vector3 newPos = grid[nextPos[0], nextPos[1]].transform.position;
		StartCoroutine(SmoothMovement(newPos));
	}

	public override void SortNextMove()
	{
		boardMan.playerPos = currentPos;
		movesCompleted++;
		turnMan.NpcsAndEnemiesMove();
	}

	public void Attack(int[] targetPos)
	{
		//Debug.Log("player Attacking....");
		if(boardMan.entities[targetPos[0], targetPos[1]] != null)
		{
			EntityBehavior target = boardMan.entities[targetPos[0], targetPos[1]].GetComponent<EntityBehavior>();
			//StartAnimation
			KillEntity(target);
		}

		if(turnMan.ContinueGame())			
			turnMan.ResolveFirstRow();
		else
		{
			//turnMan.GameOver();
			turnMan.ResolveFirstRow();
		}
	}

	public void KillEntity(EntityBehavior entB)
	{
		entB.EliminateEntity();
		//Trigger target animation
		scoreMan.HonorAndScoreUpdater(entB, true);
		entB.DestroyEntity();	
	}

	public IEnumerator KnifeImpact()
	{
		int x = currentPos[0];
		int[] pos = {x,0};

		for(int i=1; i<gridH; i++)
		{
			pos[1] = i;

			if(boardMan.entities[x,i] != null)
				break;
		}

		knife = Instantiate(boardMan.knifePrefab, grid[currentPos[0], currentPos[1]].transform.position, Quaternion.identity) as GameObject; 
		Knife knifeB = knife.GetComponent<Knife>();
		knifeB.LaunchKnife(pos);

		while(!knifeB.hasHit)
			yield return new WaitForSeconds(0.1f);

		if(turnMan.ContinueGame())			
			ElaborateMove();
		else
		{
			//turnMan.GameOver();
			ElaborateMove();
		}

	}	


}
