﻿using UnityEngine;
using System.Collections;

public class Castle : MonoBehaviour {

	public ScoreMan scoreMan;
	public TurnMan turnMan;
	public BoardMan boardMan;

	public void TakeIn(EntityBehavior entB)
	{

		//Debug.Log(entB.name + " " + entB.currentPos[0] + " " + entB.currentPos[1]);

		scoreMan.HonorAndScoreUpdater(entB, false);

//		if(turnMan.ContinueGame())
//		{
			entB.EliminateEntity();
			turnMan.movesCompleted++;

			if(turnMan.ContinueTurn())
			{
				turnMan.NextTurn();
			}
//		}
//		else
//			turnMan.GameOver();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{

		//Debug.Log(entB.name + " " + entB.currentPos[0] + " " + entB.currentPos[1]);

		EntityBehavior entB = coll.GetComponent<EntityBehavior>();

		scoreMan.HonorAndScoreUpdater(entB, false);

		boardMan.RemoveFromGrid(entB.currentPos);

//		if(turnMan.ContinueGame())
//		{
			entB.EliminateEntity();
			turnMan.movesCompleted++;

			if(turnMan.ContinueTurn())
			{
				turnMan.NextTurn();
			}
	
	}
}
