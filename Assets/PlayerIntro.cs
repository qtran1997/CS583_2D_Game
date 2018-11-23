using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerIntro : MonoBehaviour
{

	public Image fadeToBlack;

	public GameObject zombieObject;
	public GameObject beamObject;
	public GameObject beamCharge2Object;
	public Transform beamSpawnPoint;

	public GameObject explosionObject;

	private bool faceRight = true;

	private string[] dialogOrder = new string[20];
	private int dialogCounter = 0;

	private string dialogString = "";

	public TextAsset textFile;

	// Use this for initialization
	void Start()
	{
		readString();
		StartCoroutine(intro1());
		StartCoroutine(intro2());
		StartCoroutine(removeBoss());
		StartCoroutine(intro3());
		StartCoroutine(intro4());
		StartCoroutine(intro5());
		StartCoroutine(intro5explosion());
		StartCoroutine(exitScene());
	}

	// Update is called once per frame
	void Update()
	{
		GameObject.Find("Canvas").transform.Find("PlayerText/TextBox").gameObject.GetComponent<Text>().text = dialogString;
	}

	void FixedUpdate()
	{
		GameObject[] zombies = GameObject.FindGameObjectsWithTag("WalkingEnemy");
		for (var i = 0; i < zombies.Length; i++)
		{
			zombies[i].GetComponent<Animator>().speed = .5f + (i * .15f);
			zombies[i].transform.Translate(new Vector3(.015f, 0, 0));
		}
	}

	public IEnumerator intro1()
	{
		yield return new WaitForSeconds(2.8f);
		StartCoroutine(AnimateText(dialogOrder[dialogCounter]));
		StopCoroutine(AnimateText(dialogOrder[dialogCounter]));
	}

	public IEnumerator intro2()
	{
		yield return new WaitForSeconds(11.56f);
		StopCoroutine(intro1());
		StartCoroutine(AnimateText(dialogOrder[++dialogCounter]));
	}

	public IEnumerator intro3()
	{
		yield return new WaitForSeconds(18.009f);
		StopCoroutine(removeBoss());
		StopCoroutine(intro2());
		StartCoroutine(AnimateText(dialogOrder[++dialogCounter]));
	}

	public IEnumerator intro4()
	{
		yield return new WaitForSeconds(22f);
		StopCoroutine(intro3());
		GameObject zombie1 = Instantiate(zombieObject, new Vector3(-18f, 2.66f, -2.62f), Quaternion.identity) as GameObject;
		GameObject zombie2 = Instantiate(zombieObject, new Vector3(-17.5f, 2.66f, -2.62f), Quaternion.identity) as GameObject;
		GameObject zombie3 = Instantiate(zombieObject, new Vector3(-18.3f, 2.66f, -2.62f), Quaternion.identity) as GameObject;

		StartCoroutine(AnimateText(dialogOrder[++dialogCounter]));
	}

	public IEnumerator intro5()
	{
		yield return new WaitForSeconds(28.03f);
		StopCoroutine(intro4());
		fire(4f);
	}

	public IEnumerator intro5explosion()
	{
		yield return new WaitForSeconds(28.06f);
		StopCoroutine(intro5());
		GameObject explosion1 = Instantiate(explosionObject, new Vector3(.118f, .127f, 0), Quaternion.identity) as GameObject;
		Destroy(explosion1, .83f);
		GameObject explosion2 = Instantiate(explosionObject, new Vector3(-5.46f, 2.84f, 0), Quaternion.identity) as GameObject;
		Destroy(explosion2, .83f);
		GameObject explosion3 = Instantiate(explosionObject, new Vector3(.188f, -0.164f, 0), Quaternion.identity) as GameObject;
		Destroy(explosion3, .83f);
		GameObject explosion4 = Instantiate(explosionObject, new Vector3(-.1f, -0.164f, 0), Quaternion.identity) as GameObject;
		Destroy(explosion4, .83f);
	}

	public IEnumerator exitScene()
	{
		yield return new WaitForSeconds(29.15f);
		StopCoroutine(intro5explosion());
		fadeToBlack.GetComponent<Animator>().SetBool("Fade", true);
		yield return new WaitUntil(() => fadeToBlack.color.a == 1);
		SceneManager.LoadScene(sceneName: "Level1");
		SceneManager.UnloadSceneAsync(sceneName: "Intro");
	}

	public IEnumerator removeBoss()
	{
		yield return new WaitForSeconds(20f);
		GameObject.Find("wizard").SetActive(false);
	}

	private void readString()
	{
		StringReader reader = new StringReader(textFile.text);
		var lineCount = 0;
		while (reader.Peek() >= 0)
		{
			dialogOrder[lineCount] = reader.ReadLine();
			lineCount++;
		}
		reader.Close();
	}

	private void openDialogBox()
	{
		GameObject.Find("Canvas").transform.Find("PlayerText").gameObject.SetActive(true);
		GameObject.Find("Canvas").transform.Find("ImageContainer").gameObject.SetActive(true);
	}

	private static void closeDialogBox()
	{
		GameObject.Find("Canvas").transform.Find("PlayerText").gameObject.SetActive(false);
		GameObject.Find("Canvas").transform.Find("ImageContainer").gameObject.SetActive(false);
	}

	private IEnumerator AnimateText(string strComplete)
	{
		dialogString = "";
		for (int i = 0; i <= strComplete.Length; i++)
		{
			dialogString = strComplete.Substring(0, i);
			yield return new WaitForSeconds(0.1F);
		}
	}

	public void fire(float time)
	{

		var shootingDirection = beamSpawnPoint.position.x;
		GameObject beam;
		if (time == 3f)
		{
			beam = Instantiate(beamCharge2Object, new Vector3(shootingDirection, beamSpawnPoint.position.y, beamSpawnPoint.position.z), Quaternion.identity) as GameObject;
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
		Destroy(beam, .2f);
	}
}
