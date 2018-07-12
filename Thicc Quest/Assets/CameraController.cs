using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float cameraSpeed = 10;
    public float CameraDelay = 0.05f;

    public bool MoveCamera = true;


    Vector2 Destination;
    Vector2 start;
    float camLerpAlpha = 0.0f;
    float startZ = -10.0f;
    // Use this for initialization
    void Start ()
    {
        startZ = gameObject.transform.position.z;
        StartCoroutine(UpdateCameraPosition());	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        LerpCamera();
    }

    private void Update()
    {

    }

    private void LerpCamera()
    {
        if (camLerpAlpha < 1.0f)
        {
            camLerpAlpha += Time.deltaTime*cameraSpeed;
            Vector3 trans = Vector3.Lerp(start, Destination, camLerpAlpha);
            trans.z = startZ;
            transform.position = trans;
        }
    }

    private IEnumerator UpdateCameraPosition()
    {
        while (MoveCamera)
        {
            Destination = player.transform.position;
            start = transform.position;
            camLerpAlpha = 0.0f;
            yield return new WaitForSeconds(CameraDelay);
        }
    }
}
