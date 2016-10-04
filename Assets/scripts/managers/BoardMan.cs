using UnityEngine;
using System.Collections;

public class BoardMan : MonoBehaviour {

	public GameObject slot;
	public int gridW ;
	public int gridH ;
	public GameObject playerPrefab;
	public GameObject knifePrefab;
	public ScoreMan scoreMan;
	public int[] playerPos = {3,0}; 
	public GameObject [,] grid; 
	public GameObject[,] entities;
	public GameObject[,] items;
	public GameObject player; 

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

	void Start()
	{
		InstantiateSingleEntity(playerPrefab, playerPos);
	}

	public GameObject InstantiateSingleEntity(GameObject toSpawn, int[] atPos)
	{
		Vector3 spawnPos = grid[atPos[0], atPos[1]].transform.position;
		GameObject newEntity = Instantiate(toSpawn, spawnPos, Quaternion.identity) as GameObject;
		EntityBehavior entB = newEntity.GetComponent<EntityBehavior>();
		entB.currentPos = new int[]{atPos[0], atPos[1]};
		entities[atPos[0], atPos[1]] = newEntity;
		entB.GetComponent<SpriteRenderer>().sortingOrder = entB.currentPos[1];

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
}

