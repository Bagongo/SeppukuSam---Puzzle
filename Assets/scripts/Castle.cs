using UnityEngine;
using System.Collections;

public class Castle : MonoBehaviour {

	public ScoreMan scoreMan;
	public TurnMan turnMan;

	void OnTriggerenter2D(Collider coll)
	{
		EntityBehavior entB = coll.GetComponent<EntityBehavior>();
		scoreMan.HonorAndScoreUpdater(entB, false);


		if(!turnMan.EvaluateContinuation())
			turnMan.GameOver();

		entB.EliminateEntity();
	}
}
