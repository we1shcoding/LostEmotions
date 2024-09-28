using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterSpriteAddresses
{
    // ĳ���� �̸� - ĳ���� ��������Ʈ �ּ�
    private static readonly Dictionary<string, string> spriteAddress = new Dictionary<string, string>
    {
        {"A", "Character/TestImage" },
        {"누군가의 목소리", "Character/TestImage3"},
        {"???", "Character/TestImage2"},
        {"하니", "Character/TestImage2"}
    };

    // �̸����� �˻��ؼ� ��������Ʈ �ּ� ��ȯ
    public static string GetSpriteAddress(string name)
    {
        if (spriteAddress.TryGetValue(name, out string address))
        {
            return address;
        }
        return "Character/Empty";
    }
}
