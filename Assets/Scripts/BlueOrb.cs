using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueOrb : MonoBehaviour 
{
	[SerializeField] private ParticleSystem particleSystem;

	public void Vaporize()
	{
		GamePlayController.instance.PlayGlassBreakSound();
		ShowParticles(transform.position, particleSystem);
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
