using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roroCamera2 : MonoBehaviour
{
    public float delayTime = 28f; // 28초 대기 시간
    public float moveDuration = 12f; // 카메라가 이동하는 데 걸리는 시간
    public Vector3 targetPosition; // 카메라가 이동할 목표 위치

    private Vector3 startPosition; // 카메라의 초기 위치
    private bool isMoving = false; // 카메라가 이동 중인지 여부
    private float elapsedTime = 0f; // 경과 시간 기록

    void Start()
    {
        startPosition = transform.position; // 카메라의 초기 위치 저장
        StartCoroutine(StartMoveAfterDelay());
    }

    IEnumerator StartMoveAfterDelay()
    {
        yield return new WaitForSeconds(delayTime); // 28초 대기
        isMoving = true;
    }

    void Update()
    {
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration; // 이동 경과 시간 비율 (0에서 1로 증가)

            if (t <= 1f)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, t); // 카메라 이동
            }
            else
            {
                isMoving = false; // 목표 위치에 도달하면 이동 중지
            }
        }
    }
}
