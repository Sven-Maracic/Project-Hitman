using UnityEngine;

public class PlayerCameraControls : MonoBehaviour
{
    [SerializeField] float maxOffsetDistance = 9;

    [SerializeField] float MinFOV = 5;
    [SerializeField] float MaxFOV = 10;
    [SerializeField] float ZoomSmoothTime = 10;




    Camera cam;
    float desiredCamFOV;


    Vector3 mousePosition, startingPosition;

    private void Start()
    {
        cam = GetComponent<Camera>();
        startingPosition = transform.localPosition;
        desiredCamFOV = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            desiredCamFOV -= (desiredCamFOV > MinFOV) ? 1 : 0;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            desiredCamFOV += (desiredCamFOV < MaxFOV) ? 1 : 0;
        }
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, desiredCamFOV, ZoomSmoothTime * Time.deltaTime);


        mousePosition = cam.ScreenPointToRay(Input.mousePosition).origin - transform.position;
        transform.localPosition = Vector3.ClampMagnitude(mousePosition, maxOffsetDistance) + startingPosition;
    }
}
