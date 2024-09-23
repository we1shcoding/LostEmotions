using System.Collections;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // roro의 TextMeshProUGUI 컴포넌트
    public AutoSizeTextBox autoSizeTextBox; // 텍스트 박스 자동 크기 조정 스크립트
    public Transform roroTransform; // roro의 Transform
    public Canvas canvas; // UI가 표시되는 캔버스
    public float textChangeInterval = 4f; // 텍스트 변경 간격 (4초로 변경)
    public float verticalOffset = 50f; // 텍스트 박스를 roro 위로 약간 띄우기 위한 오프셋

    private string[] dialogues = new string[]
    {
        "...폐기물 더미 내 수집 요소 분석중",
        "금속제 폐기물 발견, 상태 심각한 부식",
        "다음 탐색 지속...",
        "전자기기 잔해 발견 : 작은 오염, 사용감 조금 있음",
        "다음 폐기물 더미 내 수집 요소 분석중...",
        "...비정상적 생물학적 신호 감지",
        "위치 : 대상을 우선적으로 수색합니다.",
        "탐색 결과 : 인간 여성, 연구 가치 있음",
        "대상을 연구소로 이송하여 회복조치를 우선적으로 수행합니다."
    };

    private int currentDialogueIndex = 0; // 현재 대사 인덱스

    void Start()
    {
        if (textComponent == null)
        {
            Debug.LogError("TextComponent is not assigned.");
            return;
        }

        if (roroTransform == null)
        {
            Debug.LogError("Roro's Transform is not assigned.");
            return;
        }

        if (canvas == null)
        {
            Debug.LogError("Canvas is not assigned.");
            return;
        }

        textComponent.gameObject.SetActive(false); // 시작 시 텍스트 박스 비활성화
        StartCoroutine(ChangeDialogueRoutine()); // 대사 변경 코루틴 시작
    }

    private IEnumerator ChangeDialogueRoutine()
    {
        while (true)
        {
            // 텍스트 박스 활성화
            textComponent.gameObject.SetActive(true);

            // 텍스트 변경
            textComponent.text = dialogues[currentDialogueIndex];

            // 텍스트 박스 크기 조정
            if (autoSizeTextBox != null)
            {
                autoSizeTextBox.AdjustTextBoxSize();
            }

            // roro의 월드 좌표를 화면 좌표로 변환
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(roroTransform.position);

            // 화면 좌표를 캔버스의 로컬 좌표로 변환
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPosition, Camera.main, out Vector2 localPoint);

            // 약간의 y축 오프셋 적용
            localPoint.y += verticalOffset;

            // TextBox 위치 설정
            autoSizeTextBox.textBox.localPosition = localPoint;

            // 인덱스 업데이트 (다음 대사로 순환)
            currentDialogueIndex = (currentDialogueIndex + 1) % dialogues.Length;

            // 대기
            yield return new WaitForSeconds(textChangeInterval);

            // 텍스트 박스 비활성화
            textComponent.gameObject.SetActive(false);
        }
    }
}
