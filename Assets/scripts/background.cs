using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    public float speed;//배경 속도
    public Transform[] backgrounds;//배경 담을 변수

    float leftPosX = 0f;//배경 끝 x좌표
    float rightPosX = 0f;//배경 시작 x좌표
    float xScreenHalfSize;//게임 화면의 x좌표 절반
    float yScreenHalfSize;//게임 화면의 y좌표 절반
    //스크롤링 변수

    // Start is called before the first frame update
    void Start()
    {//초기화
        yScreenHalfSize = Camera.main.orthographicSize;
        xScreenHalfSize = yScreenHalfSize * Camera.main.aspect;

        leftPosX = -(xScreenHalfSize * 2);
        rightPosX = xScreenHalfSize * 2 * backgrounds.Length;

    }
    public void OnRightMove()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].position += new Vector3(-speed, 0, 0) * Time.deltaTime;

            if (backgrounds[i].position.x < leftPosX)
            {
                Vector3 nextPos = backgrounds[i].position;
                nextPos = new Vector3(nextPos.x + rightPosX, nextPos.y, nextPos.z);
                backgrounds[i].position = nextPos;
            }
        }
    }

    public void OnLeftMove()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].position += new Vector3(speed, 0, 0) * Time.deltaTime;

            if (backgrounds[i].position.x > -leftPosX)
            {
                Vector3 nextPos = backgrounds[i].position;
                nextPos = new Vector3(nextPos.x - rightPosX, nextPos.y, nextPos.z);
                backgrounds[i].position = nextPos;
            }
        }
    }


// Update is called once per frame
void Update()
    {//배경들의 좌표를 speed만큼 계속 이동
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].position += new Vector3(-speed, 0, 0) * Time.deltaTime;

            if (backgrounds[i].position.x < leftPosX)//이동 중 x좌표가 배경 끝 x좌표보다 작아짐=배경이 사라짐
            {//배경 x좌표를 rightPosX(배경 시작 좌표)만큼 이동시켜 다시 화면의 시작에 위치하도록 배치
                Vector3 nextPos = backgrounds[i].position;
                nextPos = new Vector3(nextPos.x + rightPosX, nextPos.y, nextPos.z);
                backgrounds[i].position = nextPos;
            }
        }

    }

}
