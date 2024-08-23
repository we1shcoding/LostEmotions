using System.Collections;
using UnityEngine;
using TMPro; // TextMeshPro를 사용할 경우

public class TextController : MonoBehaviour
{

    public TextMeshProUGUI uiText; // TextMeshProUGUI 타입으로 변경
    // public TextMeshProUGUI uiText; // TextMeshPro를 사용할 경우

    void Start()
    {
        StartCoroutine(DisplayTextSequence());
    }

    IEnumerator DisplayTextSequence()
    {
        // 첫 번째 메시지 표시
        uiText.text = "tlqkf";
        yield return new WaitForSeconds(2f); // 2초 대기

        // 두 번째 메시지 표시
        uiText.text = "jonna";
        yield return new WaitForSeconds(2f); // 2초 대기

        // 세 번째 메시지 표시
        uiText.text = "sibal";
    }
}
