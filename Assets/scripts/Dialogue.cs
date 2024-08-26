using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//커스텀 클래스
[System.Serializable]
public class Dialogue
{
    [TextArea(1, 2)]
    public string[] sentences;//문장 배열
    public Sprite[] sprites;//그림 배열
    public Sprite[] dialogueWindows;//대사창 배열(이름같은거)
}
