using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField] private ScoreSO scoreSO = default;
    [SerializeField] private GameOverChannelSO _gameOver = default;

    private int _score = 0;
    private bool _ableToScore = true;
    private GameObject _player;
    private TextMeshProUGUI _scoreText;

    private void OnEnable()
    {
        _gameOver.GameOverEvent += turnOffScoring;
    }

    private void OnDisable()
    {
        _gameOver.GameOverEvent -= turnOffScoring;
    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        if(_ableToScore)
        {
            _score = Mathf.Max(_score, (int)(_player.transform.position.y * 10));

            scoreSO.setScore(_score, gameObject.scene.name);
            _scoreText.text = "Score: " + _score.ToString();
        }
    }

    // turns off scoring when game ends
    private void turnOffScoring()
    {
        _ableToScore = false;
    }
}
