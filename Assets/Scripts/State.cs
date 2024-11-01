using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Classe de base représentant un état pour le NPC (personnage non joueur)
public class State
{

    // Différents états possibles pour le NPC
    public enum STATE
    {
        IDLE,      // En attente
        PATROL,    // En patrouille
        PURSUE,    // En poursuite du joueur
        ATTACK,    // En attaque
        SLEEP,     // En sommeil
        RUNAWAY    // En fuite vers une zone sécurisée
    };

    // Événements d'état pour gérer les transitions
    public enum EVENT
    {
        ENTER,     // Entrée dans un état
        UPDATE,    // Mise à jour pendant un état
        EXIT       // Sortie d'un état
    };

    public STATE name;                   // Nom de l'état actuel
    protected EVENT stage;               // Événement actuel de l'état
    protected GameObject npc;            // Référence au NPC
    protected Animator anim;             // Contrôleur d'animation du NPC
    protected Transform player;          // Référence au joueur
    protected State nextState;           // Prochain état du NPC
    protected NavMeshAgent agent;        // Agent de navigation pour les déplacements

    // Variables de distance et d'angle pour détecter le joueur
    float attackDistance = 1.0f;              // Distance de tir

    // Constructeur pour initialiser les paramètres de l'état
    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        player = _player;
        stage = EVENT.ENTER;
    }

    // Méthodes virtuelles pour gérer l'entrée, la mise à jour, et la sortie des états
    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    // Processus pour gérer le cycle de vie de chaque état
    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState; // Retourne le prochain état après la sortie
        }
        return this;
    }

    // Méthode pour vérifier si le NPC peut voir le joueur
    public bool CanSeePlayer()
    {
        RaycastHit hit;
        Physics.SphereCast(npc.transform.position, 10f, npc.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity);
        if (hit.collider.CompareTag("Player"))
        {
            Debug.DrawRay(npc.transform.position, npc.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            return true;
        }
        else
        {
            Debug.DrawRay(npc.transform.position, npc.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            return false;
        }
    }

    // Méthode pour vérifier si le joueur est derrière le NPC
    public bool IsPlayerBehind()
    {
        Vector3 direction = npc.transform.position - player.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        if (direction.magnitude < 2.0f && angle < 30.0f) return true;
        return false;
    }

    // Méthode pour vérifier si le NPC est à distance d'attaque du joueur
    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        if (direction.magnitude < attackDistance)
        {
            return true;
        }
        return false;
    }
}

// État "Idle" : NPC reste en attente
public class Idle : State
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        else if (Random.Range(0, 100) < 10)
        {
            nextState = new Patrol(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

// État "Patrol" : NPC patrouille autour des points de contrôle
public class Patrol : State
{
    int currentIndex = -1;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PATROL;
        agent.speed = 2.0f;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        // Trouve le point de patrouille le plus proche pour commencer
        float lastDistance = Mathf.Infinity;
        for (int i = 0; i < GameEnvironment.Singleton.Checkpoints.Count; ++i)
        {
            GameObject thisWP = GameEnvironment.Singleton.Checkpoints[i];
            float distance = Vector3.Distance(npc.transform.position, thisWP.transform.position);
            if (distance < lastDistance)
            {
                currentIndex = i - 1;
                lastDistance = distance;
            }
        }
        base.Enter();
    }

    public override void Update()
    {

        if (agent.remainingDistance < 1)
        {
            if (GameEnvironment.Singleton.Checkpoints.Count == 0)
            {
                // Handle the case where there are no checkpoints
                return;
            }
            if (currentIndex >= GameEnvironment.Singleton.Checkpoints.Count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
            agent.SetDestination(GameEnvironment.Singleton.Checkpoints[currentIndex].transform.position);
        }

        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        else if (IsPlayerBehind())
        {
            nextState = new RunAway(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

// État "Pursue" : NPC poursuit le joueur
public class Pursue : State
{
    public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PURSUE;
        agent.speed = 2.0f;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.position);

        if (agent.hasPath)
        {
            if (CanAttackPlayer())
            {
                nextState = new Attack(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
            else if (!CanSeePlayer())
            {
                nextState = new Patrol(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

// État "Attack" : NPC attaque le joueur
public class Attack : State
{
    float rotationSpeed = 2.0f;

    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.ATTACK;
    }

    public override void Enter()
    {
        Debug.Log("Attacking player");
        anim.enabled = true;
        anim.SetBool("IsClose", true);
        agent.isStopped = true;
        base.Enter();
    }

    public override void Update()
    {
        // Vector3 direction = player.position - npc.transform.position;
        // float angle = Vector3.Angle(direction, npc.transform.forward);
        // direction.y = 0.0f;

        // npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);

        if (!CanAttackPlayer())
        {
            anim.SetBool("IsClose", false);
            nextState = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

// État "RunAway" : NPC s'enfuit vers un lieu sûr
public class RunAway : State
{
    GameObject safeLocation;

    public RunAway(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.RUNAWAY;
        safeLocation = GameObject.FindGameObjectWithTag("Safe");
    }

    public override void Enter()
    {
        agent.isStopped = false;
        agent.speed = 6;
        agent.SetDestination(safeLocation.transform.position);
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1.0f)
        {
            nextState = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}