using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monitor : MonoBehaviour {

	public int level;
	public int turn;
	public LevelMan levelMan;
	public SpawnMan spawnMan;
	public TurnMan turnMan;

	void Awake()
	{
		spawnMan = (SpawnMan)FindObjectOfType(typeof(SpawnMan));
		turnMan = (TurnMan)FindObjectOfType(typeof(TurnMan));
		levelMan = (LevelMan)FindObjectOfType(typeof(LevelMan));
	}

	void Start()
	{
		UpdateMonitor();
	}

	public void UpdateMonitor()
	{
		level = levelMan.currentLvl;
		turn = turnMan.turnNmr;
	}

	public string ParseProbsArray (float[] probsArray, List<GameObject> pool)
	{
		string probsString = "";
		float total = 0;

		for(int i=0; i < probsArray.Length; i++)
			total += probsArray[i]; 

		for(int i=0; i < probsArray.Length; i++)
		{
//			if(pool.Count > 0)
//				probsString += pool[i].name + ":";

			probsString += " " + ((probsArray[i]/total) * 100).ToString("0.0") + "% ";
		}

		return probsString;
	}

	public void UpdateLevel()
	{
		levelMan.currentLvl = 1;
		spawnMan.InitializePoolsAndValues();
		spawnMan.PopulatePools();
		
		for(int i=1; i < level; i++)
			levelMan.NextLevel();
	}

}
