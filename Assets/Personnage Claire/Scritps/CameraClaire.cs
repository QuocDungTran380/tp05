using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClaire : MonoBehaviour
{

    public GameObject player;
    private Vector3 offset;

    [SerializeField] int rotationSpeed = 5;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = player.transform.position + player.transform.rotation * offset;
        transform.position = desiredPosition;
        transform.LookAt(player.transform);
    }
}
