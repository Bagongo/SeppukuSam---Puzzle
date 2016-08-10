using UnityEngine;
using System.Collections;

public class BoardMan : MonoBehaviour {

	public GameObject slot;
	public int gridW ;
	public int gridH ;
	public GameObject[] npcs;
	public GameObject[] enemies;
	public GameObject playerPrefab;
	public GameObject knifePrefab;
	public int turnNmr;
	public int[] playerPos = {3,0}; 
	public GameObject [,] grid; 
	public GameObject[,] entities;
	public GameObject[,] items;
	public int turnsAtSetup = 5;

	private enum Row {empty, npc, enemy};
	private GameObject knife;
	private GameObject player; 

//	private GameObject entity; use to trim down some of the parameter passing....
//	private int[] enitityPos;

	void Awake(){

		grid = new GameObject[gridW, gridH];
		items = new GameObject[gridW, gridH];

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

		player = Instantiate(playerPrefab, grid[playerPos[0], playerPos[1]].transform.position, Quaternion.identity) as GameObject;
		entities[playerPos[0],playerPos[1]] = player;
		player.GetComponent<PlayerBehavior>().currentPos = playerPos;

		turnNmr = 1;

		SpawnRow(WhatTypeOfRow());

		for(int i=0; i<=turnsAtSetup; i++)
			EndTurn();
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

	void SpawnRow(Row whatTospawn) //consider delegating type of spawnable choice to other method or brand new spawner in game element...
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

			if(entities[posToFill, gridH-1] == null && entities[posToFill, gridH-2] == null)
			{	
				Vector3 spawnPos = grid[posToFill, gridH-1].transform.position;
				GameObject newEntity = Instantiate(toSpawn, spawnPos, Quaternion.identity) as GameObject;
				newEntity.GetComponent<EntityBehavior>().currentPos = new int[]{posToFill, gridH-1};
				newEntity.GetComponent<EntityBehavior>().LookForKnife();
				entities[posToFill, gridH-1] = newEntity;
				quantityFilled++ ;
			}
		}
	}

	public void EndTurn()
	{
		for(int y=1; y<gridH; y++)
		{
			for(int x=0; x<gridW; x++)
			{
				if (entities[x,y] != null)
				{
					if(y == 1)
						RemoveFromGrid(new int[]{x,y});
					else						
						entities[x,y].GetComponent<EntityBehavior>().RequestMove();
				}
			}
		}

		turnNmr++;
		SpawnRow(WhatTypeOfRow());
	}

	public void KnifeImpact()
	{
		int x = playerPos[0];

		for(int i=1; i<gridH; i++)
		{
			int[] pos = new int[]{x,i};

			if(entities[x,i] != null)
			{
				RemoveFromGrid(pos); //should call method 'killSmething' once it's ready....
				DropKnife(pos);	
				break;
			}
			else if(i == gridH-1 && entities[x,i] == null)
				DropKnife(pos);	
		}	
	}

	public void PickUpKnife(int[] pos)
	{
		Destroy(items[pos[0], pos[1]]);
		items[pos[0], pos[1]] = null;
	}

	public void DropKnife(int[] pos)
	{
		knife = Instantiate(knifePrefab, grid[pos[0],pos[1]].transform.position, Quaternion.identity) as GameObject; //instantiate at launch when implementing anims......
		items[pos[0],pos[1]] = knife;	
	}

	public int[] EvaluateMovement(int[]pos, int[]dir)
	{
		int[] requestedPos = new int[]{pos[0]+dir[0], pos[1]+dir[1]};
		int[] newPos;

		if((requestedPos[0] >= 0 && requestedPos[0] < gridW) && requestedPos[1] >= 0 && entities[requestedPos[0], requestedPos[1]] == null)
			newPos = new int[] {requestedPos[0], requestedPos[1]};  
		else
		{
			Debug.Log("invalid move - pos: " + pos[0] + " " + pos[1]);	
			newPos = new int[] {pos[0], pos[1]};	
		}

		return newPos;
	}

	public void ElaborateMove(int[]pos, int[]dir)
	{
		GameObject ent = entities[pos[0], pos[1]];
		EntityBehavior entB = ent.GetComponent<EntityBehavior>();

		int[] newPos = EvaluateMovement(pos, dir);

		entities[pos[0], pos[1]] = null;
		entities[newPos[0], newPos[1]] = ent;

		entB.MoveEntity(newPos);

		if(ent.GetComponent<EnemyBehavior>())
		{}
		else if(ent.GetComponent<NpcBehavior>())
		{}					
	}

	void RemoveFromGrid(int[] pos)
	{
		if(entities[pos[0], pos[1]].GetComponent<EntityBehavior>().hasKnife  && entities[pos[0], pos[1]].GetComponent<EntityBehavior>().canDrop)
		{
			DropKnife(new int[] {pos[0], pos[1] - 1});
			player.GetComponent<PlayerBehavior>().LookForKnife(); //put somewhere in animation or else. Move the whole knifedropping at eliminate.........
		}

		Destroy(entities[pos[0], pos[1]].gameObject);
		entities[pos[0], pos[1]] = null;												
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

