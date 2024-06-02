using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
	public void reloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public void loadNextScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
