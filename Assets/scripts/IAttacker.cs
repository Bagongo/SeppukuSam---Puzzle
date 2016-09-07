using UnityEngine;
using System.Collections;

public interface IAttacker{

	 void Attack(int[] targetPos);
	 void KillEntity(EntityBehavior entB);

}
