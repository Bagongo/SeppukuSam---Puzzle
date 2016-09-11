using UnityEngine;
using System.Collections;
using System.Linq;

public class RegEnemy : EntityBehavior, IMovable, IAttacker{

	protected int[] target;

	public int[] EvaluateMovement()
	{
		int[] requestedPos = new int[]{currentPos[0]+moveDirection[0], currentPos[1]+moveDirection[1]};
		int[] newPos;
									
		if(IsInBoundsX(requestedPos))
		{
			if(boardMan.entities[requestedPos[0], requestedPos[1]] == null)
				newPos = requestedPos;
			else
			{
				int[] tempPos = TryNeighbor(requestedPos);

				if (!currentPos.SequenceEqual(tempPos))
					newPos = tempPos;
				else    //enemy is stuck
				{				
					target = WhereToAttack();

					if(target != null)
					{
						Attack(target);
						newPos = target;
						target = null;	
					}
					else
						newPos = tempPos;
				}
			} 
		}
		else 
			newPos = currentPos;

		return newPos;
	}

	public int[] WhereToAttack()
	{ 
		int[] attackHere;
		int checkFirst = Random.value <= .5 ? 1 : -1;
		int[] slotToCheck = new int[]{currentPos[0] + checkFirst, currentPos[1] - 1};

		if(boardMan.entities[currentPos[0], currentPos[1] - 1].tag == "npc")
			attackHere = new int[]{currentPos[0], currentPos[1] - 1};
		else if(IsInBoundsX(slotToCheck) && boardMan.entities[slotToCheck[0], slotToCheck[1]].tag == "npc")
			attackHere = slotToCheck;
		else if(IsInBoundsX(new int[]{-slotToCheck[0], slotToCheck[1]}) && boardMan.entities[-slotToCheck[0], slotToCheck[1]].tag == "npc")
			attackHere = new int[]{-slotToCheck[0], slotToCheck[1]};
		else 
			attackHere = null;

		Debug.Log(attackHere[0] + " " + attackHere[1]);

		return attackHere;		
	}

	public void Attack(int[] targetPos)
	{
		EntityBehavior entB = boardMan.entities[targetPos[0], targetPos[1]].GetComponent<EntityBehavior>();
		KillEntity(entB);
	}

	public void KillEntity(EntityBehavior entB)
	{
		scoreMan.HonorAndScoreUpdater(entB, false);
		entB.EliminateEntity();
	}		
}
