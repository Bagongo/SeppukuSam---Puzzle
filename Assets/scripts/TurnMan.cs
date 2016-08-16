using UnityEngine;
using System.Collections;

public enum Row {empty, npc, enemy};

public class TurnMan : MonoBehaviour {

	public int turnNmr;
	public int turnsAtSetup = 5;
	public bool holdStep = false;
	public int totalMovesToClear = 0;
	public int movesCleared = 0;

	private int gridW;
	private int gridH;
	private BoardMan boardMan;
	private ScoreMan scoreMan;
	private PlayerBehavior player;

	void Awake()
	{
		boardMan = (BoardMan)FindObjectOfType(typeof(BoardMan));
		gridW = boardMan.gridW;
		gridH = boardMan.gridH;

		scoreMan = (ScoreMan)FindObjectOfType(typeof(ScoreMan));
	}

	void Start () {

		player = FindObjectOfType<PlayerBehavior>();					

		turnNmr = 1;

		boardMan.SpawnRow(WhatTypeOfRow(turnNmr));

//		for(int i=0; i<=turnsAtSetup; i++)
//		{
//			NpcsAndEnemiesMove();
//			NextTurn();
//		}
	}

	Row WhatTypeOfRow(int turnNmr)
	{
		if(turnNmr%7 == 0)
			return Row.enemy;
		else if(turnNmr%4 == 0 || turnNmr%5 == 0 || turnNmr%6 ==0)
			return Row.npc;
		else 
			return Row.empty;
	}

	public void AllEntitiesMoved()
	{
		player.Attack();
	}

	public void ResolveFirstRow()
	{		
		int entitiesFound = 0;

		for(int x=0; x<gridW; x++)
		{
			if (boardMan.entities[x,1] != null)
			{
				entitiesFound++;
				EntityBehavior entB = boardMan.entities[x,1].GetComponent<EntityBehavior>();
				totalMovesToClear++;
				entB.GoToCastle();
			}
		}

		if(entitiesFound < 1)
			NextTurn();									
	}

	public void NpcsAndEnemiesMove()
	{
		int entitiesFound = 0;

		for(int y=2; y<gridH; y++)
		{
			for(int x=0; x<gridW; x++)
			{
				if (boardMan.entities[x,y] != null)
				{						
					entitiesFound++;
					EntityBehavior entb = boardMan.entities[x,y].GetComponent<EntityBehavior>();
					totalMovesToClear += entb.nmbrOfMoves;
					entb.StartCoroutine(entb.RequestMove());
				}
			}
		}

		if(entitiesFound < 1)
			AllEntitiesMoved();		
	}

	public bool ContinueTurn()
	{
		if(movesCleared >= totalMovesToClear)
		{
			movesCleared = 0;
			totalMovesToClear = 0;
			return true;
		}
		else
			return false;
	}

	public bool ContinueGame()
	{
		if(scoreMan.honor > 0)
			return true;
		else
			return false;
	}

	public void NextTurn()
	{
		turnNmr++;
		boardMan.SpawnRow(WhatTypeOfRow(turnNmr));
		player.playerBlocked = false;	
	}

	public void GameOver()
	{
		Debug.Log("Game Over");
	}


}
