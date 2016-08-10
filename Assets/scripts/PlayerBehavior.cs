using UnityEngine;
using System.Collections;

public enum PlayerMoves {left, right, still, knife};

public class PlayerBehavior : EntityBehavior {

	void Update () {

		if(Input.GetKeyDown("left"))
			boardMan.PlayerMove(PlayerMoves.left);
		else if(Input.GetKeyDown("right"))
			boardMan.PlayerMove(PlayerMoves.right);
		else if (Input.GetKeyDown("space"))
			boardMan.PlayerMove(PlayerMoves.still);
		else if(Input.GetKeyDown("up") && hasKnife)
		{
			boardMan.PlayerMove(PlayerMoves.knife);
			hasKnife = false;
		}
	}

}
