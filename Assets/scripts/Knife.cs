﻿using UnityEngine;
using System.Collections;

public class Knife : EntityBehavior {

	public bool hasHit = false;

	private int[] targetPos;
	private PlayerBehavior player;

	void Start()
	{
		player = (PlayerBehavior)FindObjectOfType(typeof(PlayerBehavior));
	}

	public void LaunchKnife(int[] target)
	{
		targetPos = target;
		Vector3 targetCoord = grid[targetPos[0], targetPos[1]].transform.position;		
		StartCoroutine(SmoothMovement(targetCoord));
	}

	public override void FinalizeMovement()
	{
		transform.position = grid[targetPos[0], targetPos[1]].transform.position;
		items[targetPos[0], targetPos[1]] = this.gameObject;
		
		if(boardMan.entities[targetPos[0], targetPos[1]] != null)
		{
			EntityBehavior entB = boardMan.entities[targetPos[0], targetPos[1]].GetComponent<EntityBehavior>();

			//entB.LookForKnife(); figure out if needed to be picked up first and dropped at elimination........

			player.KillEntity(entB);
		}

		hasHit = true;
	}
}