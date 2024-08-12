using UnityEngine;

public class ScrollBackfront : MonoBehaviour
{
    [SerializeField]
    private Transform[] backgrounds; // 두 개의 배경 이미지
    [SerializeField]
    private float scrollSpeed; // 배경 스크롤 속도
    [SerializeField]
    private Camera mainCamera; // 메인 카메라

    private float backgroundWidth; // 배경 이미지의 너비
    private Vector3 lastCameraPosition; // 마지막 카메라 위치

    private void Start()
    {
        if (backgrounds.Length < 2)
        {
            Debug.LogError("배경 이미지가 2개 이상 필요합니다.");
            return;
        }

        // 각 배경 이미지의 너비를 설정 (배경 이미지가 동일한 너비를 가진다고 가정)
        backgroundWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;

        // 초기 배경 위치 설정
        ArrangeBackgrounds();

        // 카메라의 초기 위치 저장
        lastCameraPosition = mainCamera.transform.position;
    }

    private void Update()
    {
        // 카메라의 현재 위치
        Vector3 currentCameraPosition = mainCamera.transform.position;

        // 카메라가 이동했는지 확인
        if (currentCameraPosition != lastCameraPosition)
        {
            // 카메라 이동 방향을 계산
            Vector3 moveDirection = currentCameraPosition - lastCameraPosition;

            // 배경 이동 방향 결정 (카메라 이동 방향과 반대 방향으로 이동)
            float moveAmount = scrollSpeed * Time.deltaTime * (moveDirection.x < 0 ? 1 : -1);

            // 배경을 이동
            for (int i = 0; i < backgrounds.Length; i++)
            {
                backgrounds[i].position += Vector3.right * moveAmount;
            }

            // 왼쪽 배경이 카메라의 오른쪽 범위를 벗어났을 때
            if (backgrounds[0].position.x < currentCameraPosition.x - backgroundWidth)
            {
                ScrollRight();
            }

            // 오른쪽 배경이 카메라의 왼쪽 범위를 벗어났을 때
            if (backgrounds[1].position.x > currentCameraPosition.x + backgroundWidth)
            {
                ScrollLeft();
            }

            // 마지막 카메라 위치 업데이트
            lastCameraPosition = currentCameraPosition;
        }
    }

    private void ScrollLeft()
    {
        // 오른쪽 배경을 왼쪽 끝으로 이동
        backgrounds[1].position = backgrounds[0].position - Vector3.right * backgroundWidth;
        // 배경의 인덱스를 변경
        Transform temp = backgrounds[0];
        backgrounds[0] = backgrounds[1];
        backgrounds[1] = temp;
    }

    private void ScrollRight()
    {
        // 왼쪽 배경을 오른쪽 끝으로 이동
        backgrounds[0].position = backgrounds[1].position + Vector3.right * backgroundWidth;
        // 배경의 인덱스를 변경
        Transform temp = backgrounds[1];
        backgrounds[1] = backgrounds[0];
        backgrounds[0] = temp;
    }

    private void ArrangeBackgrounds()
    {
        // 배경을 카메라의 범위에 맞게 초기 배치
        backgrounds[1].position = backgrounds[0].position + Vector3.right * backgroundWidth;
    }
}
