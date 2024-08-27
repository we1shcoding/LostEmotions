using System.Collections;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    public TextMeshProUGUI uiText; // TextMeshProUGUI 타입으로 변경
    public float displayDelay = 3f; // 각 메시지 사이의 지연 시간
    public float displayDelay2 = 2f; // 각 메시지 사이의 지연 시간
    public float displayDelay3 = 1f; // 각 메시지 사이의 지연 시간
    public float typingSpeed = 0.03f; // 타이핑 속도
    public AutoSizeTextBox autoSizeTextBox; // AutoSizeTextBox 스크립트 참조
    public GameObject roro; // roro 개체 참조
    public float moveDistance = 3f; // roro의 이동 거리
    public float extendedMoveDistance = 6f; // roro의 더 멀리 이동할 거리
    public float extendedMoveDistance1 = 4f; // roro의 더 멀리 이동할 거리
    public float extendedMoveDistance2 = 3f; // roro의 더 멀리 이동할 거리
    public float extendedMoveDistance3 = 2f; // roro의 더 멀리 이동할 거리
    public float extendedMoveDistance4 = 1f; // roro의 더 멀리 이동할 거리
    public float extendedMoveDistance5 = 6.5f; // roro의 더 멀리 이동할 거리
    public float moveDuration = 3f; // roro의 이동에 걸리는 시간
    public float shakeDuration = 1f; // roro가 좌우로 흔들리는 시간
    public GameObject textBoxObject; // TextBox GameObject 참조
    public float textBoxOffsetY = 2f; // TextBox가 roro 위에 위치할 오프셋

    private Animator roroAnimator; // Animator 컴포넌트 참조
    private CanvasGroup canvasGroup; // CanvasGroup 참조
    public SceneSwitcher sceneSwitcher;

    void Start()
    {
        if (textBoxObject == null)
        {
            Debug.LogError("TextBox not assigned. Please assign TextBox GameObject.");
            return;
        }

        roroAnimator = roro.GetComponent<Animator>();

        // CanvasGroup을 TextBox에 추가하거나 가져옵니다.
        canvasGroup = textBoxObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = textBoxObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f; // 시작할 때 TextBox를 투명하게 설정

        StartCoroutine(DisplayTextSequence());
    }

    void Update()
    {
        if (textBoxObject != null && roro != null)
        {
            Vector3 roroPosition = roro.transform.position;
            float newX = roroPosition.x - 0.5f; // TextBox를 왼쪽으로 1 단위 이동
            float newY = roroPosition.y + 1.2f; // TextBox를 아래로 2 단위 이동
            textBoxObject.transform.position = new Vector3(newX, newY, textBoxObject.transform.position.z);
        }
    }

    IEnumerator DisplayTextSequence()
    {
        // 첫 번째 roro 이동 (확장된 거리로 이동)
        yield return StartCoroutine(MoveRoroX(extendedMoveDistance));

        // 첫 번째 메시지 표시
        yield return StartCoroutine(DisplayMessage("위험 요소 탐색중 ..."));

        // 두 번째 roro 이동 (기본 거리로 이동)
        yield return StartCoroutine(MoveRoroX(moveDistance));
        yield return new WaitForSeconds(displayDelay);

        // 두 번째 메시지 표시
        yield return StartCoroutine(DisplayMessage("비정상적 생명체 감지!"));

        // roro 애니메이션 재생
        yield return StartCoroutine(PlayAnimation("PlayRoroWow", displayDelay3));

        // roro가 좌우로 흔들리게 함
        yield return StartCoroutine(ShakeRoro());

        // 세 번째 메시지 표시
        yield return StartCoroutine(DisplayMessage("쓰레기장에서 인간 발견!"));
        yield return new WaitForSeconds(displayDelay3);
        yield return StartCoroutine(ShakeRoro());

        // 네 번째 메시지 표시
        yield return StartCoroutine(MoveRoroX(extendedMoveDistance));
        yield return StartCoroutine(DisplayMessage("인간 추가 분석 필요."));
        yield return new WaitForSeconds(displayDelay3);

        // 마지막 메시지 표시 및 Y축 이동
        yield return StartCoroutine(MoveRoroY(-extendedMoveDistance3));
        yield return StartCoroutine(DisplayMessage("연구 대상 데려가겠습니다."));

        // roro 애니메이션 재생 후 LaboratoryScene으로 이동
        yield return StartCoroutine(PlayAnimation("PlayRoroDud", displayDelay3));

        // LaboratoryScene으로 이동
        if (sceneSwitcher != null)
        {
            sceneSwitcher.LaboratoryScence();
        }
        else
        {
            Debug.LogError("SceneSwitcher is not assigned.");
        }

    }

    IEnumerator DisplayMessage(string message)
    {
        // TextBox 투명도를 서서히 1로 설정
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f, 0.5f));

        uiText.text = "";  // 이전 텍스트 초기화
        int totalLength = message.Length;

        for (int i = 0; i < totalLength; i++)
        {
            uiText.text = message.Substring(0, i + 1);

            if (autoSizeTextBox != null)
            {
                autoSizeTextBox.AdjustTextBoxSize();  // 텍스트 박스 크기 조정
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(displayDelay2);
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cg.alpha = endAlpha;
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
            yield return null;
        }

        roro.transform.position = targetPosition;
    }

    IEnumerator MoveRoroY(float distance)
    {
        if (roro == null)
        {
            Debug.LogError("roro GameObject is not assigned.");
            yield break;
        }

        Vector3 startPosition = roro.transform.position;
        Vector3 targetPosition = startPosition + new Vector3(0, distance, 0);
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            roro.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        roro.transform.position = targetPosition;
    }

    IEnumerator ShakeRoro()
    {
        if (roro == null)
        {
            Debug.LogError("roro GameObject is not assigned.");
            yield break;
        }

        float elapsedTime = 0f;
        Vector3 originalPosition = roro.transform.position;
        float shakeAmount = 0.1f;
        Vector3 originalScale = roro.transform.localScale;

        while (elapsedTime < shakeDuration)
        {
            float offset = Mathf.Sin(elapsedTime * Mathf.PI * 2f / shakeDuration) * shakeAmount;
            roro.transform.position = originalPosition + new Vector3(offset, 0, 0);

            if (offset > 0)
            {
                roro.transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            else
            {
                roro.transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        roro.transform.position = originalPosition;
        roro.transform.localScale = originalScale;
    }

    IEnumerator PlayAnimation(string triggerName, float delay)
    {
        if (roroAnimator != null)
        {
            roroAnimator.SetTrigger(triggerName);
            yield return new WaitForSeconds(delay);
            roroAnimator.ResetTrigger(triggerName);
        }
    }
}
