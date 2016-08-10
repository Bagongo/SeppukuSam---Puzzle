using UnityEngine;
using System.Collections;

public enum PlayerMoves {left, right, still, knife};

public class PlayerBehavior : EntityBehavior {

	void Update () {

		if(Input.GetKeyDown("left"))
			MoveEntity(boardMan.PlayerMove(PlayerMoves.left));
		else if(Input.GetKeyDown("right"))
			MoveEntity (boardMan.PlayerMove(PlayerMoves.right));
		if (Input.GetKeyDown("space"))
			 MoveEntity(boardMan.PlayerMove(PlayerMoves.still));
		else if(Input.GetKeyDown("up") && hasKnife)
		{
			MoveEntity(boardMan.PlayerMove(PlayerMoves.knife));
			hasKnife = false;
		}
	}
}
