using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletCollision : MonoBehaviour
{

	public int BulletDamage = 8;

	void Awake()
	{
		// Check if player has an upgrade and change BulletDamage
	}
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		// Check if bullet hits player
		if (collision.gameObject.layer == 9)
		{
			GameObject.Find("Player").GetComponent<Player>().hitByEnemy((gameObject.GetComponent<Rigidbody2D>().velocity.x > 0f), BulletDamage);
			Destroy(gameObject);
		}
		// Check if bullet hits a platform
		if (collision.gameObject.layer == 0 || collision.gameObject.layer == 10)
		{
			Destroy(gameObject);
		}
	}
}
