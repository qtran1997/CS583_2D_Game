using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShoot : MonoBehaviour
{

	public GameObject player;
	public GameObject beamCharge2Object;
	public Transform beamSpawnPoint;



	// Use this for initialization
	void Start()
	{
		player.transform.Translate(0.0f, 0.0f, 0.0f);
	}

	// Update is called once per frame
	void Update()
	{
	}

	void FixedUpdate()
	{

	}
	public void fire()
	{
		var shootingDirection = beamSpawnPoint.position.x;
		GameObject beam = Instantiate(beamCharge2Object, new Vector3(shootingDirection, beamSpawnPoint.position.y, beamSpawnPoint.position.z - 30), Quaternion.identity) as GameObject;
		beam.transform.localScale += new Vector3(60f, 60f, 60f);
		beam.GetComponent<SpriteRenderer>().flipX = false;
		var beamRigidBody = beam.GetComponent<Rigidbody2D>();
		beamRigidBody.velocity = Quaternion.Euler(0, 0, 0) * Vector3.right * 1250;
		Destroy(beam, 1f);
	}
}
