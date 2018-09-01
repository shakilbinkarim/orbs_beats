using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatBar : MonoBehaviour 
{

	public int _band;
	public float _startScale, _scaleMultiplier;

	// Update is called once per frame
	void Update ()
	{
		transform.localScale = new Vector3(transform.localScale.x, (AudioViz._frequencyBand[_band] * _scaleMultiplier) + _startScale, transform.localScale.z);
	}

}
