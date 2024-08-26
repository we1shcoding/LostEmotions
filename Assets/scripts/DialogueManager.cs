using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//text 사용하려면 가져와야 할 라이브러리

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager instance;


    public Text text;//글씨
    public SpriteRenderer rendererSprite;//그림
    public SpriteRenderer rendererDialogueWindow;//대사창

    private List<string> listSentences;//글씨 리스트
    private List<Sprite> listSprites;//그림 리스트
    private List<Sprite> listDialogueWindow;//대사창 리스트
    //커스텀 클래스의 배열을 리스트에 넣을 예정

    private int count;//대화 진행 상황 카운트

    public Animator animSprite;//그림 등장
    public Animator animDialogueWindow;//대사창 등장

    //public Image moveCursar; //??이건 뭐지?

    public Animator animCursar;

    public string typeSound;//대사 소리
    public string enterSound;//스킵 소리

    // private AudioManager theAudio;

    public bool talking = false;
    private bool KeyActivated = false;
    private bool onlyText = false;

    // Start is called before the first frame update
    void Start()
    {//초기화
        count = 0;
        text.text = "";
        listSentences = new List<string>();
        listSprites = new List<Sprite>();
        listDialogueWindow = new List<Sprite>();
        // theAudio = FindObjectOfType<AudioManager>();
    }

    //다이얼로그 나오게 하는 함수
    public void ShowDialogue(Dialogue dialogue)//인수로 커스텀 클래스
    {
        onlyText = false;
        talking = true;

        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            listSentences.Add(dialogue.sentences[i]);
            listSprites.Add(dialogue.sprites[i]);
            listDialogueWindow.Add(dialogue.dialogueWindows[i]);//없어서 오류나는 건가? 맞네 미친

        }
        animSprite.SetBool("Appear", true);
        animDialogueWindow.SetBool("Appear", true);
        animCursar.SetBool("cursarMove", true);
        StartCoroutine(StartDialogueCoroutine());
    }

    public void ShowText(Dialogue dialogue)//인수로 커스텀 클래스
    {//택스트 출력
        onlyText = true;
        talking = true;

        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            listSentences.Add(dialogue.sentences[i]);
        }
        animDialogueWindow.SetBool("Appear", true);
        animCursar.SetBool("cursarMove", true);
        StartCoroutine(StartTextCoroutine());
    }

    public void ExitDialogue()
    {
        count = 0;
        text.text = "";
        listSentences.Clear();//문장
        listSprites.Clear();//그림
        listDialogueWindow.Clear();//대사창
        animSprite.SetBool("Appear", false);
        animDialogueWindow.SetBool("Appear", false);
        animCursar.SetBool("cursarMove", false);
        talking = false;
    }


    IEnumerator StartTextCoroutine()
    {//텍스트만 출력됨 오로지 텍스트. 다른게 없는데 대체 왜 텍스트만?
        KeyActivated = true;
        for (int i = 0; i < listSentences[count].Length; i++)
        {//문장 출력하기
            text.text += listSentences[count][i];//1글자씩 출력
            if (i % 7 == 1)
            {
                // theAudio.Play(typeSound);
            }
            yield return new WaitForSeconds(0.01f);//대기 0.01초
        }
    }

    //코르틴
    IEnumerator StartDialogueCoroutine()
    {
        //그림 교체 확인

        if (count > 0)
        {//count-1오류 잡기 위해서 추가한 조건
            if (listDialogueWindow[count] != listDialogueWindow[count - 1])
            {//두 대사창이 다르다면(이름)
                animSprite.SetBool("Change", true);//교체에서는 투명도 조절
                animDialogueWindow.SetBool("Appear", false);//사라졌다가 다시 나타나도록
                yield return new WaitForSeconds(0.2f);//0.2초 기다리기
                rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindow[count];//대사창 교체
                rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];//그림 교체
                animDialogueWindow.SetBool("Appear", true);//나타나기
                animSprite.SetBool("Change", false);//다시 false로 바꾸기
            }
            else
            {
                if (listSprites[count] != listSprites[count - 1])
                {//두 그림이 다르다면

                    animSprite.SetBool("Change", true);//투명도 조절                
                    yield return new WaitForSeconds(0.1f);//0.1초 기다리기                
                    rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
                    animSprite.SetBool("Change", false);
                }
                else
                {//그림, 대사창 모두 같다면 그냥 기다리기
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
        else
        {//이미지 교체(처음 이미지는 무조건 교체 되니까)
            rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindow[count];
            rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
        }

        KeyActivated = true;
        for (int i = 0; i < listSentences[count].Length; i++)
        {//문장 출력하기
            text.text += listSentences[count][i];//1글자씩 출력
            if (i % 7 == 1)
            {
                // theAudio.Play(typeSound);
            }
            yield return new WaitForSeconds(0.01f);//대기 0.01초
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (talking && KeyActivated)
        {
            if (Input.GetButtonDown("Jump"))//스페이스가 눌렸다
            {
                KeyActivated = false;
                count++;
                text.text = "";//문장 초기화
                // theAudio.Play(enterSound);

                if (count == listSentences.Count)
                {//count값이 가진 리스트 크기와 같다면
                    StopAllCoroutines();//코르틴 끝내기
                    ExitDialogue();//다이얼로그 끄기
                }
                else
                {
                    StopAllCoroutines();//일단 끄고
                    if (onlyText)
                    {
                        StartCoroutine(StartTextCoroutine());
                    }
                    else
                    {
                        StartCoroutine(StartDialogueCoroutine());//다시 시작
                    }
                }
            }
        }
    }

}