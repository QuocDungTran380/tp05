using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool("HasEntered", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
