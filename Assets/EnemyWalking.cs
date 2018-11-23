using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalking : MonoBehaviour
{
	private GameObject Player;

	private bool facingRight = false;

	private bool follow = false;
	// Use this for initialization
	void Start()
	{
		Player = GameObject.Find("Player");
	}


	void Update()
	{

	}
	// Update is called once per frame

	void FixedUpdate()
	{
		if (Mathf.Abs(transform.position.x - Player.transform.position.x) < 6f || follow == true)
		{
			follow = true;
			float speed = 1.0f;
			facingRight = transform.position.x - Player.transform.position.x < 0f;
			if (facingRight)
			{
				GetComponent<SpriteRenderer>().flipX = false;
				GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
			}
			else
			{
				GetComponent<SpriteRenderer>().flipX = true;
				GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, GetComponent<Rigidbody2D>().velocity.y);
			}
		}


	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag.Equals("Player"))
		{
			Player.GetComponent<Player>().hitByEnemy(facingRight, 10);
		}
	}
}
