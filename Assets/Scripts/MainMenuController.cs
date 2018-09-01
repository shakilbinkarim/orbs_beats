using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class MainMenuController : MonoBehaviour 
{

	private Text highScore, soundText;
	private bool willPlay = false;

	private void Start()
	{
		HandleHighScoreText();
		HandleMusicButtonText();
	}

	private void HandleMusicButtonText()
	{
		// 音楽
		int music = GamerPrefs.GetMusicOn(); // 悪い
		soundText = GameObject.Find("Sound Button").GetComponent<Text>();
		soundText.text = (music == 1) ? "Sound On" : "Sound Off";
		willPlay = (music == 1) ? true : false;
		HandleMainMenuBGMusic();
	}

	private void HandleHighScoreText()
	{
		// スコア
		int score = GamerPrefs.GetHighScore(); // 悪い
		highScore = GameObject.Find("High Score Text").GetComponent<Text>();
		highScore.text = "High Score: " + score;
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public void PlayGame()
	{
		SceneManager.LoadScene("Loading");
	}

	public void HandleSound()
	{
		int music = GamerPrefs.GetMusicOn();
		if (music == 1)
		{
			music = 0;
			GamerPrefs.SetMusicOn(0);
		}
		else
		{
			music = 1;
			GamerPrefs.SetMusicOn(1);
		}
		Text soundText = GameObject.Find("Sound Button").GetComponent<Text>();
		soundText.text = (music == 1) ? "Sound On" : "Sound Off";
		willPlay = (music == 1) ? true : false;
		HandleMainMenuBGMusic(); 
	}

	public void HandleMainMenuBGMusic()
	{
		AudioSource audioSource = GetComponent<AudioSource>();
		if (willPlay)
		{
			if (!audioSource.isPlaying)
			{
				audioSource.Play();
			}
		}
		else
		{
			if (audioSource.isPlaying)
			{
				audioSource.Stop();
			}
		}
	}

}
