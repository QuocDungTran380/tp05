using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public static int _numberOfKeys;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _numberOfKeys = 0;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _numberOfKeys++;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_numberOfKeys == 3)
        {
            GameObject.Find("Door").GetComponent<Door>()._canOpen = true;
        }
        
    }
}
