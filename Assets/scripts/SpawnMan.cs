using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpawnMan : MonoBehaviour {

	public GameObject[] npcsToAdd;
	public GameObject[] enemiesToAdd;
	public BoardMan boardMan;
	public TurnMan turnMan;
	public LevelMan levelMan;

	private List<GameObject> npcsPool;
	private List<GameObject> enemiesPool;
	private int npcPoolIdx = 0;
	private int enemiesPoolIdx = 0;

	void Start () {

		npcsPool.Add(null);

		CreateNpcRow();
	
	}

	public void EvaluatePoolAdding(int difficulty)
	{
		int totNpcs = npcsToAdd.Length + npcsPool.Count;
		int totEnemies = enemiesToAdd.Length + npcsPool.Count;

		if(difficulty % 5 == 0)
		{
			if(npcsPool.Count < totNpcs)
			{
				npcsPool.Add(npcsToAdd[npcPoolIdx]);
				npcPoolIdx++;
			}						 
		}

		if(difficulty % 8 == 0)
		{
			if(enemiesPool.Count < totEnemies)
			{
				enemiesPool.Add(enemiesToAdd[enemiesPoolIdx]);
				enemiesPoolIdx++;
			}						 
		}			
	}

	public List<GameObject> WhatTypeOfRow()
	{
		int turnNmr = turnMan.turnNmr;

		if(turnNmr%7 == 0)
			return enemiesPool;
		else if(turnNmr%4 == 0 || turnNmr%5 == 0 || turnNmr%6 ==0)
			return CreateNpcRow();
		else 
			return null;
	}

	public List<GameObject> CreateNpcRow()
	{
		List<GameObject> row = new List<GameObject>();
		int idx;

		for(int i=0; i < boardMan.gridW; i++)
		{
			idx = (int) Choose(CreateProbsInverse(npcsPool.Count));
			row.Add(npcsPool[idx]);	
		}

		Debug.Log(row);

		return row;
	}

	float[] CreateProbsInverse(int dim)
	{
		float[] probs = new float[dim];
		int agumentProb;

		for(int i = probs.Length; i>0; i--)
		{
			agumentProb = levelMan.diff;
			probs[i] = 100/i + agumentProb;
			agumentProb -= i*10;
		}

		return probs;
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
