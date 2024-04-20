using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon; 

    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    private void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        //inputVec.x = Input.GetAxisRaw("Horizontal");
        //inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void OnMove(InputValue value)
    {
        if(!GameManager.instance.isLive)
        {
            return;
        }
        inputVec = value.Get<Vector2>();
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        anim.SetFloat("Speed",inputVec.magnitude);
        if(inputVec.x != 0)
        {
            sprite.flipX = inputVec.x < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!GameManager.instance.isLive)
        {
            return;
        }
        GameManager.instance.health -= Time.deltaTime * 10;

        if(GameManager.instance.health < 0)
        {
            for(int i = 2;i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
