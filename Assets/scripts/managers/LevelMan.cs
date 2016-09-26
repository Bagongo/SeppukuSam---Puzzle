using UnityEngine;
using System.Collections;

public class LevelMan : MonoBehaviour {

	public int currentLvl = 1;
	public SpawnMan spawnMan;

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
		spawnMan.DifficultyIncreaser();
	}
		

}