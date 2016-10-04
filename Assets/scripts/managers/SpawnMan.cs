using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpawnMan : MonoBehaviour {

	public GameObject[] npcsToAdd;
	public GameObject[] enemiesToAdd;
	public List<GameObject> npcsPool;
	public List<GameObject> enemiesPool;
	public int startingNpcsNmr;
	public int startinEnemiesNmr;
	public int maxNpcXRow;
	public float[] npcsRowDimProbs;
	public float[] npcsPoolingProbs;
	public float[] enemiesPoolingProbs;
	public int gridW;
	public int gridH;

	private BoardMan boardMan;
	private LevelMan levelMan;
	private TurnMan turnMan;


	void Awake()
	{
		turnMan = (TurnMan)FindObjectOfType(typeof(TurnMan));
		levelMan = (LevelMan)FindObjectOfType(typeof(LevelMan)); 
		boardMan = (BoardMan)FindObjectOfType(typeof(BoardMan));

		gridW = boardMan.gridW;
		gridH = boardMan.gridH;
	}

	void Start () {

		InitializePoolsAndValues();
		PopulatePools();
		PopulateBoard();
		boardMan.TakeSnapshot();
	}

	public void InitializePoolsAndValues()
	{
		npcsPool.Clear();
		enemiesPool.Clear();
		maxNpcXRow = gridW / 2;
	}

	public void PopulatePools()
	{			 
		for(int i=0; i < startingNpcsNmr; i++)
			npcsPool.Add(npcsToAdd[i]);	

		for(int i=0; i < startinEnemiesNmr; i++)
			enemiesPool.Add(enemiesToAdd[i]);
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

	public void SpawnNpcsRow(int yPos)
	{
		npcsRowDimProbs = CreateProbs(maxNpcXRow, turnMan.turnNmr);
		int npcQuant = Mathf.Clamp(Choose(npcsRowDimProbs) + 1, 1, maxNpcXRow);
		List<int> availableSlots = new List<int>();

		for(int i=0; i < gridW; i++)
			availableSlots.Add(i);

		for(int i=1; i <= npcQuant; i++)
		{
			npcsPoolingProbs = CreateProbs(npcsPool.Count, levelMan.Diff);
			int idx = Choose(npcsPoolingProbs);
			GameObject npc = npcsPool[idx];
			int xPos = availableSlots[Random.Range(0, availableSlots.Count)];

			if(boardMan.entities[xPos, yPos - 1] == null)
			{
				boardMan.InstantiateSingleEntity(npc, new int[]{xPos, yPos});
			}	

			availableSlots.Remove(xPos);
		}	
	}

	public void SpawnEnemyRow(int yPos)
	{
		enemiesPoolingProbs = CreateProbs(enemiesPool.Count, levelMan.Diff);
		int idx = Choose(enemiesPoolingProbs);
		GameObject enemy = enemiesPool[idx];
		int xPos = Random.Range(0, gridW - 1);
		boardMan.InstantiateSingleEntity(enemy, new int[]{xPos, yPos});
	}

	float[] CreateProbs (int dim, float increment)
	{
		float[] probs = new float[dim];

		for(int i=0; i < probs.Length; i++)
			probs[i] = 1000/(i+1) + (increment * i);

		return probs;
	}

	int Choose (float[] probs) 
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