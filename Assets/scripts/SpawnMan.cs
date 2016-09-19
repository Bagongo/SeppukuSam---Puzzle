using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpawnMan : MonoBehaviour {

	public GameObject[] npcsToAdd;
	public GameObject[] enemiesToAdd;
	public BoardMan boardMan;
	public LevelMan levelMan;
	public List<GameObject> npcsPool;
	public List<GameObject> enemiesPool;

	private TurnMan turnMan;
	private int gridW;
	private int gridH;

	void Awake()
	{
		gridW = boardMan.gridW;
		gridH = boardMan.gridH;
		turnMan = (TurnMan)FindObjectOfType(typeof(TurnMan)); 
	}


	void Start () {

		PopulatePool(npcsPool, npcsToAdd, 2);
		PopulatePool(enemiesPool, enemiesToAdd, 1);
		PopulateBoard();
	}

	void PopulatePool(List<GameObject> pool, GameObject[] toAddFrom, int upToIdx)
	{
		for(int i=0; i < upToIdx; i++)
			pool.Add(toAddFrom[i]);	
	}

	public void PopulateBoard()
	{
		for(int y=1; y<gridH; y++)
		{
			if(y==4 || y==6)
				SpawnNpcsRow(y);
			else if(y== gridH-1)
				SpawnEnemyRow(y);			
		}
	}

	public void EvaluatePoolAdding(int difficulty)
	{
		if(difficulty % 5 == 0)
		{
			if(npcsPool.Count < npcsToAdd.Length)
				npcsPool.Add(npcsToAdd[npcsPool.Count]);						 
		}

		if(difficulty % 8 == 0)
		{
			if(enemiesPool.Count < enemiesToAdd.Length)
				enemiesPool.Add(enemiesToAdd[enemiesPool.Count]);						 
		}			
	}

	public void SpawnNpcsRow(int yPos)
	{
		int npcQuant = (int) Choose(CreateProbs(gridW-1, turnMan.turnNmr)) + 1;
		List<int> availableSlots = new List<int>();

		for(int i=0; i < gridW; i++)
			availableSlots.Add(i);

		for(int i=1; i <= npcQuant; i++)
		{
			int idx = (int)Choose(CreateProbs(npcsPool.Count, levelMan.diff));
			GameObject npc = npcsPool[idx];
			int xPos = availableSlots[Random.Range(0, availableSlots.Count)]; 
			availableSlots.Remove(xPos);
			boardMan.InstantiateSingleEntity(npc, new int[]{i, yPos});	
		}	
	}

	public void SpawnEnemyRow(int yPos)
	{
		int idx = (int)Choose(CreateProbs(enemiesPool.Count, levelMan.diff));
		GameObject enemy = enemiesPool[idx];
		int xPos = Random.Range(0, gridW - 1);

		boardMan.InstantiateSingleEntity(enemy, new int[]{xPos, yPos});
	}

	float[] CreateProbs(int dim, int increment)
	{
		float[] probs = new float[dim];

		for(int i=0; i < probs.Length; i++)
		{
			probs[i] = (100 / (i+1)) + (increment * (i+1));
			//Debug.Log(levelMan.diff + " " + probs[i]);
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
