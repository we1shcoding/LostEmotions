using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchFileToObject : DialogueDataSwitchable
 
{
    public Queue<DialogueData> SwitchData()
    {
        Queue<DialogueData> dialogues = new Queue<DialogueData>();
        string filePath = "Assets/DialogueData/";
        string fileExtension = ".txt";
        string sceneName = SceneManager.GetActiveScene().name;
        string[] data = File.ReadAllLines(filePath + sceneName + fileExtension);
       

        for (int i = 0; i < data.Length; i++)
        {
            string[] values = data[i].Split('\t');
            DialogueData dialogueData = ScriptableObject.CreateInstance<DialogueData>();
            dialogueData.name = values[0];
            values[1] = values[1].Replace("\\n", "\u000A");
            dialogueData.content = values[1];

            dialogues.Enqueue(dialogueData);
        }

        return dialogues;
    }
}
