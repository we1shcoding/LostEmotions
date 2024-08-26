using System.Collections;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    public TextMeshProUGUI uiText; // TextMeshProUGUI 타입으로 변경
    public float displayDelay = 3f; // 각 메시지 사이의 지연 시간
    public float displayDelay2 = 5f; // 각 메시지 사이의 지연 시간
    public float typingSpeed = 0.05f; // 타이핑 속도
    public AutoSizeTextBox autoSizeTextBox; // AutoSizeTextBox 스크립트 참조
    public GameObject roro; // roro 개체 참조
    public float moveDistance = 3f; // roro의 이동 거리
    public float extendedMoveDistance = 6f; // roro의 더 멀리 이동할 거리
    public float moveDuration = 3f; // roro의 이동에 걸리는 시간

    void Start()
    {
        StartCoroutine(DisplayTextSequence());
    }

    IEnumerator DisplayTextSequence()
    {
        // 첫 번째 roro 이동 (확장된 거리로 이동)
        yield return StartCoroutine(MoveRoroX(extendedMoveDistance)); // roro의 x축 이동

        // 첫 번째 메시지 표시
        yield return StartCoroutine(TypeText("위험 요소 탐색중 ..."));

        // 두 번째 roro 이동 (기본 거리로 이동)
        yield return StartCoroutine(MoveRoroX(moveDistance)); // roro의 x축 이동
        yield return new WaitForSeconds(displayDelay2); // 5초 대기

        // 두 번째 메시지 표시
        yield return StartCoroutine(TypeText("비정상적 생명체 감지!"));
        yield return new WaitForSeconds(displayDelay); // 3초 대기

        // 세 번째 메시지 표시
        yield return StartCoroutine(TypeText("쓰레기장에서 인간 발견!"));
        yield return new WaitForSeconds(displayDelay); // 3초 대기

        // 네 번째 메시지 표시
        yield return StartCoroutine(TypeText("인간 추가 분석 필요."));
        yield return new WaitForSeconds(displayDelay); // 3초 대기

        // 마지막 메시지 표시
        yield return StartCoroutine(TypeText("연구 대상 데려가겠습니다."));
    }

    IEnumerator TypeText(string message)
    {
        uiText.text = ""; // 기존 텍스트 초기화
        int totalLength = message.Length;

        for (int i = 0; i < totalLength; i++)
        {
            uiText.text = message.Substring(0, i + 1); // 현재 글자까지의 텍스트 표시
            autoSizeTextBox.AdjustTextBoxSize(); // 텍스트 크기에 맞게 배경 크기 조정
            yield return new WaitForSeconds(typingSpeed); // 타이핑 속도만큼 대기
        }
    }

    IEnumerator MoveRoroX(float distance)
    {
        if (roro == null)
        {
            Debug.LogError("roro GameObject is not assigned.");
            yield break;
        }

        Vector3 startPosition = roro.transform.position;
        Vector3 targetPosition = startPosition + new Vector3(distance, 0, 0);
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            roro.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 마지막 위치를 보정
        roro.transform.position = targetPosition;
    }
}
