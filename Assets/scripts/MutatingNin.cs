using UnityEngine;
using System.Collections;

public class MutatingNin : RegEnemy {

	void Start () {

		StartCoroutine(Appear());
	}

	IEnumerator Appear()
	{
		Debug.Log(name + "-> appearing...");

		Color color = GetComponent<SpriteRenderer>().color;
		float tempOpacity = color.a;

		while(tempOpacity < 1)
		{
			tempOpacity += speed * Time.deltaTime;
			color = new Color(color.r, color.g, color.b, tempOpacity);
			GetComponent<SpriteRenderer>().color = color;

			yield return null;
		}
	}


}
