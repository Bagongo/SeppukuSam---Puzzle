using UnityEngine;
using System.Collections;

public class Knife : EntityBehavior {

	public bool hasHit = false;

	private int[] targetPos;
	private PlayerBehavior player;

	void Start()
	{
		player = (PlayerBehavior)FindObjectOfType(typeof(PlayerBehavior));
		//GetComponent<SpriteRenderer>().flipX = Random.value > 0.5 ? true : false;
	}

	public void LaunchKnife(int[] target)
	{
		targetPos = target;
		Vector3 targetCoord = grid[targetPos[0], targetPos[1]].transform.position;		
		StartCoroutine(SmoothMovement(targetCoord));
	}

	public override void FinalizeMovement()
	{
		transform.position = grid[targetPos[0], targetPos[1]].transform.position;
		items[targetPos[0], targetPos[1]] = this.gameObject;
		
		if(boardMan.entities[targetPos[0], targetPos[1]] != null)
			player.AttackWithKnife(targetPos);

		hasHit = true;
	}

	public void StartPopUpAnim()
	{
		StartCoroutine(PopUpAnimation());
    }

	IEnumerator PopUpAnimation()
	{
		SpriteRenderer sptrRend = GetComponent<SpriteRenderer>();
		sptrRend.sortingLayerName = "Forefront";
		Color col = sprtRend.color;
		float opacity = sprtRend.color.a;
		Vector3 startPos = transform.position;
		float scaleFactor = transform.localScale.x; 

		while(opacity > 0.25f)
		{
			float newPosY = transform.position.y + 0.075f;
			transform.position = new Vector3(transform.position.x, newPosY, transform.position.z);
			scaleFactor += 0.05f;
			transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
			opacity -= 0.03f;
			sprtRend.color = new Color(col.r, col.g, col.g, opacity);
			yield return null; 
		}

		Destroy(this.gameObject);	
	}
}
