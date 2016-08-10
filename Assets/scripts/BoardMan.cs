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
	private GameObject player;
	private GameObject entity;
	private int[] enitityPos;
	private GameObject knife;

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

		for(int x=0; x<gridW; x++)
		{
			for(int y=0; y<gridH; y++)
				Debug.Log(items[x,y]);

		}

		entities = new GameObject[gridW, gridH];

		player = Instantiate(playerPrefab, grid[3,0].transform.position, Quaternion.identity) as GameObject;
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
						EliminateEntity(new int[]{x,y});
					else						
						EvaluateEntityMove(new int[]{x,y}, entities[x,y]);
				}
			}
		}

		turnNmr++;
		SpawnRow(WhatTypeOfRow());
	}

	public int[] PlayerMove(PlayerMoves dir)
	{
		int [] newPositionGR = playerPos;
				
		switch(dir)
		{
			case PlayerMoves.still:
				newPositionGR = playerPos;
				break;
			case PlayerMoves.knife:
				newPositionGR = playerPos;
				KnifeImpact();
				break;
			case PlayerMoves.left:
				if(playerPos[0] <= 0)
					newPositionGR = playerPos;
				else
					newPositionGR[0] = playerPos[0]-1;
				break;
			case PlayerMoves.right:
				if(playerPos[0] >= gridW-1)
					newPositionGR = playerPos;
				else
					newPositionGR[0] = playerPos[0]+1;
				break;
		}

		entities[playerPos[0], playerPos[1]] = null;
		playerPos = new int[]{newPositionGR[0], newPositionGR[1]};
		entities[playerPos[0], playerPos[1]] = player;

		EndTurn();

		return playerPos;							
	}

	void KnifeImpact()
	{
		int x = playerPos[0];

		for(int i=1; i<gridH; i++)
		{
			int[] pos = new int[]{x,i};

			if(entities[x,i] != null)
			{
				EliminateEntity(pos);
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

	public void EvaluateEntityMove(int[] pos, GameObject ent)
	{
		EntityBehavior entB = ent.GetComponent<EntityBehavior>();
		int[] requestedPos = new int[]{pos[0]+entB.moveDirection[0], pos[1]+entB.moveDirection[1]};

		if(entities[requestedPos[0], requestedPos[1]] == null)
		{
			entities[requestedPos[0], requestedPos[1]] = ent;
			entities[pos[0], pos[1]] = null;
			entB.MoveEntity(new int[] {requestedPos[0], requestedPos[1]});  
		}
		else
		{
			Debug.Log("invalid move requested by" + ent.name + "pos: " + pos[0] + " " + pos[1]);	
			entB.MoveEntity(new int[] {pos[0], pos[1]});	
		}

		if(ent.GetComponent<EnemyBehavior>())
		{}
		else if(ent.GetComponent<NpcBehavior>())
		{}					
	}

	void EliminateEntity(int[] pos)
	{
		if(entities[pos[0], pos[1]].GetComponent<EntityBehavior>().hasKnife  && entities[pos[0], pos[1]].GetComponent<EntityBehavior>().canDrop)
			DropKnife(new int[] {pos[0], pos[1] - 1});

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

