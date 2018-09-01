using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{

	[HideInInspector] public static Player instance;
	[HideInInspector] public bool alive;

	public Animator animator;

	[SerializeField] private float force = 80.0f;
	[SerializeField] private GameObject starBullet;
	[SerializeField] private ParticleSystem particleSystem;

	private bool touchingGround = false;
	private Rigidbody2D playerRigidBody;
	private int lifeCount, score;
	private string mizuMessage;
	private string lightningMessage;

	// Called Before Start
	private void Awake ()
	{
		MakeInstance();
	}

	private void MakeInstance()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	private void Start()
	{
		playerRigidBody = GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator>();
		alive = true;
		score = 0;
		lifeCount = 4;
		GamePlayController.instance.SetScore(score);
		GamePlayController.instance.SetLife(lifeCount);
		mizuMessage = "Comming in contact with blue orbs will kill you immedietely";
		lightningMessage = "Comming in contact with yellow orbs will reduce life by 1";
	}

	public void ShootStar(Vector2 mouseClickPos)
	{
		GameObject bullet = Instantiate(starBullet) as GameObject;
		bullet.transform.position = gameObject.transform.position;
		StarBullet bulletScript = bullet.GetComponent<StarBullet>();
		if (bulletScript != null)
		{
			bulletScript.SetDestinitionPos(mouseClickPos);
			bulletScript.SetWillMove(true);
		}
	}

	public void PlayerJump()
	{
		if (touchingGround)
		{
			touchingGround = false;
			playerRigidBody.AddForce(new Vector2(0.0f, force));
			Debug.Log("Jump"); 
		}
	}

	private void OnCollisionEnter2D(Collision2D collider)
	{
		if (collider.gameObject.name == "Platform")
		{
			touchingGround = true;
		}
		if (collider.gameObject.tag == "Nature")
		{
			GreenOrb greenOrb = collider.gameObject.GetComponent<GreenOrb>();
			if (greenOrb)
			{
				greenOrb.Burn();
			}
		}
		if (collider.gameObject.tag == "Water")
		{
			BlueOrb blueOrb = collider.gameObject.GetComponent<BlueOrb>();
			if (blueOrb)
			{
				blueOrb.Vaporize();
				animator.SetTrigger("Flash");
			}
			lifeCount = 1;
			PerformDeathFormallities(mizuMessage);
		}
		if (collider.gameObject.tag == "Lightning")
		{
			YellowOrb yellowOrb = collider.gameObject.GetComponent<YellowOrb>();
			if (yellowOrb)
			{
				yellowOrb.Discharge();
				animator.SetTrigger("Flash");
				PerformDeathFormallities(lightningMessage);
			}
		}
	}

	public void IncreaseScore()
	{
		// TODO: Play Sound
		score++;
		GamePlayController.instance.SetScore(score);
	}

	public void IncreaseLife()
	{
		// TODO: Play Sound
		if (lifeCount < 4)
		{
			lifeCount++;
			GamePlayController.instance.SetLife(lifeCount); 
		}
	}

	public void PerformDeathFormallities(string message)
	{
		lifeCount--;
		GamePlayController.instance.SetLife(lifeCount);
		if (lifeCount < 1)
		{
			KillPlayer(message);
		}
	}

	private void KillPlayer(string message)
	{
		alive = false;
		GamePlayController.instance.SetFinalScore(score);
		GamePlayController.instance.GameOver(message);
		// Play death particle fx
		ShowParticles(transform.position, particleSystem);
		gameObject.SetActive(false);
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
