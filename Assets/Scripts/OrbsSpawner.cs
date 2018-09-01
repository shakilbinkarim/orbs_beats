using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbsSpawner : MonoBehaviour 
{

	[SerializeField] private GameObject[] orbs;
	[SerializeField] private float offset = 0.5f;
	[SerializeField] private float timeBetweenOrbs = 3.0f;

	private float minX, maxX;
	private bool keepSpawning = true;
	private int previousIndex = 0;

	private void Awake()
	{
		SetBounds();
	}

	/// <summary>
	/// Sets the values of minX and maxX according to player screen
	/// </summary>
	private void SetBounds()
	{
		Vector3 bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f));
		maxX = bounds.x - (offset * 2);
		minX = -bounds.x + (offset * 4);
	}

	// Use this for initialization
	private void Start()
	{
		UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
		StartCoroutine(SpawnOrbsAtIntervals(timeBetweenOrbs));
	}


	IEnumerator SpawnOrbsAtIntervals(float secondsBetweenSpawns)
	{
		// Repeat until keepSpawning == false or this GameObject is disabled/destroyed.
		while (keepSpawning)
		{
			// Put this coroutine to sleep until the next spawn time.
			yield return new WaitForSeconds(secondsBetweenSpawns);
			// Now it's time to spawn again.
			Spawn();
		}
	}

	private void Spawn()
	{
		int randomIndex = (int)Mathf.Floor(UnityEngine.Random.Range(0, orbs.Length));
		//while (randomIndex == previousIndex)
		//{
		//	randomIndex = (int)Mathf.Floor(UnityEngine.Random.Range(0, orbs.Length));
		//}
		//previousIndex = randomIndex;
		float xPos = UnityEngine.Random.Range(minX, maxX);
		Instantiate(orbs[randomIndex], new Vector3(xPos, transform.position.y, 0), Quaternion.identity);
	}

}
