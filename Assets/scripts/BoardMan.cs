using UnityEngine;
using System.Collections;

public class BoardMan : MonoBehaviour {

	public GameObject slot;
	public int gridW ;
	public int gridH ;
	public GameObject[] npcs;
	public GameObject[] enemies;
	public GameObject emptyObj;
	public GameObject sam;
	public int turnNmr;
	public int[] playerPos = {3,0}; 
	public GameObject [,] grid; 
	public GameObject[,] entities;

	private enum Row {empty, npc, enemy};
	private GameObject player;
	//private float delayEndTurn = 0.5f; 

	void Awake(){

		grid = new GameObject[gridW, gridH];

		for(int x=0; x<gridW; x++)
		{
			for(int y=0; y<gridH; y++)
			{
				GameObject gridSlot = (GameObject)Instantiate(slot, new Vector3(x, y, 0), Quaternion.identity);
				gridSlot.transform.parent = transform;
				grid[x,y]=gridSlot;
			}

		}

		transform.position = new Vector3(transform.position.x - grid.GetLength(0)/2, transform.position.y - grid.GetLength(1)/2, 0);

	}

	void Start(){

		entities = new GameObject[gridW, gridH];

		player = Instantiate(sam, grid[3,0].transform.position, Quaternion.identity) as GameObject;
		entities[playerPos[0],playerPos[1]] = player;
		player.GetComponent<PlayerBehavior>().currentPos = playerPos;

		turnNmr = 1;

		SpawnRow(WhatTypeOfRow());
	}

	void Update(){

//		if (Input.GetKeyDown("space"))
//			EndTurn();
    }

	Row WhatTypeOfRow()
	{
		if(turnNmr%7 == 0)
			return Row.enemy;
		else if(turnNmr%4 == 0 || turnNmr%5 == 0 || turnNmr%6 ==0)
			return Row.npc;
		else 
			return Row.empty;
	}

	void SpawnRow(Row whatTospawn)
	{
		Row whatToSpawn = whatTospawn;

		int quantityTofill = 0;
		int quantityFilled = 0;

		GameObject[] spawnables = null;

		switch(whatToSpawn)
		{
			case Row.empty:
				return;
			case Row.npc:
				spawnables = npcs;
				quantityTofill = Random.Range(1,4); //max 4????
				break;
			case Row.enemy:
				spawnables = enemies;
				quantityTofill = 1;
				break;										
		}

		while(quantityTofill > quantityFilled)
		{
			GameObject toSpawn = spawnables[Random.Range(0, spawnables.Length)];
			int posToFill = Random.Range(0, gridW);

			if(entities[posToFill, gridH-1] == null  && entities[posToFill, gridH-2] == null)
			{	
				Vector3 spawnPos = grid[posToFill, gridH-1].transform.position;
				GameObject entity = Instantiate(toSpawn, spawnPos, Quaternion.identity) as GameObject;
				entities[posToFill, gridH-1] = entity;
				quantityFilled++ ;
			}
		}
	}

	public void EndTurn()
	{
		for(int y=1; y<grid.GetLength(1); y++)
		{
			for(int x=0; x<grid.GetLength(0); x++)
			{
				if (entities[x,y] != null)
				{
					if(y == 1)
					{
						Destroy(entities[x,y].gameObject);
						entities[x,y] = null;
					}
					else
					{
						entities[x,y].transform.position = grid[x, y-1].transform.position;
						entities[x,y-1] = entities[x,y];
						entities[x,y] = null;
					}
				}
			}
		}

		turnNmr++;
		SpawnRow(WhatTypeOfRow());
	}

	public int[] PlayerMove(int[] currentPos, Direction dir)
	{
		int [] newPositionGR = currentPos;
				
		switch(dir)
		{
			case Direction.still:
				newPositionGR = currentPos;
				break;
			case Direction.left:
				if(currentPos[0] <= 0)
					newPositionGR = currentPos;
				else
					newPositionGR[0] = currentPos[0]-1;
				break;
			case Direction.right:
				if(currentPos[0] >= gridW-1)
					newPositionGR = currentPos;
				else
					newPositionGR[0] = currentPos[0]+1;
				break;
		}

		entities[currentPos[0], currentPos[1]] = null;
		entities[newPositionGR[0], newPositionGR[1]] = player;

		EndTurn();

		return newPositionGR;							
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
