using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public bool CanOpen = false;
    public bool HasEntered = false;
    Animator animator;

    void Start()
    {
        GetComponent<Animator>().enabled = true;
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        animator.SetBool("HasEntered", true);

        // Vérifiez si l'objet qui est entré dans le trigger a le tag "Player"
        if (other.CompareTag("Player") && CanOpen)
        {
            animator.SetBool("HasEntered", false);

            // Exécutez l'action souhaitée, par exemple afficher un message
            Debug.Log("Le joueur est entré dans la zone de trigger!");

            // Vous pouvez également appeler une méthode ou changer l'état du jeu
            // Exemple : augmenter un score, terminer un niveau, etc.
            // IncreaseScore();
            // EndLevel();
        }
    }
}
