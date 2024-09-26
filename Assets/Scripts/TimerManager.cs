using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public ItemsController itemsController;
    public TextMeshProUGUI timerText;
    public float _remainingTime = 60f;
    public bool _isGameOver = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!_isGameOver)
        {
            _remainingTime -= Time.deltaTime;
            timerText.text = "Time: " + _remainingTime.ToString("F0");
        }
        if (_isGameOver || _remainingTime <= 0)
        {
            itemsController.DestroyGame();
            timerText.text = "Game Over!";
        }
    }
}
