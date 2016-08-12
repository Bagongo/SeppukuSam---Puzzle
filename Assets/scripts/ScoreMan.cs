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
	
	public bool HonorAndScoreUpdater(int sPoints, int hPoints)
	{
		score += sPoints;
		honor += hPoints;

		scoreDisplayer.text = "Score: " +  score;
		honorDisplayer.text = "Honor: " + honor;

		if(honor > 0)
			return true;
		else 
			return false;
	}
}
