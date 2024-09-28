using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextController : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // roro의 TextMeshProUGUI 컴포넌트
    public AutoSizeTextBox autoSizeTextBox; // 텍스트 박스 자동 크기 조정 스크립트
    public Transform roroTransform; // roro의 Transform
    public Canvas canvas; // UI가 표시되는 캔버스
    public GameObject background; // Background UI 오브젝트
    public float initialWaitTime; // 시작 후 대사 출력 전 대기 시간

    private string[] dialogues = new string[] {
        "...폐기물 더미 내\n수집 요소 분석중",
        "금속제 폐기물 발견,\n상태 심각한 부식",
        "다음 탐색 지속...",
        "전자기기 잔해 발견 :\n작은 오염, 사용감 조금 있음",
        "다음 폐기물 더미 내\n수집 요소 분석중...",
        "...비정상적 생물학적\n신호 감지",
        "위치 :\n폐기물 더미 사이",
        "탐색 결과 :\n인간 여성, 연구 가치 있음",
        "대상을 연구소로 이송하여\n회복조치를 수행합니다."
    };

    private float[] dialogueDurations = new float[] {
        3f, 4f, 7f, 3f, 2f, 3f, 2f, 2f, 2f
    };

    private int currentDialogueIndex = 0; // 현재 대사 인덱스

    void Start()
    {
        if (textComponent == null || roroTransform == null || canvas == null || background == null)
        {
            Debug.LogError("필수 컴포넌트가 할당되지 않았습니다.");
            return;
        }

        // 텍스트 박스와 배경을 비활성화 (초기 상태)
        textComponent.gameObject.SetActive(false);
        background.SetActive(false); // Background 비활성화

        // 텍스트 박스 부모를 roroTransform에 설정
        //textComponent.transform.parent.parent = roroTransform;

        StartCoroutine(ChangeDialogueRoutine()); // 대사 변경 코루틴 시작
    }

    private IEnumerator ChangeDialogueRoutine()
    {
        // 초기 대기 시간
        yield return new WaitForSeconds(initialWaitTime);

        while (currentDialogueIndex < dialogues.Length)
        {
            // 텍스트 박스와 배경 활성화
            background.SetActive(true); // Background 활성화
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
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPosition, Camera.main, out Vector2 localPoint);
            localPoint.y += 50f; // verticalOffset 적용

            // TextBox 위치 설정
            autoSizeTextBox.textBox.localPosition = localPoint;

            // 대사 시간 동안 대기
            int count = 0;
            while (count < dialogueDurations[currentDialogueIndex] * 100)
            {
                Vector3 screenPoint = Camera.main.WorldToScreenPoint(roroTransform.position + new Vector3(-40, 100, 0));
                textComponent.transform.parent.position = screenPoint;
                yield return new WaitForSeconds(0.01f);
                count++;
            }

            // 텍스트 박스와 배경 비활성화
            textComponent.gameObject.SetActive(false);
            background.SetActive(false); // Background 비활성화

            // 인덱스 업데이트 (다음 대사로 순환)
            currentDialogueIndex++;
        }

        // 마지막 대사 후 1.5초 대기 후 씬 전환
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("LaboratoryScene");
    }
}
