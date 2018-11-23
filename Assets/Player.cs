using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	private int health = 100;
	public float maxSpeed = 5.0f;
	public GameObject player;
	private Rigidbody2D playerRB;

	private Animator playerAnim;


	public GameObject beamObject;
	public GameObject beamCharge1Object;
	public GameObject beamCharge2Object;
	public Transform beamSpawnPoint;

	public bool faceRight = true;

	public bool charge = false;
	public bool shoot = false;
	public bool running = false;
	public bool jump = false;
	public bool screwjumping = false;
	public bool jumping = false;
	public bool falling = false;
	public bool isGrounded = false;
	public Transform groundCheck;
	private float groundRadius = .5f;
	private float chargeTimer = 0;
	public LayerMask whatIsGround;

	private bool invincible;


	// Use this for initialization
	void Start()
	{
		player.transform.Translate(0.0f, 0.0f, 0.0f);
		playerRB = player.GetComponent<Rigidbody2D>();
		playerAnim = player.GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		jump = Input.GetKeyDown(KeyCode.C);
		running = Input.GetKey(KeyCode.LeftShift);
		falling = playerRB.velocity.y < -.4f;
		jumping = playerRB.velocity.y > 0f;
		shoot = Input.GetKeyDown(KeyCode.X);
		charge = Input.GetKey(KeyCode.X);


		// Animations
		playerAnim.SetBool("Falling", falling);
	}

	void FixedUpdate()
	{
		float move = Input.GetAxis("Horizontal");
		playerRB.velocity = new Vector2(move * maxSpeed, playerRB.velocity.y);

		if (isGrounded)
		{
			falling = false;
		}

		if (health > 0)
			// Update Health
			GameObject.Find("Canvas").transform.Find("Health").gameObject.GetComponent<Text>().text = health.ToString();
		else
			Die();

		// ScrewJumping
		if (!jumping)
		{
			screwjumping = false;
			playerAnim.ResetTrigger("ScrewJump");
		}
		// Walking animation
		playerAnim.SetFloat("Speed", Mathf.Abs(move));

		// Check movement and direciton
		if (move > 0 && !faceRight) flip();
		else if (move < 0 && faceRight) flip();

		// Check if on ground
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		playerAnim.SetBool("onGround", isGrounded);

		// Check if running
		if (running)
		{
			if (isGrounded)
			{
				playerAnim.speed = 3.25f;
				maxSpeed = 7.0f;
			}
		}
		else
		{
			playerAnim.speed = 1.75f;
			maxSpeed = 5.0f;
		}

		// Check if jumping
		if (jump && running && isGrounded && playerRB.velocity.y < .4f)
		{
			playerRB.velocity = (new Vector2(playerRB.velocity.x, 7f));
			playerAnim.SetTrigger("ScrewJump");
			screwjumping = true;
		}
		else if (jump && isGrounded && playerRB.velocity.y < .4f)
		{
			playerRB.velocity = (new Vector2(playerRB.velocity.x, 7f));
			playerAnim.SetTrigger("VertJump");
		}

		// Check if shooting

		if (shoot && (running || move != 0) && !screwjumping && !jumping)
		{
			playerAnim.SetTrigger("Shoot");
			fire(0);
		}
		else if (shoot && !screwjumping)
		{
			playerAnim.SetTrigger("Shoot");
			fire(0);
		}

		if (charge)
		{
			chargeTimer += Time.deltaTime;
		}
		else
		{
			chargeTimer = 0f;
		}
		if (Input.GetKeyUp(KeyCode.X))
		{
			if (chargeTimer > .1f)
			{
				fire(chargeTimer);
				chargeTimer = 0f;
			}
		}
		playerAnim.SetFloat("Charging", chargeTimer);

	}

	public Vector3 getPosition()
	{
		return player.GetComponent<Rigidbody2D>().position;
	}

	public void fire(float time)
	{

		waitFire();
		var shootingDirection = beamSpawnPoint.position.x;
		GameObject beam;
		if (time > 3f)
		{
			beam = Instantiate(beamCharge2Object, new Vector3(shootingDirection, beamSpawnPoint.position.y, beamSpawnPoint.position.z), Quaternion.identity) as GameObject;
		}
		else if (time > 2f)
		{
			beam = Instantiate(beamCharge1Object, new Vector3(shootingDirection, beamSpawnPoint.position.y, beamSpawnPoint.position.z), Quaternion.identity) as GameObject;
		}
		else
		{
			beam = Instantiate(beamObject, new Vector3(shootingDirection, beamSpawnPoint.position.y, beamSpawnPoint.position.z), Quaternion.identity) as GameObject;
		}

		// Check direction
		if (faceRight)
		{
			beam.GetComponent<SpriteRenderer>().flipX = false;
		}
		else
		{
			beam.GetComponent<SpriteRenderer>().flipX = true;
		}
		var beamRigidBody = beam.GetComponent<Rigidbody2D>();
		if (faceRight)
		{
			beamRigidBody.velocity = Quaternion.Euler(0, 0, 0) * Vector3.right * 10;
		}
		else
		{
			beamRigidBody.velocity = Quaternion.Euler(0, 0, 0) * Vector3.left * 10;
		}
		Destroy(beam, 2f);
	}

	IEnumerator waitFire()
	{
		yield return new WaitForSeconds(0.6f);
	}

	IEnumerator spaceOutShots()
	{
		yield return new WaitForSeconds(2f);
	}

	void flip()
	{
		faceRight = !faceRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		player.transform.localScale = theScale;

	}

	public void hitByEnemy(bool enemyDirection, int damageAmount)
	{
		if (!invincible)
		{
			playerAnim.SetTrigger("HitByEnemy");
			if (enemyDirection == true)
			{
				playerRB.AddForce(new Vector2(1200f, 100f));
			}
			else
			{
				playerRB.AddForce(new Vector2(-1200f, 100f));
			}
			// turn collision off
			invincible = true;
			Damage(damageAmount);
			Physics2D.IgnoreLayerCollision(9, 11, true);
			StartCoroutine(invincibleTimer());
			StopCoroutine(invincibleTimer());
		}
	}

	public void Damage(int damageAmount)
	{
		health -= damageAmount;
		StartCoroutine(flicker());
		StopCoroutine(flicker());
	}

	IEnumerator flicker()
	{
		for (float i = 0; i < .3; i += Time.deltaTime)
		{
			player.GetComponent<SpriteRenderer>().enabled = !player.GetComponent<SpriteRenderer>().enabled;
			yield return new WaitForSeconds(.1f);
		}
		player.GetComponent<SpriteRenderer>().enabled = true;
	}

	private void Die()
	{
		Debug.Log("Dead");
	}

	private IEnumerator invincibleTimer()
	{
		yield return new WaitForSeconds(2f);
		// turn collision back on
		Physics2D.IgnoreLayerCollision(9, 11, false);
		invincible = false;
	}
}
