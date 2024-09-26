using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform player;        // Référence au joueur
    public float speed = 0.05f;      // Vitesse de déplacement vers le joueur
    public const float activationDistance = 20f;  // Distance d'activation de l'ennemi
    private Animator animator;      // Référence à l'Animator
    public HealthBar healthBar;
    private bool canHit = true;

    Vector3 direction;

    void Start()
    {
        // Si le joueur n'est pas assigné manuellement dans l'inspecteur, trouvez-le automatiquement
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Récupérer le composant Animator
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;  // Désactiver l'Animator au début
        }
    }
    void HitPlayer()
    {
        if (canHit)
        {
            healthBar.TakeDamage(5f);
            canHit = false;
        }
        else return;
    }

    void Update()
    {
        // Vérifier que le joueur est assigné et existe toujours
        if (player != null && animator != null)
        {
            // Calculer la distance entre l'ennemi et le joueur
            float distance = Vector3.Distance(transform.position, player.position);

            // Si le joueur est dans la distance d'activation, activer l'animation
            if (distance <= activationDistance)
            {
                if (!animator.enabled)
                {
                    animator.enabled = true;  // Activer l'animation
                    Debug.Log("Le joueur s'approche. Activation de l'ennemi.");
                }

                // // Calculer la direction vers le joueur
                direction = new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z).normalized;

                Debug.Log("Le joueur s'approche. Activation de l'ennemi." + direction);

                // // Déplacer l'ennemi vers le joueur
                transform.position += (direction / 5) * speed * Time.deltaTime;

                // Faire face au joueur
                transform.LookAt(player);
                if (distance <= 1f)
                {
                    Debug.Log("Le joueur est proche de l'ennemi.");
                    animator.SetBool("IsClose", true);
                    HitPlayer();
                }
                else {
                    animator.SetBool("IsClose", false);
                    canHit = true;
                }
            }
        }
    }
}