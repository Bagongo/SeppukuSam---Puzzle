using UnityEngine;
using System.Collections;

public enum PlayerMoves {left, right, still, knife};

public class PlayerBehavior : EntityBehavior {

	public bool playerBlocked = false;

	void Update () {

		if(!playerBlocked)
		{
		if(Input.GetKeyDown("left"))
			ParseCommand(PlayerMoves.left);
		else if(Input.GetKeyDown("right"))
			ParseCommand(PlayerMoves.right);
		else if (Input.GetKeyDown("space"))
			ParseCommand(PlayerMoves.still);
		else if(Input.GetKeyDown("up") && hasKnife)
			ParseCommand(PlayerMoves.knife);
		}
	}

	void ParseCommand(PlayerMoves move)
	{	
		playerBlocked = true;
										
		switch(move)
		{
			case PlayerMoves.still:
				ElaborateMove(new int[]{0,0});			
				break;
			case PlayerMoves.knife:
				hasKnife = false;				
				boardMan.KnifeImpact();
				ElaborateMove(new int[]{0,0});
				break;
			case PlayerMoves.left:
				ElaborateMove(new int[]{-1,0});
				break;
			case PlayerMoves.right:
				ElaborateMove(new int[]{1,0});
				break;
		}
	}

	public void Attack()
	{
		//animation here or something else...
		if(boardMan.entities[currentPos[0], currentPos[1] + 1] != null)
		{
			EntityBehavior target = boardMan.entities[currentPos[0], currentPos[1] + 1].GetComponent<EntityBehavior>();
			scoreMan.HonorAndScoreUpdater(target, true);
			boardMan.RemoveFromGrid(new int[] {currentPos[0], currentPos[1]+1});
			target.EliminateEntity();
			Debug.Log("killed " + target.name);
		}

		if(turnMan.EvaluateContinuation())
			turnMan.ResolveFirstRow();
		else
			turnMan.ResolveFirstRow();
	}

}
