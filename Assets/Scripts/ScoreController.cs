using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    private int _score = 0;
    private bool _gameOver = false;
    private GameObject _player;
    private TextMeshProUGUI _scoreText;

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        if(!_gameOver)
        {
            _score = Mathf.Max(_score, (int)(_player.transform.position.y * 10));
            _scoreText.text = "Score: " + _score.ToString();
        }
    }
}
