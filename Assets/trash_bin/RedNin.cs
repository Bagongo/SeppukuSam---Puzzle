using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RedNin : RegEnemy {

	public int availableLeaps = 2;

	//private bool hasLeapedPrevMove = false;
	private PlayerBehavior player;

	void Start()
	{
		player = FindObjectOfType<PlayerBehavior>();
	}


	public override void ElaborateMove()
	{
		string coroutine;

		if(CanLeap())
		{
			int[] freeSlotsX = AvailableLeapingSlots().ToArray();
			nextPos = new int[] {freeSlotsX[Random.Range(0, freeSlotsX.Length - 1)], currentPos[1]};
			//hasLeapedPrevMove = true;
			availableLeaps--;
			coroutine = "NinjaLeap";
		}
		else
		{
			nextPos = EvaluateMovement();
			//hasLeapedPrevMove = false;
			coroutine = "SmoothMovement";
		}

		Debug.Log("starting" + coroutine);

		grid[nextPos[0], nextPos[1]].GetComponent<SpriteRenderer>().color = moveColor;
		boardMan.UpdateGrid(currentPos, nextPos);
		Vector3 newPos = grid[nextPos[0], nextPos[1]].transform.position;				
		isMoving = true;
		StartCoroutine(coroutine, newPos);
	}

	protected bool CanLeap()
	{
		if(currentPos[1] < gridH - 2 && currentPos[1] > 3 && 
			player.currentPos[0] == currentPos[0] &&
			AvailableLeapingSlots().Count > 0 && 
			//Random.value > 0.4f &&
			//!hasLeapedPrevMove &&
			availableLeaps > 0)
			return true;
		else 
			return false; 				
	}

	protected List<int> AvailableLeapingSlots()
	{
		List<int> freeSlotsX = new List<int>(); 

		for(int x=currentPos[0] - 3; x<=currentPos[0] + 3; x++)
		{
			if(IsInBoundsX(new int[]{x, currentPos[1]}) && boardMan.entities[x, currentPos[1]] == null)
				freeSlotsX.Add(x);				
		}

		return freeSlotsX;				
	}

	private IEnumerator NinjaLeap(Vector3 newPos)
    {
		Color color = GetComponent<SpriteRenderer>().color;
		float tempOpacity = color.a;

		while(tempOpacity > 0)
		{
			tempOpacity -= speed * Time.deltaTime * 2;
			color = new Color(color.r, color.g, color.b, tempOpacity);
			GetComponent<SpriteRenderer>().color = color;

			yield return null; 
		}

		transform.position = newPos;

		while(tempOpacity < 1)
		{
			tempOpacity += speed * Time.deltaTime * 2;
			color = new Color(color.r, color.g, color.b, tempOpacity);
			GetComponent<SpriteRenderer>().color = color;

			yield return null;
		}

		CompensateMoves(1);
		FinalizeMovement();
	}

	float Choose (float[] probs) 
	{
        float total = 0;

        foreach (float elem in probs) {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i= 0; i < probs.Length; i++) {
            if (randomPoint < probs[i]) {
                return i;
            }
            else {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }		 
			
}
