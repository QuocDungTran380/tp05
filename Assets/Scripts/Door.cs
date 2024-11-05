using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator _animator;
    public bool _canOpen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        _canOpen = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_canOpen)
            {
                _animator.SetBool("HasEntered", false);
            }
            else if (!_canOpen)
            {
                _animator.SetBool("HasEntered", true);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
