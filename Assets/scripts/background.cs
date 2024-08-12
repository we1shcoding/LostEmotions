using UnityEngine;

public class background : MonoBehaviour
{

    //플레이어 안 움직여도 흐르는 하늘배경 스크롤링

    [SerializeField]
    private Transform[] backgrounds; // 두 개의 배경 이미지
    [SerializeField]
    private float scrollSpeed; // 배경 스크롤 속도
    [SerializeField]
    private Camera mainCamera; // 메인 카메라

    private float viewZone; // 카메라의 가로 범위
    private int leftIndex; // 왼쪽 배경의 인덱스
    private int rightIndex; // 오른쪽 배경의 인덱스
    private float backgroundWidth; // 배경 이미지의 너비

    private void Start()
    {
        if (backgrounds.Length < 2)
        {
            Debug.LogError("배경 이미지가 2개 이상 필요합니다.");
            return;
        }

        // 각 배경 이미지의 너비를 설정 (배경 이미지가 동일한 너비를 가진다고 가정)
        backgroundWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;

        // 카메라의 가로 범위를 설정
        viewZone = mainCamera.orthographicSize * Screen.width / Screen.height;

        // 초기 인덱스 설정
        leftIndex = 0;
        rightIndex = backgrounds.Length - 1;
    }

    private void Update()
    {
        // 배경 이미지를 왼쪽으로 이동
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        }

        // 오른쪽 배경이 카메라 왼쪽으로 벗어났을 때
        if (mainCamera.transform.position.x - backgrounds[leftIndex].position.x > viewZone + backgroundWidth / 2)
        {
            ScrollLeft();
        }

        // 왼쪽 배경이 카메라 오른쪽으로 벗어났을 때
        if (backgrounds[rightIndex].position.x - mainCamera.transform.position.x > viewZone + backgroundWidth / 2)
        {
            ScrollRight();
        }
    }

    private void ScrollLeft()
    {
        // 왼쪽 배경을 오른쪽 끝으로 이동
        int lastRight = rightIndex;
        backgrounds[leftIndex].position = backgrounds[rightIndex].position + Vector3.right * backgroundWidth;
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == backgrounds.Length)
        {
            leftIndex = 0;
        }
    }

    private void ScrollRight()
    {
        // 오른쪽 배경을 왼쪽 끝으로 이동
        int lastLeft = leftIndex;
        backgrounds[rightIndex].position = backgrounds[leftIndex].position - Vector3.right * backgroundWidth;
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
        {
            rightIndex = backgrounds.Length - 1;
        }
    }
}
