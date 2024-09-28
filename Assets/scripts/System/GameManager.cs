using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /** ---         Fields          ----- */
    #region Fields
    private int dialogueIndex = 1;
    private int lineIndex = 1;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private AnimatorController[] playerAnimation;
    [SerializeField]
    private FadeInImage fadeInImage;
    #endregion

    /** --- Unity Callback Methods  ----- */
    #region Unity Callback Methods
    // Start is called before the first frame update
    void Start()
    {
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    /** ---         Methods         ----- */
    #region Methods
    public void AddLineIndex()
    {
        lineIndex++;

        switch(dialogueIndex)
        {
            case 1:
                switch (lineIndex)
                {
                    case 11:
                        StartCoroutine(FadeInScreen());    
                        break;
                }
                break;

            case 2:
                switch (lineIndex)
                {            
                    case 2: break;
                }
                break;
        }
    }

    public void AddDialogueIndex()
    {
        dialogueIndex++;
        lineIndex = 1;
    }

    public void StartDialogue()
    {
        GetComponent<ShowDialogueUI>().ShowDialogue(dialogueIndex);
    }
    #endregion

    /** ---         Coroutine       ----- */
    #region Coroutine
    IEnumerator FadeInScreen()
    {
        fadeInImage.FadeInEffect();
        yield return new WaitForSeconds(2f);
        player.transform.GetComponent<Animator>().runtimeAnimatorController = playerAnimation[0];
        yield return new WaitForSeconds(1.5f);
        StartDialogue();
    }
    #endregion
}
