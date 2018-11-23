using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitch : MonoBehaviour
{

	public Button StartButton;
	public Button QuitButton;
	public Button CreditButton;
	public GameObject Player;

	private bool Up;
	private bool Down;

	private int start = 1;
	private int quit = 2;
	private int credits = 3;

	private int menuButtonIndex = 1;

	private bool animating = false;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		Up = Input.GetKeyDown(KeyCode.UpArrow);
		Down = Input.GetKeyDown(KeyCode.DownArrow);
		if (!animating)
		{
			if (Up)
			{
				if (menuButtonIndex == start)
				{
					menuButtonIndex = credits;
					Player.transform.Translate(0, -38.46f, 0);

				}
				else
				{
					Player.transform.Translate(0, 19.23f, 0);
					menuButtonIndex--;
				}
			}
			else if (Down)
			{
				if (menuButtonIndex == credits)
				{
					Player.transform.Translate(0, 38.46f, 0);

					menuButtonIndex = start;
				}
				else
				{
					Player.transform.Translate(0, -19.23f, 0);

					menuButtonIndex++;
				}
			}

			if (menuButtonIndex == start)
			{
				// Change to gray (focused)
				ColorBlock cb = StartButton.GetComponent<Button>().colors;
				cb.normalColor = Color.gray;
				StartButton.GetComponent<Button>().colors = cb;

				cb = QuitButton.GetComponent<Button>().colors;
				cb.normalColor = Color.white;
				QuitButton.GetComponent<Button>().colors = cb;

				cb = CreditButton.GetComponent<Button>().colors;
				cb.normalColor = Color.white;
				CreditButton.GetComponent<Button>().colors = cb;
			}
			else if (menuButtonIndex == quit)
			{
				// Change to gray (focused)
				ColorBlock cb = QuitButton.GetComponent<Button>().colors;
				cb.normalColor = Color.gray;
				QuitButton.GetComponent<Button>().colors = cb;

				cb = StartButton.GetComponent<Button>().colors;
				cb.normalColor = Color.white;
				StartButton.GetComponent<Button>().colors = cb;

				cb = CreditButton.GetComponent<Button>().colors;
				cb.normalColor = Color.white;
				CreditButton.GetComponent<Button>().colors = cb;
			}
			else if (menuButtonIndex == credits)
			{
				// Change to gray (focused)
				ColorBlock cb = QuitButton.GetComponent<Button>().colors;
				cb.normalColor = Color.white;
				QuitButton.GetComponent<Button>().colors = cb;

				cb = StartButton.GetComponent<Button>().colors;
				cb.normalColor = Color.white;
				StartButton.GetComponent<Button>().colors = cb;

				cb = CreditButton.GetComponent<Button>().colors;
				cb.normalColor = Color.gray;
				CreditButton.GetComponent<Button>().colors = cb;
			}

			if (Input.GetKeyDown(KeyCode.X))
			{
				Player.GetComponent<MenuShoot>().fire();
				Player.GetComponent<Animator>().Play("xeonmenushoot");

				if (menuButtonIndex == 1)
				{
					// START GAME
					StartCoroutine(StartGame());
					animating = true;

				}
				else if (menuButtonIndex == 2)
				{
					// EXIT GAME
					StartCoroutine(ExitGame());
					animating = true;

				}
				else if (menuButtonIndex == 3)
				{
					// CREDITS
					animating = true;

				}


			}
		}
	}

	IEnumerator StartGame()
	{
		float length = Player.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length;
		yield return new WaitForSeconds(length);
		SceneManager.LoadScene(sceneName: "Intro");
		SceneManager.UnloadSceneAsync(sceneName: "MenuScene");
	}

	IEnumerator ExitGame()
	{
		float length = Player.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length;
		yield return new WaitForSeconds(length);
		Debug.Log("QUIT");
		Application.Quit();
	}
}
