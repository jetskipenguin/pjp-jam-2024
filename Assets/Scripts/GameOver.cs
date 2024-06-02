using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject gameOverPanel = default;
    [SerializeField] private GameObject leaderboardPanel = default;
    [SerializeField] private TextMeshProUGUI highScoreUI = default;
    [SerializeField] private TextMeshProUGUI scoreUI = default;
    [SerializeField] private TextMeshProUGUI newHighScoreUI = default;


    [Header("Asset References")]
    [SerializeField] private GameOverChannelSO gameOverChannel = default;
    [SerializeField] private ScoreSO scoreSO = default;

    private bool _gameOver = false;

    private void OnEnable()
    {
        gameOverChannel.GameOverEvent += OnGameOver;
    }

    private void OnDisable()
    {
        gameOverChannel.GameOverEvent -= OnGameOver;
        newHighScoreUI.gameObject.SetActive(false);
    }

    private void OnGameOver()
    {
        Debug.Log("Game Over");
        if (_gameOver) { return; }

        Debug.Log("Game Over");
        if(scoreSO.getScore() >= scoreSO.highScores[gameObject.scene.name])
        {
            leaderboardPanel.SetActive(true);
            newHighScoreUI.gameObject.SetActive(true);
        }
        else
        {
            leaderboardPanel.SetActive(false);
            gameOverPanel.SetActive(true);
        }

        highScoreUI.text = "Highscore: " + scoreSO.highScores[gameObject.scene.name].ToString();
        scoreUI.text = "Score: " + scoreSO.getScore().ToString();
        _gameOver = true;
    }

    public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
