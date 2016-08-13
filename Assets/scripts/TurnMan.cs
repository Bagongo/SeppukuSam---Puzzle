using UnityEngine;
using System.Collections;

public enum Row {empty, npc, enemy};

public class TurnMan : MonoBehaviour {

	public int turnNmr;
	public int turnsAtSetup = 5;

	private int gridW;
	private int gridH;
	private BoardMan boardMan;
	private ScoreMan scoreMan;


	void Awake()
	{
		boardMan = (BoardMan)FindObjectOfType(typeof(BoardMan));

		gridW = boardMan.gridW;
		gridH = boardMan.gridH;

		scoreMan = (ScoreMan)FindObjectOfType(typeof(ScoreMan));					
	}

	void Start () {

		turnNmr = 1;

		boardMan.SpawnRow(WhatTypeOfRow(turnNmr));

		for(int i=0; i<=turnsAtSetup; i++)
		{
			OtherEntitiesMove();
			NextTurn();
		}
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

	public void OtherEntitiesMove()
	{
		for(int y=2; y<gridH; y++)
		{
			for(int x=0; x<gridW; x++)
			{
				if (boardMan.entities[x,y] != null)
				{						
					boardMan.entities[x,y].GetComponent<EntityBehavior>().RequestMove();
				}
			}
		}
	}

	public void EvaluateContinuation()
	{
		if(scoreMan.honor > 0)
		{
			ResolveFirstRow();
		}
		else
		{
			Debug.Log("game over!!!");
			ResolveFirstRow();
		}
	}

	public void ResolveFirstRow()
	{		
		for(int x=0; x<gridW; x++)
		{
			if (boardMan.entities[x,1] != null)
				boardMan.RemoveFromGrid(new int[]{x,1});
		}
					
		NextTurn();				
	}

	public void NextTurn()
	{
		turnNmr++;
		boardMan.SpawnRow(WhatTypeOfRow(turnNmr));	
	}


}
