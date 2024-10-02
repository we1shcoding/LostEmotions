using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0; // 1분에 120개 생성한다는 의미
    double currentTime = 0d; // 노트 생성 시간을 체크하는 변수, 오차가 더 작은 더블로 선언
    [SerializeField] Transform tfNoteAppear = null; // 노트가 생성될 위치
    [SerializeField] GameObject goNote = null; // 노트 프리팸을 담을 변수

    TimingManager theTimingManager;

    private void Start()
    {
        theTimingManager = GetComponent<TimingManager>();
    }
    void Update()
    {
        currentTime += Time.deltaTime;

        if (goNote != null && currentTime >= 60d / bpm) // null 체크 추가
        {
            GameObject t_note = Instantiate(goNote, tfNoteAppear.position, Quaternion.identity);
            t_note.transform.SetParent(this.transform);
            theTimingManager.boxNoteList.Add(t_note);
            currentTime -= 60d / bpm;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            theTimingManager.boxNoteList.Remove(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
