using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class purple_slime_move : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;
    Animator anim;
    public int Hp;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Hp = 50;
        Think();

        // Invoke : 주어진 시간이 지난 뒤, 지정된 함수를 실행하는 함수
        // 재귀함수는 딜레이 없이 쓰면 위험해짐
        Invoke("Think", 3);
    }

    void FixedUpdate()
    {
        //rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        if (nextMove == 0)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
        else
        {
            rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        }

        // 플랫폼 체크
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            nextMove *= -1;

            // CancelInvoke() : 현재 작동 중인 모든 Invoke 함수를 멈추는 함수
            CancelInvoke();
            Invoke("Think", 3);
        }

        // Update the sprite direction
        UpdateSpriteDirection();
    }


    // 행동지표 바꾸기, 재귀함수
    void Think()
    {
        // Range() : 최소 ~ 최대 범위의 랜덤 수 생성 (최대 제외)
        nextMove = Random.Range(-1, 2);

        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

        anim.SetInteger("WalkSpeed", nextMove);

        // Update the sprite direction
        UpdateSpriteDirection();
    }

    void UpdateSpriteDirection()
    {
        // Flip the sprite based on the movement direction
        if (nextMove != 0)
        {
            Vector3 newScale = transform.localScale;
            newScale.x = Mathf.Abs(newScale.x) * (nextMove > 0 ? -1 : 1);
            transform.localScale = newScale;
        }
    }

    public GameObject hudDamageText;//데미지 텍스트 프리펩
    public Transform hudPos;//데미지 뜰 위치

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        //Debug.Log(Hp);
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;
        hudText.GetComponent<DamageText>().damage = damage;
        Debug.Log(damage);
    }
}
