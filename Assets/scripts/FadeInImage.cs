using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class FadeInImage : MonoBehaviour
{
    public Image blackImage;  // 검정색으로 채워진 이미지
    public float fadeDuration = 2.0f;  // 페이드 아웃이 일어나는 시간 (초)
    public float waitTime = 10.0f;  // 페이드가 시작되기 전에 대기할 시간 (초)

    //private void Start()
    //{
    //    // 시작 시 코루틴 호출
    //    StartCoroutine(FadeIn());
    //}

    public void FadeInEffect()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        // 페이드 시작 전에 대기
        yield return new WaitForSeconds(waitTime);

        // 이미지 색상 정보 가져오기
        Color imageColor = blackImage.color;

        // 2초 동안 점점 투명해지게 만들기
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);  // 1에서 0으로 알파값 변화
            blackImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alpha);  // 새로운 알파값 적용
            yield return null;
        }

        // 페이드 아웃 완료 후 완전 투명하게 설정
        blackImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, 0);
    }
}
