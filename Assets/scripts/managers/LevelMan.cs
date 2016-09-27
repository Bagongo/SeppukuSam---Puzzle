using UnityEngine;
using System.Collections;

public class LevelMan : MonoBehaviour {

	public int currentLvl = 1;
	public int npcRowSizeTH;
	public int npcsPoolTH;
	public int enemiesPoolTH;

	private SpawnMan spawnMan;
	private TurnMan turnMan;

	public float Diff
	{
		get
		{
			return currentLvl * 1.5f;
		}
	}

	void Awake()
	{
		spawnMan = (SpawnMan)FindObjectOfType(typeof(SpawnMan));
		turnMan = (TurnMan)FindObjectOfType(typeof(TurnMan));
	}

	public void NextLevel()
	{
		currentLvl++;
		DifficultyIncreaser();
	}

	public void DifficultyIncreaser()
	{
		turnMan.turnThreshold += 1;

		if(Diff % npcRowSizeTH == 0)
		spawnMan.maxNpcXRow = Mathf.Clamp(spawnMan.maxNpcXRow + 1, 1, spawnMan.gridW - 1);

		if((int)Diff % npcsPoolTH == 0)
		{
			if(spawnMan.npcsPool.Count < spawnMan.npcsToAdd.Length)
				spawnMan.npcsPool.Add(spawnMan.npcsToAdd[spawnMan.npcsPool.Count]);						 
		}

		if((int)Diff % enemiesPoolTH == 0)
		{
			if(spawnMan.enemiesPool.Count < spawnMan.enemiesToAdd.Length)
				spawnMan.enemiesPool.Add(spawnMan.enemiesToAdd[spawnMan.enemiesPool.Count]);						 
		}			
	}
		

}