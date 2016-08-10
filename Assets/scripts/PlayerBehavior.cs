using UnityEngine;
using System.Collections;

public enum PlayerMoves {left, right, still, knife};

public class PlayerBehavior : EntityBehavior {

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
		switch(move)
		{
			case PlayerMoves.still:
				break;
			case PlayerMoves.knife:
				hasKnife = false;				
				boardMan.KnifeImpact();
				break;
			case PlayerMoves.left:
				boardMan.ElaborateMove(currentPos, new int[]{-1, currentPos[1]});
				break;
			case PlayerMoves.right:
				boardMan.ElaborateMove(currentPos, new int[]{1, currentPos[1]});
				break;
		}

		boardMan.playerPos = currentPos;
		boardMan.EndTurn();
	}

}
