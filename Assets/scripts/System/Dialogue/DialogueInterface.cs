using System.Collections.Generic;
using UnityEngine;
public interface DialogueDataSwitchable
{
    public Queue<DialogueData> SwitchData(int count);
}