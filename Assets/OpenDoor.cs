using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

	private bool doorOpened = false;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (doorOpened)
		{
			StartCoroutine(OpenClose());
		}
		else
		{
			StopCoroutine(OpenClose());

		}
	}


	public IEnumerator OpenClose()
	{
		gameObject.transform.position = new Vector2(gameObject.transform.position.x, 19.36f);
		doorOpened = true;
		yield return new WaitForSeconds(3f);
		gameObject.transform.position = new Vector2(gameObject.transform.position.x, 22.486f);
		doorOpened = false;
	}
}
