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
		btn.onClick.AddListener(TriggerBtn);
	}

	public void TriggerBtn()
	{
		//player = FindObjectOfType<PlayerBehavior>(); //find better solution....

		if(player.movesCollected < player.nmbrOfMoves)
		{
			if(move == PlayerMoves.knife)
			{
				if(!player.hasKnife)
					Debug.Log("Doesn't have knife");
				else
				{
					player.hasKnife = false;
					player.MovesCollector(move);
				}
			}
			else
				player.MovesCollector(move);
		}
	}		
}
