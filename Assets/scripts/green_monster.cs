using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class green_monster : MonoBehaviour
{//이름만 green이고 죽는 모션 있는 코드!!
    Rigidbody2D rigid;
    public int nextMove;
    Animator anim;
    public int Hp;
    private bool isHit;
    public float knockbackForce; // 넉백 힘
    public int rayLen;//레이저 길이

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
    private void Update()
    {//애니메이션 fixedUpdate에서 물리이동이랑 같이 처리하면 딜레이 생겨서 여기에 작성함
        anim.SetInteger("WalkSpeed", nextMove);
        UpdateSpriteDirection();
    }

    void FixedUpdate()
    {
        //Debug.Log(isKnockback);
        if (!isHit && !anim.GetBool("damage"))
        {

            rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

            // 플랫폼 체크
            Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(1, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, rayLen, LayerMask.GetMask("Platform"));

            if (rayHit.collider == null)
            {
                nextMove *= -1;
                // CancelInvoke() : 현재 작동 중인 모든 Invoke 함수를 멈추는 함수
                CancelInvoke();
                Invoke("Think", 3);
            }
        }

    }


    // 행동지표 바꾸기, 재귀함수
    void Think()
    {
        // Range() : 최소 ~ 최대 범위의 랜덤 수 생성 (최대 제외)
        nextMove = Random.Range(-1, 2);
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

    }

    void UpdateSpriteDirection()
    {
        // Flip the sprite based on the movement direction
        if (nextMove != 0)
        {
            //Debug.Log(nextMove);
            Vector3 newScale = transform.localScale;
            newScale.x = Mathf.Abs(newScale.x) * (nextMove > 0 ? -1 : 1);
            transform.localScale = newScale;

        }
    }

    public GameObject hudDamageText;//데미지 텍스트 프리펩
    public Transform hudPos;//데미지 뜰 위치

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        Hp -= damage;
        Debug.Log(Hp);
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;
        hudText.GetComponent<DamageText>().damage = damage;
        //Debug.Log(damage);

        if (Hp <= 0)
        {
            Debug.Log("moster die");
            StartCoroutine(Die());
        }
        else
        {
            StartCoroutine(OnHit(knockbackDirection));
        }

        

    }

    IEnumerator OnHit(Vector2 knockbackDirection)
    {
        isHit = true;

        // isHit 파라미터를 true로 설정하여 피격 애니메이션으로 전환
        anim.SetBool("damage", true);

        // 넉백 적용
        rigid.velocity = Vector2.zero; // 기존 속도 초기화
        rigid.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.4f);

        rigid.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.5f);

        // 애니메이션을 원래 상태로 되돌림
        anim.SetBool("damage", false);

        isHit = false;
    }

    IEnumerator Die()
    {
        // 죽는 애니메이션 트리거 설정
        anim.SetTrigger("Die");

        // 애니메이션 길이만큼 대기
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // 오브젝트 파괴
        Destroy(this.gameObject);
    }
}
