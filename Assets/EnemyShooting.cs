using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
	private GameObject Player;
	public Transform beamSpawnPoint;

	public GameObject beamObject;

	private bool facingRight = false;

	private bool shoot = false;

	// Use this for initialization
	void Start()
	{
		Player = GameObject.Find("Player");
		StartCoroutine(fireTime());
	}


	void Update()
	{
	}
	// Update is called once per frame

	void FixedUpdate()
	{
		facingRight = transform.position.x - Player.transform.position.x < 0f;

		if (facingRight)
		{
			Vector3 theScale = transform.localScale;
			theScale.x = Mathf.Abs(theScale.x) * -1;
			transform.localScale = theScale;
		}
		else
		{
			Vector3 theScale = transform.localScale;
			theScale.x = Mathf.Abs(theScale.x);
			transform.localScale = theScale;
		}

		if (shoot)
		{
			fire();
		}

	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag.Equals("Player"))
		{
			Player.GetComponent<Player>().hitByEnemy(facingRight, 10);
		}
	}

	public IEnumerator fireTime()
	{
		while ((Mathf.Abs(transform.position.x - Player.transform.position.x) < 6f))
		{
			shoot = false;
			yield return new WaitForSeconds(2f);
			shoot = true;
		}
	}

	public void fire()
	{
		Debug.Log("shooting");
		var shootingDirection = beamSpawnPoint.position.x;
		GameObject beam = Instantiate(beamObject, new Vector3(shootingDirection, beamSpawnPoint.position.y, beamSpawnPoint.position.z), Quaternion.identity) as GameObject;
		beam.GetComponent<SpriteRenderer>().color = Color.red;
		gameObject.GetComponent<Animator>().SetTrigger("Shoot");

		// Check direction
		if (facingRight)
		{
			beam.GetComponent<SpriteRenderer>().flipX = false;
		}
		else
		{
			beam.GetComponent<SpriteRenderer>().flipX = true;
		}
		var beamRigidBody = beam.GetComponent<Rigidbody2D>();
		if (facingRight)
		{
			beamRigidBody.velocity = Quaternion.Euler(0, 0, 0) * Vector3.right * 10;
		}
		else
		{
			beamRigidBody.velocity = Quaternion.Euler(0, 0, 0) * Vector3.left * 10;
		}
		Destroy(beam, 1f);
	}
}
