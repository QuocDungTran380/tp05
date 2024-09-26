using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Image _energyBar;
    public TextMeshProUGUI _level;
    public float _currentEnergy = 100f;

    private void LoseEnergy(float value)
    {
        if (_currentEnergy > 0)
        {
            _currentEnergy -= value;
            _currentEnergy = Math.Clamp(_currentEnergy, 0, 100);
            _energyBar.fillAmount = _currentEnergy / 100f;
            _level.text = _currentEnergy.ToString("F0") + " %";
        }
    }

    public void GainEnergy() {
        if (_currentEnergy < 100) {
            _currentEnergy += 15f;
            _currentEnergy = Math.Clamp(_currentEnergy, 0, 100);
            _energyBar.fillAmount = _currentEnergy / 100f;
            _level.text = _currentEnergy.ToString("F0") + " %";
        }
    }

    void Start()
    {
        _energyBar.fillAmount = _currentEnergy / 100f;
        _level.text = _currentEnergy + " %";
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            LoseEnergy(0.1f);
        }
        LoseEnergy(1f * Time.deltaTime);

    }
}
