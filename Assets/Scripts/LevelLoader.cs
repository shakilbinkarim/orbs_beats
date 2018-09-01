using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour 
{

	[SerializeField] private int sceneIndex;
	[SerializeField] private Text InstText;

	private string[] instructions = new string[9];

	private void Start()
	{
		ShowAnInstruction();
		StartCoroutine(LoadAsyncronously());
	}

	private void ShowAnInstruction()
	{
		PopulateInstructionsArray();
		int inst = (int)Mathf.Floor(Random.Range(0, instructions.Length));
		InstText.text = instructions[inst];
	}

	private void PopulateInstructionsArray()
	{
		instructions[0] = "Avoid shooting green orbs to not loose lives";
		instructions[1] = "Comming in contact with blue orbs will kill you immedietely";
		instructions[2] = "Shooting yellow orbs will give you a life";
		instructions[3] = "Shooting green orbs will reduce your lives by one";
		instructions[4] = "Comming in contact with yellow orbs will reduce lives by one";
		instructions[5] = "Shooting blue orbs will add to your sccore";
		instructions[6] = "Comming in contact with green orbs will have no effects";
		instructions[7] = "Press the pink jump button to jump";
		instructions[8] = "Jumping may not always prove to be useful";
	}

	IEnumerator LoadAsyncronously()
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
		while (!operation.isDone)
		{
			yield return null;
		}
	}

}
