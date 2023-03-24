using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    public float moveSpeed; //Player Speed

    private Animator anim; // Animator 변수 불러오기
    void Start()
    {
        anim = GetComponent<Animator>(); //anim 변수 선언     
    }
    void Update()
    {
        if(Input.GetAxisRaw("Horizontal") > 0f || Input.GetAxisRaw("Horizontal") < 0f) // Left, Right Move
        {
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
            anim.SetBool("notmove", false);
        }
        else if (Input.GetAxisRaw("Vertical") > 0f || Input.GetAxisRaw("Vertical") < 0f) // Up, Down Move
        {
            transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
            anim.SetBool("notmove", false);
        }
        else
        {
            anim.SetBool("notmove", true);
        }

        // Animation Move X, Move Y
        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
    }

}


    //float moveX, moveY;

    //[Header("이동속도 조절")]
    //[SerializeField][Range(1f, 30f)] float moveSpeed = 20f;

    //void Update()
    //{
        //이동키: WASD, 상하좌우 이동
    //  moveX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
    //    moveY = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;

    //    transform.position = new Vector2(transform.position.x + moveX, transform.position.y + moveY);
    //}