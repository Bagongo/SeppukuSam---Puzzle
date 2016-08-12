using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreMan : MonoBehaviour {

	public Text scoreDisplayer;
	public Text honorDisplayer;

	public int score = 0;
	public int honor = 5;

	void Start () {

		scoreDisplayer.text += score;
		honorDisplayer.text += honor;
	}

	public void HonorAndScoreUpdater(EntityBehavior entB, bool fromPlayer)
	{
		if(fromPlayer)
		{
			if(entB.tag == "enemy")
				score += entB.scoreValue;

			honor += entB.honorValue;
		}
		else
		{
			if(entB.tag == "npc")
				score += entB.scoreValue;
			else
				honor -= entB.honorValue;
		}

		scoreDisplayer.text = "Score: " +  score;
		honorDisplayer.text = "Honor: " + honor;

	}
}