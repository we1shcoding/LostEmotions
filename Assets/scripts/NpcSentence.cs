using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSentence : MonoBehaviour
{
    public string[] sentences; // NPC 대사를 담을 string 배열
    public Transform chatTr; // 말풍선 NPC 머리 위에 생성, 말풍선 생성 위치
    public GameObject chatBoxPrefab; // ChatBox 복사하기 위함

    private void Start()
    {

    }
    public void TalkNpc()
    {
        GameObject go = Instantiate(chatBoxPrefab);
        go.GetComponent<ChatSystem>().Ondialogue(sentences);
    }
}
