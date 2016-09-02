using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreMan : MonoBehaviour {

	public Text scoreDisplayer;
	public Text honorDisplayer;
	public GameObject scoreTag;
	public GameObject honorTagPos;
	public GameObject honorTagNeg;
	public float fadeTime = 0.02f;
	public int score = 0;
	public int honor = 5;
	public int maxHonor = 5;

	void Start () {

		scoreDisplayer.text += score;
		honorDisplayer.text += honor;
	}

	public void HonorAndScoreUpdater(EntityBehavior entB, bool fromPlayer)
	{
		if(fromPlayer)
		{
			honor += entB.honorValue;
			PrintScore(entB.honorValue, 0, entB.transform.position);
		}
		else
		{
			if(entB.tag == "npc")
			{
				score += entB.scoreValue;
				PrintScore(0, entB.scoreValue, entB.transform.position);
			}

			else
			{
				honor -= entB.honorValue;
				PrintScore(-entB.honorValue, 0, entB.transform.position);
			}
		}

		honor = Mathf.Clamp(honor, 0, maxHonor);

		scoreDisplayer.text = "Score: " +  score;
		honorDisplayer.text = "Honor: " + honor;

	}

	IEnumerator FadeMoveTag (GameObject tag)
    {
		Color newCol = tag.GetComponent<TextMesh>().color;
		float posY = tag.transform.position.y;

		while(newCol.a > 0)
        {
			posY += fadeTime;
			newCol = new Color(newCol.r, newCol.g, newCol.b, newCol.a - fadeTime);

			tag.GetComponent<TextMesh>().color = newCol;
			tag.transform.position = new Vector3(tag.transform.position.x, posY, tag.transform.position.z);
			
            yield return null;
        }

        Destroy(tag);
		StopCoroutine("FadeMoveTag");
    }

	void PrintScore(int honor, int score, Vector3 entPos)
	{
		GameObject tag;
		Color color; 
		GameObject tagToDisplay;
		Vector3 pos = new Vector3(entPos.x, entPos.y + 0.2f, -1);

		if(honor != 0)
		{
			tag = honor < 0 ? honorTagNeg : honorTagPos;
			tagToDisplay = Instantiate(tag, pos, Quaternion.identity) as GameObject;
		}
		else
		{
			tag = scoreTag;
			color = score > 0 ? Color.green : Color.red;
			string appendToTag = score > 0 ? "+" : "";
			tagToDisplay = Instantiate(tag, pos, Quaternion.identity) as GameObject;
			tagToDisplay.GetComponent<TextMesh>().text = appendToTag + score.ToString();
			tagToDisplay.GetComponent<TextMesh>().color = color;
		}

		StartCoroutine(FadeMoveTag(tagToDisplay));
	}


}