using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCamera : MonoBehaviour
{
    private Vector2 velocity;

    public float smoothTimeX = 0.3f;
    public float smoothTimeY = 0.3f;

    public GameObject player;

    public bool bounds;
    private Vector3 minCameraPos;
    private Vector3 maxCameraPos;

    public BoxCollider2D bound;

    private float harfHeight;
    private float harfWidth;

    private Camera theCamera;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        theCamera = GetComponent<Camera>();
        minCameraPos = bound.bounds.min;
        maxCameraPos = bound.bounds.max;
        harfHeight = theCamera.orthographicSize;
        harfWidth = harfHeight * Screen.width / Screen.height;
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
            float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

            transform.position = new Vector3(posX, posY, transform.position.z);

            if (bounds)
            {
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x + harfWidth, maxCameraPos.x - harfWidth),
                    Mathf.Clamp(transform.position.y, 0, 0),
                    Mathf.Clamp(transform.position.z, -10, -10));
            }
        }
    }

    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        minCameraPos = bound.bounds.min;
        maxCameraPos = bound.bounds.max;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

