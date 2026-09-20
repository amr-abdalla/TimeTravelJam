using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
	public static GameOverManager Instance { get; private set; }

	[SerializeField] private GameObject _gameOverScreen;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning("Two instances of StorageManager detected, destroying one");
			Destroy(Instance);
		}
		Instance = this;

		Time.timeScale = 1.0f;
	}

	public void GameOver()
	{
		_gameOverScreen.SetActive(true);
		Time.timeScale = 0;
		StopAllCoroutines();
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(0);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
