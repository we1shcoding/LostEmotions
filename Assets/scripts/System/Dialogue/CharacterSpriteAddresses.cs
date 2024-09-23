using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterSpriteAddresses
{
    // 캐릭터 이름 - 캐릭터 스프라이트 주소
    private static readonly Dictionary<string, string> spriteAddress = new Dictionary<string, string>
    {
        {"A", "Character/TestImage" }
    };

    // 이름으로 검색해서 스프라이트 주소 반환
    public static string GetSpriteAddress(string name)
    {
        if (spriteAddress.TryGetValue(name, out string address))
        {
            return address;
        }
        return "Character/Empty";
    }
}
