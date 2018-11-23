using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDeath : MonoBehaviour
{

	private int health = 20;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		// Check health
		if (health <= 0)
		{
			Die();
		}
	}

	public void Damage(int damageAmount)
	{
		health -= damageAmount;
	}

	private void Die()
	{
		Destroy(gameObject);
	}
}
