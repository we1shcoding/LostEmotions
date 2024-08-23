using System.Collections;
using UnityEngine;
using TMPro; // TextMeshPro를 사용할 경우

public class TextController : MonoBehaviour
{

    public TextMeshProUGUI uiText; // TextMeshProUGUI 타입으로 변경
    public float displayDelay = 2f; // 각 메시지 사이의 지연 시간

    void Start()
    {
        StartCoroutine(DisplayTextSequence());
    }

    IEnumerator DisplayTextSequence()
    {
        // 첫 번째 메시지 표시
        uiText.text = "위험 요소 탐색중 ...";
        yield return new WaitForSeconds(displayDelay); // 2초 대기

        // 두 번째 메시지 표시
        uiText.text = "비정상적 생명체 감지!";
        yield return new WaitForSeconds(displayDelay); // 2초 대기

        uiText.text = "쓰레기장에서 인간 발견!";
        yield return new WaitForSeconds(displayDelay); // 2초 대기

        uiText.text = "인간 추가 분석 필요.";
        yield return new WaitForSeconds(displayDelay); // 2초 대기

        uiText.text = "연구 대상 데려가겠습니다.";
    }
}
