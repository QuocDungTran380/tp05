using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Classe AI pour gérer le comportement du NPC avec un système d'états
public class AI : MonoBehaviour
{
    NavMeshAgent agent;      // Référence à l'agent de navigation
    Animator anim;           // Référence à l'animateur pour les animations du NPC
    State currentState;      // État actuel du NPC

    public Transform player; // Référence au joueur pour interagir avec le NPC
    public int hitcount;
    private float jumpAttackCooldown = 3f;
    private float jumpAttackCooldownLeft = 0f;

    public void EnemyHit()
    {
        if (jumpAttackCooldownLeft <= 0)
        {
            hitcount++;
            jumpAttackCooldownLeft = jumpAttackCooldown;
            Debug.Log("Hit " + hitcount);
            if (hitcount == 5)
            {
                Destroy(gameObject);
            }
        }
    }
    // Initialisation au démarrage de la scène
    void Start()
    {
        hitcount = 0;
        agent = GetComponent<NavMeshAgent>();    // Initialisation de l'agent de navigation
        anim = GetComponent<Animator>();         // Initialisation de l'animateur
        currentState = new Idle(gameObject, agent, anim, player); // Le NPC commence en état "Idle"
    }

    // Mise à jour à chaque frame pour traiter l'état actuel
    void Update()
    {
        jumpAttackCooldownLeft -= Time.deltaTime;
        currentState = currentState.Process();
    }
}