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

		ElaborateMove(moveDirection);
	}

	public void Attack()
	{

		Debug.Log("player Attacking....");
		//animation here or something else...
		if(boardMan.entities[currentPos[0], currentPos[1] + 1] != null)
		{
			EntityBehavior target = boardMan.entities[currentPos[0], currentPos[1] + 1].GetComponent<EntityBehavior>();
			scoreMan.HonorAndScoreUpdater(target, true);
			boardMan.RemoveFromGrid(new int[] {currentPos[0], currentPos[1]+1});
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
