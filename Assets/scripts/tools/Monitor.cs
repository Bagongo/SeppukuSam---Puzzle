using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monitor : MonoBehaviour {

	public int level;
	public int turn;
	public int turnTreshold;
	public int maxNpcXRow;
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
		turnTreshold = turnMan.turnThreshold;

		maxNpcXRow = spawnMan.maxNpcXRow;
	}

	public string ParseProbsArray(float[] probsArray)
	{
		string probsString = "";

		for(int i=0; i < probsArray.Length; i++)
			probsString += " " + probsArray[i];

		return probsString;
	}

	public void UpdateLevel()
	{
		levelMan.currentLvl = 1;
		spawnMan.InitializePoolsAndValues();
		
		for(int i=1; i < level; i++)
			levelMan.NextLevel();
	}
}
