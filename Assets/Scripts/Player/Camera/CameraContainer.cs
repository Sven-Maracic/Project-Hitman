using UnityEngine;

public class CameraContainer : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position;
    }
}
