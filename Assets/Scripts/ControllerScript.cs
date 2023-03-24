using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    public float moveSpeed; //Player Speed

    private Animator anim; // Animator ���� �ҷ�����
    void Start()
    {
        anim = GetComponent<Animator>(); //anim ���� ����     
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

    //[Header("�̵��ӵ� ����")]
    //[SerializeField][Range(1f, 30f)] float moveSpeed = 20f;

    //void Update()
    //{
        //�̵�Ű: WASD, �����¿� �̵�
    //  moveX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
    //    moveY = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;

    //    transform.position = new Vector2(transform.position.x + moveX, transform.position.y + moveY);
    //}