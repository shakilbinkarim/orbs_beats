using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBullet : MonoBehaviour 
{

	[SerializeField] private float speed = 8.0f;
	[SerializeField] private float fixedHeight = 10.0f;
	[SerializeField] private ParticleSystem particleSystem;

	private float requiredWidth;
	private bool willMove = false;
	private GameObject camera;

	public void SetDestinitionPos(Vector2 pos)
	{
		camera = GameObject.Find("Main Camera") as GameObject;
		Vector2 worldPos = camera.GetComponent<Camera>().ScreenToWorldPoint(pos);
		if (worldPos.y < gameObject.transform.position.y)
		{
			fixedHeight = (-1 * fixedHeight);
		}
		float ratio = (worldPos.x - transform.position.x) / (worldPos.y - transform.position.y);
		requiredWidth = ratio * fixedHeight;
	}

	public void SetWillMove(bool move)
	{
		willMove = move;
	}

	// Update is called once per frame
	void Update ()
	{
		if (willMove)
		{
			float step = speed * Time.deltaTime;
			Vector3 targetPos = new Vector3(requiredWidth, fixedHeight, transform.position.z);
			transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "Platform") // TODO: Should use tags instead
		{
			fixedHeight = (-1 * fixedHeight);
		}
		else if (collision.gameObject.tag == "Nature")
		{
			Player.instance.PerformDeathFormallities("Avoid shooting green orbs to not loose lives");
			collision.gameObject.GetComponent<GreenOrb>().Burn();
			DestroySelf();
		}
		else if (collision.gameObject.tag == "Lightning")
		{
			collision.gameObject.GetComponent<YellowOrb>().Discharge();
			Player.instance.IncreaseLife();
			Player.instance.animator.SetTrigger("Life");
			DestroySelf();
		}
		else if (collision.gameObject.tag == "Water")
		{
			Player.instance.IncreaseScore();
			collision.gameObject.GetComponent<BlueOrb>().Vaporize();
			DestroySelf();
		}
		else if (collision.gameObject.tag == "StarDestroyer")
		{
			Destroy(gameObject);
		}
	}

	private void DestroySelf()
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
