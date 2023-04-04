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
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            anim.SetInteger("MoveCondition", 1);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            anim.SetInteger("MoveCondition", 2);
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            anim.SetInteger("MoveCondition", 3);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            anim.SetInteger("MoveCondition", 4);
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            anim.SetInteger("MoveCondition", 0);

        if (anim.GetInteger("MoveCondition") == 1 || anim.GetInteger("MoveCondition") == 2)
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
        else if (anim.GetInteger("MoveCondition") == 3 || anim.GetInteger("MoveCondition") == 4)
            transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
        // x = Horizontal, y = Vertical, z = 3D 일때만(앞뒤)
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


 