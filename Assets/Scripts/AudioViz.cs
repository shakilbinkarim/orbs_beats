using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioViz : MonoBehaviour 
{

	public AudioClip[] _audioClips = new AudioClip[6];
	[HideInInspector] public static float[] _samples = new float[512];
	[HideInInspector] public static float[] _frequencyBand = new float[8];

	private AudioSource _audioSource;
	private int previousIndex = 0;
	private bool willPlayMusic = true;
	private int track;
	
	public void HandleGameBGMusic()
	{
		_audioSource = GetComponent<AudioSource>();
		_audioSource.loop = true;
		if (GamerPrefs.GetMusicOn() == 1)
		{
			willPlayMusic = true;
		}
		else
		{
			willPlayMusic = false;
		}
		HandleBGMusicStart();
	}

	public void HandleBGMusicStart()
	{
		AudioSource audioSource = GetComponent<AudioSource>();
		if (willPlayMusic)
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

	// Use this for initialization
	private void Start ()
	{
		HandleGameBGMusic();
		StartCoroutine(playMusics());
	}

	IEnumerator playMusics()
	{
		while (willPlayMusic)
		{
			track = (int)Mathf.Floor(Random.Range(0, _audioClips.Length));
			while (track == previousIndex)
			{
				track = (int)Mathf.Floor(UnityEngine.Random.Range(0, _audioClips.Length));
			}
			_audioSource.clip = _audioClips[track];
			_audioSource.Play(); 
			previousIndex = track;
			yield return new WaitForSeconds(_audioSource.clip.length); 
		}
	}

	// Update is called once per frame
	void Update ()
	{
		GetSpectrumAudioSource();
		MakeFrequencyBands();
	}

	private static void MakeFrequencyBands()
	{
		int count = 0;
		for (int i = 0; i < 8; i++)
		{
			float average = 0;
			int sampleCount = (int)Mathf.Pow(2, i) * 2;
			if (i == 7)
			{
				sampleCount += 2;
			}
			for (int j = 0; j < sampleCount; j++)
			{
				average += _samples[count] * (count + 1);
				count++;
			}
			average /= count;
			_frequencyBand[i] = average * 10;
		}
	}

	private void GetSpectrumAudioSource()
	{
		_audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
	}

}
