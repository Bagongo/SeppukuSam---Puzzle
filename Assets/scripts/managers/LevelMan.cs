using UnityEngine;
using System.Collections;

public class LevelMan : MonoBehaviour {

	public int currentLvl = 1;
	public SpawnMan spawnMan;
	public int npcRowSizeTH;
	public int npcsPoolTH;
	public int enemiesPoolTH;

	public int Diff
	{
		get
		{
			return currentLvl;
		}
	}

	void Awake()
	{
		spawnMan = (SpawnMan)FindObjectOfType(typeof(SpawnMan));
	}

	public void NextLevel()
	{
		currentLvl++;
		DifficultyIncreaser();
	}

	public void DifficultyIncreaser()
	{
		if(Diff % npcRowSizeTH == 0)
		spawnMan.maxNpcXRow = Mathf.Clamp(spawnMan.maxNpcXRow + 1, 1, spawnMan.gridW - 1);

		if(Diff % npcsPoolTH == 0)
		{
			if(spawnMan.npcsPool.Count < spawnMan.npcsToAdd.Length)
				spawnMan.npcsPool.Add(spawnMan.npcsToAdd[spawnMan.npcsPool.Count]);						 
		}

		if(Diff % enemiesPoolTH == 0)
		{
			if(spawnMan.enemiesPool.Count < spawnMan.enemiesToAdd.Length)
				spawnMan.enemiesPool.Add(spawnMan.enemiesToAdd[spawnMan.enemiesPool.Count]);						 
		}			
	}
		

}