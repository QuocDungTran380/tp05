using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    private static int keyCount = 0;

    private void OnCollisionEnter(Collision other)
    {

        if (other.collider.tag == "Player")
        {
            keyCount++;
            Destroy(gameObject);
            if (keyCount == 3)
            {
                GameObject.Find("Door").GetComponent<ExitDoor>().CanOpen = true;
            }
        }
    }
}
