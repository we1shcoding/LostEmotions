using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    // 플레이어 속도, 점프
    public float maxSpeed;
    public float jumpPower;

    // 대쉬
    public float dashPower; // 대쉬 파워 추가
    public float dashCooldown; // 대쉬 쿨다운 시간
    private bool canDash = true; // 대쉬 가능 여부
    private bool isDashing = false; // 대쉬 상태 여부

    // 공격
    private float curTime;
    public float coolTime = 0.5f;
    public Transform pos;//오른쪽방향
    public Transform posLeft;
    public Vector2 boxSize;
    public float invincibleDuration = 0.5f;//공격무적

    //피격 중복방지
    private bool isHurt = false;

    // 콤보 공격
    private int comboStep = 0;
    private float comboResetTime = 1f;
    private float lastAttackTime;
    private bool atkButton;
    private bool isAttacking;
    int damages;
    


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping") && !isAttacking)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        // Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        // 방향 전환
        //if (Input.GetButtonDown("Horizontal"))
        //    spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;

        //방향전환 버튼 동시 눌림 방지
        if (Input.GetButton("Horizontal")&&!isAttacking)
        {
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }

        }

        // 걷기 애니메이션
        if (rigid.velocity.normalized.x == 0)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);

        //콤보 공격
        HandleComboAttack();

        if (Input.GetKeyUp(KeyCode.Z))
        {
            atkButton = false;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
        Gizmos.DrawWireCube(posLeft.position, boxSize);
    }



    void FixedUpdate()
    {
        

        if (!isAttacking)
        {
            // Move By Key Control
            float h = Input.GetAxisRaw("Horizontal");
            
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            // Max Speed
            if (!isDashing) // 대쉬 중이 아닐 때만 속도 제한 적용
            {
                if (rigid.velocity.x > maxSpeed) // 오른쪽 맥스 스피드
                {
                    rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
                }
                else if (rigid.velocity.x < maxSpeed * (-1)) // 왼쪽 맥스 스피드, 왼쪽이니까 -1로 곱함
                {
                    rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
                }
            }
        }

        // Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1.0f)
                    anim.SetBool("isJumping", false);
            }
        }
    }

    // Dash Coroutine
    IEnumerator Dash()
    {
        canDash = false; // 대쉬 불가 상태로 설정
        isDashing = true; // 대쉬 상태로 설정
        float originalGravity = rigid.gravityScale; // 원래 중력 값 저장
        rigid.gravityScale = 0; // 대쉬 동안 중력 무시
        float dashDirection = spriteRenderer.flipX ? 1 : -1; // 대쉬 방향 설정

        rigid.velocity = new Vector2(dashDirection * dashPower, 0); // 대쉬 속도 설정

        yield return new WaitForSeconds(0.2f); // 대쉬 지속 시간

        rigid.gravityScale = originalGravity; // 원래 중력 값 복원
        isDashing = false; // 대쉬 상태 해제
        yield return new WaitForSeconds(dashCooldown - 0.2f); // 쿨다운 시간
        canDash = true; // 대쉬 가능 상태로 설정
    }

    // 플레이어가 몬스터에게 닿았을 때
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "monster")
        {
            OnDamaged(collision.transform.position);
        }
    }

    // 맞았을 때 잠깐 무적
    void OnDamaged(Vector2 targetPos)
    {
        if (!isHurt)
        {
            isHurt = true;

            gameObject.layer = 6; // PlayerDamaged 레이어 번호

            // 피격 됐을 때 플레이어 색상
            spriteRenderer.color = new Color(1, 1, 1, 0.4f); // R G B 순인데 맨 뒤는 투명도 값

            // 피격됐을 때 튕겨나감
            int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

            // 피격 애니메이션
            anim.SetTrigger("Damaged");

            // 시간차를 주기 위한 Invoke 사용
            Invoke("OffDamaged", 2);
        }

    }

    IEnumerator invincible()
    {//공격무적
        isHurt = true;
        yield return new WaitForSeconds(invincibleDuration);
        isHurt = false;
    }

    //공격대쉬
    IEnumerator Adash()
    {
        float dashDirection = spriteRenderer.flipX ? 1 : -1; // 대시 방향 설정
        float originalGravity = rigid.gravityScale; // 원래 중력 값 저장
        rigid.gravityScale = 0; // 대시 동안 중력 무시
        float dashDistance = dashPower * 1;
        rigid.velocity = new Vector2(dashDirection * dashDistance, 0); // 대시 속도 설정

        yield return new WaitForSeconds(0.2f); // 대시 지속 시간

        rigid.velocity = Vector2.zero; // 대시 종료 후 속도 초기화
        rigid.gravityScale = originalGravity; // 원래 중력 값 복원
    }

    // 무적 모드 해제
    void OffDamaged()
    {
        gameObject.layer = 3;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        isHurt = false;
    }


    //콤보
    void HandleComboAttack()
    {
        float timeSinceLastAttack = Time.time - lastAttackTime;

        if (timeSinceLastAttack > comboResetTime)
        {
            comboStep = 0;
        }

        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.Z)&&!atkButton)
            {
                atkButton = true;
                comboStep++;
                lastAttackTime = Time.time;

                if (comboStep > 2)
                {
                    comboStep = 1;
                }

                anim.SetInteger("ComboIndex", comboStep);

                Vector2 posAttack = spriteRenderer.flipX ? pos.position : posLeft.position;
                bool monsterInRange = false;

                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(posAttack, boxSize, 0);
                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider.tag == "monster")
                    {//콤보별로 데미지 바꾸려면 여기서 if문으로 하면 될듯...?
                        if (comboStep == 1||comboStep==0) damages = Random.Range(1, 5);
                        else if (comboStep == 2) damages = Random.Range(3, 7);

                        Vector2 direction = (collider.transform.position - transform.position).normalized;

                        collider.GetComponent<purple_slime_move>().TakeDamage(damages, direction);
                        monsterInRange = true;
                    }
                }

                StartCoroutine(invincible());

                if (!monsterInRange && (comboStep==0 || comboStep==1)) //앞으로 치는 첫번째 공격에서만 대쉬
                {
                    StartCoroutine(Adash());
                }

                anim.SetTrigger("attack");
                curTime = coolTime;
                
                isAttacking = true;
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }

        if (isAttacking && curTime <= 0)
        {
            isAttacking = false;
        }
        
    }


}
