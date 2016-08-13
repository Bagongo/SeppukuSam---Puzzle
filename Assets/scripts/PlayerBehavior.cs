using UnityEngine;
using System.Collections;

public enum PlayerMoves {left, right, still, knife};

public class PlayerBehavior : EntityBehavior {

	public bool playerBLocked = false;

	void Update () {

		if(Input.GetKeyDown("left"))
			ParseCommand(PlayerMoves.left);
		else if(Input.GetKeyDown("right"))
			ParseCommand(PlayerMoves.right);
		else if (Input.GetKeyDown("space"))
			ParseCommand(PlayerMoves.still);
		else if(Input.GetKeyDown("up") && hasKnife)
			ParseCommand(PlayerMoves.knife);
	}

	void ParseCommand(PlayerMoves move)
	{	
		playerBLocked = true;
										
		switch(move)
		{
			case PlayerMoves.still:
				break;
			case PlayerMoves.knife:
				hasKnife = false;				
				boardMan.KnifeImpact();
				break;
			case PlayerMoves.left:
				ElaborateMove(new int[]{-1, currentPos[1]});
				break;
			case PlayerMoves.right:
				ElaborateMove(new int[]{1, currentPos[1]});
				break;
		}

		boardMan.playerPos = currentPos;
		turnMan.OtherEntitiesMove();
		Attack();
	}

	void Attack()
	{
		//animation here or something else...
		if(boardMan.entities[currentPos[0], currentPos[1] + 1] != null)
		{
			EntityBehavior target = boardMan.entities[currentPos[0], currentPos[1] + 1].GetComponent<EntityBehavior>();
			scoreMan.HonorAndScoreUpdater(target, true);
			boardMan.RemoveFromGrid(new int[] {currentPos[0], currentPos[1]+1});
			Debug.Log("killed " + target.name);
		}

		turnMan.EvaluateContinuation();
	}

}
