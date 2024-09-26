using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public Image _hungerBar;
    public TextMeshProUGUI _level;
    public float _currentHunger = 100f;

    private void TakeHunger()
    {
        if (_currentHunger > 0)
        {
            _currentHunger -= 3f * Time.deltaTime;
            _hungerBar.fillAmount = _currentHunger / 100f;
            _level.text = _currentHunger.ToString("F0") + " %";
        }
    }

    public void Eat() {
        if (_currentHunger < 100) {
            _currentHunger += 10f;
            _currentHunger = Mathf.Clamp(_currentHunger, 0, 100);
            _hungerBar.fillAmount = _currentHunger / 100f;
            _level.text = _currentHunger.ToString("F0") + " %";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _hungerBar.fillAmount = _currentHunger / 100f;
        _level.text = _currentHunger + " %";
    }

    // Update is called once per frame
    void Update()
    {
        TakeHunger();        
    }
}
