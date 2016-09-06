using UnityEngine;
using System.Collections;

public class BoardMan : MonoBehaviour {

	public GameObject slot;
	public int gridW ;
	public int gridH ;
	public GameObject[] npcs;
	public GameObject[] enemies;
	public GameObject playerPrefab;
	public GameObject knifePrefab;
	public ScoreMan scoreMan;
	public int[] playerPos = {3,0}; 
	public GameObject [,] grid; 
	public GameObject[,] entities;
	public GameObject[,] items;
	public GameObject player; 

	private GameObject knife;

	void Awake(){

		grid = new GameObject[gridW, gridH];
		entities = new GameObject[gridW, gridH];
		items = new GameObject[gridW, gridH];

		for(int x=0; x<gridW; x++)
		{
			for(int y=0; y<gridH; y++)
			{
				GameObject gridSlot = (GameObject)Instantiate(slot, new Vector3(x, y, 0), Quaternion.identity);
				gridSlot.transform.parent = transform;
				grid[x,y]=gridSlot;
			}
		}

		transform.position = new Vector3(transform.position.x - grid.GetLength(0)/2, transform.position.y - grid.GetLength(1)/2, 0);
	}

	void Start(){

		PopulateBoard();
	}

	void PopulateBoard()
	{
		player = Instantiate(playerPrefab, grid[playerPos[0], playerPos[1]].transform.position, Quaternion.identity) as GameObject;

		//reactivate in case player row consider player as unity (null and recreate on each move)........
		//entities[playerPos[0], playerPos[1]] = player;

		player.GetComponent<PlayerBehavior>().currentPos = playerPos;

		for(int y=1; y<gridH; y++)
		{
			if(y==4 || y==6)
				SpawnRow(Row.npc, y);
			else if(y== gridH-1)
				SpawnRow(Row.enemy, y);
			else 
				SpawnRow(Row.empty, y);
		}
	}

	public void SpawnRow(Row whatTospawn, int y) //consider delegating type of spawnable choice to other method or brand new spawner in game element...
	{
		Row whatToSpawn = whatTospawn;
		int yPos = Mathf.Clamp(y, 1, gridH-1);

		int quantityTofill = 0;
		int quantityFilled = 0;

		GameObject[] spawnables = null;

		switch(whatToSpawn)
		{
			case Row.empty:
				return;
			case Row.npc:
				spawnables = npcs;
				quantityTofill = Random.Range(1,4); //max 4????
				break;
			case Row.enemy:
				spawnables = enemies;
				quantityTofill = 1;
				break;
		}

		while(quantityTofill > quantityFilled)
		{
			GameObject toSpawn = spawnables[Random.Range(0, spawnables.Length)];
			int xPos = Random.Range(0, gridW);

			if(entities[xPos, yPos] == null && entities[xPos, yPos-1] == null)
			{	
				InstantiateSingleEntity(toSpawn, new int[]{xPos, yPos});
				quantityFilled++ ;
			}
		}
	}

	public GameObject InstantiateSingleEntity(GameObject toSpawn, int[] atPos)
	{
		Vector3 spawnPos = grid[atPos[0], atPos[1]].transform.position;
		GameObject newEntity = Instantiate(toSpawn, spawnPos, Quaternion.identity) as GameObject;
		EntityBehavior entB = newEntity.GetComponent<EntityBehavior>();
		entB.currentPos = new int[]{atPos[0], atPos[1]};
		entities[atPos[0], atPos[1]] = newEntity;

		return newEntity;
	}

	public void UpdateGrid(int[]formerPos, int[]newPos)
	{
		GameObject ent = entities[formerPos[0], formerPos[1]];
		entities[formerPos[0], formerPos[1]] = null;
		entities[newPos[0], newPos[1]] = ent;
	}

	public void RemoveFromGrid(int[] pos)
	{
		if(entities[pos[0], pos[1]] != null)
			entities[pos[0], pos[1]] = null;												
	}

	float Choose (float[] probs) 
	{
        float total = 0;

        foreach (float elem in probs) {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i= 0; i < probs.Length; i++) {
            if (randomPoint < probs[i]) {
                return i;
            }
            else {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }

}

