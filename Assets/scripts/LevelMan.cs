using UnityEngine;
using System.Collections;

public class LevelMan : MonoBehaviour {

	public int currentLvl = 1;
	public int diff = 1;
	public SpawnMan spawnerMan;

	void Start () {
	
	}

	public void NextLevel()
	{
		currentLvl++;
		diff++;
		spawnerMan.EvaluatePoolAdding(diff);
	}
		

}
