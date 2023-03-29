using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Food"))
        {
            GameObject thisFood = collision.gameObject;
            thisFood.transform.localScale *= 4f/3f;
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            GameObject thisFood = collision.gameObject;
            thisFood.transform.localScale *= 3f/4f;
        }
    }

}


 