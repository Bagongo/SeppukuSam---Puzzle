using UnityEngine;
using UnityEngine.UI;
using System.Collections; 

public class ActionButton : MonoBehaviour {

	public PlayerBehavior player;
	public PlayerMoves move;

	private Button btn;

	void Start()
	{
		player = FindObjectOfType<PlayerBehavior>();
		btn = GetComponent<Button>();
		btn.onClick.AddListener(TriggerFoo);
	}

	public void TriggerFoo()
	{
		player = FindObjectOfType<PlayerBehavior>(); //find better solution....

		if(!player.playerBlocked)
		{
			if(move == PlayerMoves.knife && !player.hasKnife)
				Debug.Log("Doesn't have knife");
			else if(move == PlayerMoves.knife && player.hasKnife)
			{
				player.hasKnife = false;
				player.MovesCollector(move);
			}
			else
				player.MovesCollector(move);
		}
	}		
}
