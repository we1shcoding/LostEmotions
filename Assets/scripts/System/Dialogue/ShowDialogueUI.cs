using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowDialogueUI : MonoBehaviour
{
    
    /** ---         Fields          ----- */
    #region Fields
    public float textDelay;

    [SerializeField]
    private GameObject dialogueUI;
    
    private GameManager gameManager;
    private StringBuilder stringBuilder = new StringBuilder();
    private Queue<DialogueData> dialogues;
    // 타이핑 애니메이션 핸들링을 위한 변수
    private bool isAnimated;
    #endregion

    /** --- Unity Callback Methods ----- */
    #region Unity Callback Methods
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        ShowDialogue();
    }
    #endregion

    /** ---         Methods         ----- */
    #region Methods
    public void ShowDialogue()
    {
        DialogueDataSwitchable dialogueDataSwitchable = new SwitchFileToObject();
        dialogues = dialogueDataSwitchable.SwitchData();

        StartCoroutine(ShowUI());
    }

    // Coroutine
    IEnumerator ShowUI()
    {
        /**
         * - 다이얼로그 UI의 자식 구성
         * 0 : 대사창 배경, 1 : 캐릭터 스프라이트, 2 : 캐릭터 이름 텍스트, 3 : 대사 텍스트
         */
        GameObject UI = Instantiate(dialogueUI);
        DialogueData dialogue;
        int count = dialogues.Count;

        for (int i = 0; i < count; i++)
        {
            dialogue = dialogues.Dequeue();

            // 스프라이트 설정
            UI.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(dialogue.GetSpriteAddress());
            // 이름 텍스트 설정
            UI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = dialogue.name;
            // 대사 텍스트 UI
            TextMeshProUGUI contentTextMesh = UI.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            contentTextMesh.text = "";

            /**
             * 타이핑 애니메이션 
             */
            stringBuilder.Clear();
            isAnimated = true;
            // foreach 문에서 딜레이 되는 동안에도 입력을 감지하기 위한 코루틴
            StartCoroutine(InputDetection());

            // 애니메이션 작업
            foreach (char letter in dialogue.content)
            {
                yield return new WaitForSeconds(textDelay);
                if (!isAnimated) { break; }
                if(letter.Equals('.')) { yield return new WaitForSeconds(textDelay); }
                stringBuilder.Append(letter);
                contentTextMesh.text = stringBuilder.ToString();
            }

            // foreach문 종료 후처리
            StopCoroutine(InputDetection());
            contentTextMesh.text = dialogue.content;

            // 마우스 클릭 연속 감지 방지용 딜레이
            yield return null;

            // 클릭 시 다음 대사로
            while (!Input.GetMouseButtonDown(0))
            {
                yield return null;
            }

            // for문이 다시 시작될 때 마우스 클릭 연속 감지를 방지하기 위한 딜레이
            yield return null;

            // 게임 메니저의 다이얼로그 이벤트 카운트 증가
            gameManager.AddCount();
        }

        Destroy(UI);
    }

    // 입력 감지 코루틴
    IEnumerator InputDetection()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        // 타이핑 애니메이션 정지
        isAnimated = false;
    }
    #endregion
}
