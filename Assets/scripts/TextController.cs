using System.Collections;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    public TextMeshProUGUI uiText; // TextMeshProUGUI 타입으로 변경
    public float displayDelay = 2f; // 각 메시지 사이의 지연 시간
    public float typingSpeed = 0.05f; // 타이핑 속도
    public AutoSizeTextBox autoSizeTextBox; // AutoSizeTextBox 스크립트 참조

    void Start()
    {
        StartCoroutine(DisplayTextSequence());
    }

    IEnumerator DisplayTextSequence()
    {
        // 첫 번째 메시지 표시
        yield return StartCoroutine(TypeText("위험 요소 탐색중 ..."));
        yield return new WaitForSeconds(displayDelay); // 2초 대기

        // 두 번째 메시지 표시
        yield return StartCoroutine(TypeText("비정상적 생명체 감지!"));
        yield return new WaitForSeconds(displayDelay); // 2초 대기

        yield return StartCoroutine(TypeText("쓰레기장에서 인간 발견!"));
        yield return new WaitForSeconds(displayDelay); // 2초 대기

        yield return StartCoroutine(TypeText("인간 추가 분석 필요."));
        yield return new WaitForSeconds(displayDelay); // 2초 대기

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
}
