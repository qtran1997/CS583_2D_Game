using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{

	public int BulletDamage = 12;

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
		// Check if bullet hits enemy
		if (collision.gameObject.layer == 11)
		{
			collision.gameObject.GetComponent<DamageDeath>().Damage(BulletDamage);
			Destroy(gameObject);
		}
		// Check if bullet hits a platform
		if (collision.gameObject.layer == 0 || collision.gameObject.layer == 10)
		{
			Destroy(gameObject);
		}
	}
}
