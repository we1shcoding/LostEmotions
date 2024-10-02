using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>();
    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null;
    Vector2[] timingBoxs = null;

    // Start is called before the first frame update
    void Start()
    {
        // 타이밍 박스 설정
        timingBoxs = new Vector2[timingRect.Length];

        for (int i = 0; i < timingRect.Length; i++)
        {
            // 타이밍 박스의 월드 좌표로 변환
            Vector3[] worldCorners = new Vector3[4];
            timingRect[i].GetWorldCorners(worldCorners);

            // 타이밍 박스를 월드 좌표로 설정
            timingBoxs[i].Set(worldCorners[0].x, worldCorners[2].x);
        }
    }

    public void CheckTiming()
    {
        for (int i = boxNoteList.Count - 1; i >= 0; i--) // 역순으로 순회하여 RemoveAt 문제 방지
        {
            float t_notePosX = boxNoteList[i].transform.position.x;

            for (int x = 0; x < timingBoxs.Length; x++)
            {
                if (timingBoxs[x].x <= t_notePosX && t_notePosX <= timingBoxs[x].y)
                {
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i); // 인덱스 감소 관리
                    Debug.Log("Hit " + x);
                    return;
                }
            }
        }
        Debug.Log("Miss");
    }
}
