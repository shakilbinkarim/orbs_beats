using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenOrb : MonoBehaviour
{
	[SerializeField] private ParticleSystem particleSystem;
	
	public void Burn()
	{
		ShowParticles(transform.position, particleSystem);
		GamePlayController.instance.PlayGlassBreakSound();
		Destroy(gameObject);
	}
	private void ShowParticles(Vector3 position, ParticleSystem particle)
	{

		if (!particle)
		{
			return;
		}
		ParticleSystem ps = (ParticleSystem)Instantiate(particle);
		ps.transform.position = position;
		ps.Play();
		Destroy(ps.gameObject, ps.startLifetime);
	}

}
