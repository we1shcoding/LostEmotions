using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "DialogueData", menuName = "ScriptableObject/DialogueData")]
public class DialogueData : ScriptableObject
{
    public string name;
    public string content;

    // 이름을 통해 캐릭터의 스프라이트 주소를 문자열 형태로 가져오는 메소드
    public string GetSpriteAddress()
    {
        return CharacterSpriteAddresses.GetSpriteAddress(name);
    }
}
