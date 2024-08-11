using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCamera : MonoBehaviour
{
    private Vector2 velocity;//백터값이네

    private float smoothTimeX;
    private float smoothTimeY;

    public GameObject player;//따라갈 놈

    public bool bounds;
    private Vector3 minCameraPos;
    private Vector3 maxCameraPos;

    //바운드 추가설정(X값)
    //박스 컬라이더 영역의 최소 최대 x값
    //카메라의 x값이기 때문에 중심 기준, 따라서 중간값이 필요함(양 끝은 x값에서 중간값을 빼줘야 함)
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
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);

        if (bounds)
        {//Mathf.Clamp(value,min,max)=적용대상, 최소,최대
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x + harfWidth, maxCameraPos.x - harfWidth),
                Mathf.Clamp(transform.position.y, 0, 0),
                Mathf.Clamp(transform.position.z, -10, -10));
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
