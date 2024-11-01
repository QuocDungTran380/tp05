using UnityEngine;

public class KeyScript : MonoBehaviour
{
    private int numberOfKeys = 0;
    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GameObject.Find("Door").GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            numberOfKeys++;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfKeys == 3)
        {
            _animator.SetBool("HasEntered", false);
        }
        
    }
}
