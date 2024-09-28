using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int count = 0;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private AnimatorController[] playerAnimation;
    [SerializeField]
    private FadeInImage fadeInImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCount()
    {
        count++;

        switch(count)
        {
            case 10:
                fadeInImage.FadeInEffect();
                break;
            case 11: 
                player.transform.GetComponent<Animator>().runtimeAnimatorController = playerAnimation[0]; 
                break;
        }
    }
}
