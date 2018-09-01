using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour 
{
	[HideInInspector] public static GamePlayController instance;
	
	[SerializeField] private GameObject pausePanel, gameOverPanel;
	[SerializeField] private GameObject pauseButton, jumpButton;
	[SerializeField] private Text scoreText, lifeText, finalScoreText, deathMessageText;
	[SerializeField] private AudioClip[] _audioClips = new AudioClip[6];

	//private GameObject Player;
	private bool isPaused;
	private AudioSource _audioSource;
	private bool willPlayMusic = true;

	// Inits /////////////////////////////////////

	private void Awake()
	{
		MakeInstance();
		isPaused = false;
		_audioSource = GetComponent<AudioSource>();
		HandleGlassSound();
	}

	private void MakeInstance()
	{
		if (instance == null)
		{
			instance = this;
		}
	}


	// Inits /////////////////////////////////////

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && !isPaused && Player.instance.alive)
		{
			if (! UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
			{
				Vector2 pos = Input.mousePosition;
				Player.instance.ShootStar(pos); 
			}
		}
	}

	// Button Methods ////////////////////////////

	public void Jump()
	{
		Player.instance.PlayerJump();
	}

	public void PauseGame()
	{
		Time.timeScale = 0.0f;
		pausePanel.SetActive(true);
		isPaused = true;
	}

	public void ResumeGame()
	{
		Time.timeScale = 1.0f;
		pausePanel.SetActive(false);
		isPaused = false;
	}

	public void QuitGame()
	{
		isPaused = false;
		Time.timeScale = 1.0f;
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
	}

	public void RestartGame()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
	}

	// Button Methods ////////////////////////////

	// UI Update /////////////////////////////////
	public void SetScore(int score)
	{
		scoreText.text = "Score: " + score;
	}

	public void SetLife(int lives)
	{
		lifeText.text = " " + lives;
	}


	// UI Update /////////////////////////////////

	// Game Play /////////////////////////////////

	public void HandleGlassSound()
	{
		if (GamerPrefs.GetMusicOn() == 1)
		{
			willPlayMusic = true;
		}
		else
		{
			willPlayMusic = false;
		}
	}

	public void GameOver(string message)
	{
		deathMessageText.text = message;
		pauseButton.SetActive(false);
		jumpButton.SetActive(false);
		ShowAdd(); 
		gameOverPanel.SetActive(true);
	}

	private static void ShowAdd()
	{
		float addDecider = Random.Range(1, 99);
		if (addDecider > 60)
		{
			if (UnityEngine.Advertisements.Advertisement.IsReady())
			{
				UnityEngine.Advertisements.Advertisement.Show();
			}
		}
	}

	public void SetFinalScore(int score)
	{
		if (score < GamerPrefs.GetHighScore())
		{
			finalScoreText.text = "Score: " + score;
		}
		else
		{
			// TODO: Maybe play an audio High Score
			finalScoreText.text = "High Score: " + score;
			GamerPrefs.SetHighScore(score);
		}
	}

	public void PlayGlassBreakSound()
	{
		if (!_audioSource.isPlaying && willPlayMusic)
		{
			int track = (int)Mathf.Floor(Random.Range(0, _audioClips.Length));
			_audioSource.clip = _audioClips[track];
			_audioSource.Play(); 
		}
	}

	// Game Play /////////////////////////////////

}
