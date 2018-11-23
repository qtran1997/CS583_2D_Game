using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
	private bool ready = true;
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (!ready)
		{
			StartCoroutine(closing());
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag.Equals("Bullet") && ready)
		{
			StartCoroutine(GameObject.Find("Door01").GetComponent<OpenDoor>().OpenClose());
			ready = false;
		}
	}

	IEnumerator closing()
	{
		yield return new WaitForSeconds(3f);
		ready = true;
	}
}
