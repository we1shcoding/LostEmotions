using UnityEngine;
using TMPro;

public class AutoSizeTextBox : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // 텍스트 컴포넌트
    public RectTransform textBox; // 텍스트 박스의 RectTransform
    public RectTransform background; // 배경의 RectTransform

    public float padding = 20f; // 여백 추가

    void Start()
    {
        // 할당된 컴포넌트 확인
        if (textComponent == null)
        {
            Debug.LogError("TextComponent is not assigned.");
            return;
        }

        if (textBox == null)
        {
            Debug.LogError("TextBox is not assigned.");
            return;
        }

        if (background == null)
        {
            Debug.LogError("Background is not assigned.");
            return;
        }

        // AdjustTextBoxSize는 텍스트가 변경될 때마다 호출될 것입니다.
    }

    public void AdjustTextBoxSize()
    {
        if (textComponent != null && textBox != null && background != null)
        {
            // 텍스트의 최대 너비와 높이 얻기
            Vector2 preferredSize = textComponent.GetPreferredValues();

            // 여백을 추가하여 크기 조정
            Vector2 newSize = new Vector2(preferredSize.x + padding, preferredSize.y + padding);

            // 텍스트 박스와 배경의 크기 조정
            textBox.sizeDelta = newSize;
            background.sizeDelta = newSize;

            // 크기 조정이 완료된 후 확인
            Debug.Log($"Adjusted Size: {newSize}, TextBox SizeDelta: {textBox.sizeDelta}, Background SizeDelta: {background.sizeDelta}");
        }
        else
        {
            Debug.LogError("TextComponent, TextBox or Background is not assigned.");
        }
    }
}
