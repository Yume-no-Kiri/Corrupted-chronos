using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;           // The character to follow
    //public Vector3 offset = new Vector3(0F, 0f, 10f);  // Offset from the target
    public float smoothSpeed = 5f;     // How quickly the camera catches up

    void LateUpdate()
    {
        if (target == null) return;

        //Vector3 desiredPosition = target.position+ offset;
        float zoom = 6f;
        Vector3 desiredPosition = target.position - transform.forward * zoom + Vector3.up * 1f;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
