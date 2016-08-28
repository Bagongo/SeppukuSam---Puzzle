using UnityEngine;
using System.Collections;

public class OldNpc : RegNpc {

	public bool hasMovedPrev = false;

	public override void ElaborateMove()
	{
		if(hasMovedPrev)
		{
			hasMovedPrev = false;
			SortNextMove();
			return;
		}
		else
		{
			hasMovedPrev = true;
			base.ElaborateMove();
		}
	}
	
}
