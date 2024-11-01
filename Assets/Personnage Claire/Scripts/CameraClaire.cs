using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClaire : MonoBehaviour
{

    public GameObject player;
    private float offset = 10f;
    private float cameraRotation;

    private void LateUpdate()
    {
        float mouseRotation = Input.GetAxis("Mouse X") * 5f;
        cameraRotation += mouseRotation;
        Vector3 desiredPosition = player.transform.position + Vector3.up * offset;
        transform.position = desiredPosition;
        transform.rotation = Quaternion.Euler(90f, cameraRotation, 0f);
    }
}
