using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
    private GameObject player;
    public HungerBar hungerBar;
    public EnergyBar energyBar;
    public HealthBar healthBar;
    public TimerManager timerManager;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "FoodItems") {
            hungerBar.Eat();
            healthBar.Heal(2f);
            Destroy(collision.gameObject);
        } else if (collision.gameObject.tag == "EnergyItems") {
            energyBar.GainEnergy();
            Destroy(collision.gameObject);
        }
    }
    public void DestroyGame() {
        player.SetActive(false);
        timerManager._isGameOver = true;
        healthBar._currentHealth = 0;
        hungerBar._currentHunger = 0;
        energyBar._currentEnergy = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }   

    // Update is called once per frame
    void Update()
    {
        if (timerManager._remainingTime == 0 || healthBar._currentHealth <= 0 || hungerBar._currentHunger <= 0 || energyBar._currentEnergy <= 0) {
            DestroyGame();
        }
    }
}
