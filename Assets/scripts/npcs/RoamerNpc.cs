using UnityEngine;
using System.Collections;

public class RoamerNpc : RegNpc, IMovable {

	public bool switchedDir = false;

	private SpriteRenderer sprtRend;

	void Start(){

		moveDirection[0] = Random.value < 0.5f ? -1 : 1;
		FlipSprite();
	}
	
	public override int[] EvaluateMovement()
	{
		int[] requestedPos = new int[]{currentPos[0]+moveDirection[0], currentPos[1]+moveDirection[1]};
		int[] newPos;
							
		if(IsInBoundsX(requestedPos) && boardMan.entities[requestedPos[0], requestedPos[1]] == null)
		{
			newPos = requestedPos;
			switchedDir = false;
		}
		else
		{
			if(!switchedDir)
			{		
				moveDirection[0] = -moveDirection[0];
				FlipSprite();
				switchedDir = true;
				return EvaluateMovement();
			}
			else if(boardMan.entities[currentPos[0], currentPos[1] - 1] == null)
				newPos = new int[]{currentPos[0], currentPos[1] - 1};
			else 
				newPos = currentPos;
		}

		switchedDir = false;
		return newPos;
	}

	private void FlipSprite()
	{
		float facingDirX = moveDirection[0]/ Mathf.Abs(moveDirection[0]);
		transform.localScale = new Vector2(facingDirX, transform.localScale.y);
	}	
}