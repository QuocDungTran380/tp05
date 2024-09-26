using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image _healthBar;
    public TextMeshProUGUI _level;
    public float _currentHealth = 100f;

    public void TakeDamage(float value)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= value;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, 100);
            _healthBar.fillAmount = _currentHealth / 100f;
            _level.text = _currentHealth.ToString("F0") + " %";
        }
    }

    public void Heal(float value)
    {
        if (_currentHealth < 100)
        {
            _currentHealth += value;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, 100);
            _healthBar.fillAmount = _currentHealth / 100f;
            _level.text = _currentHealth.ToString("F0") + " %";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _healthBar.fillAmount = _currentHealth / 100f;
        _level.text = _currentHealth + " %";
    }

    // Update is called once per frame
    void Update()
    {
    }
}
