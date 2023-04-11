using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SuvChMoveScript : MonoBehaviour
{
    Animator anim;
    public Sprite[] Foods;
    public Sprite[] Drinks;
    public Transform[] sits;
    public Transform thisSit;
    public SpriteRenderer[] thisImage;
    public Transform[] roots1;
    public Transform[] roots2;
    public Transform[] roots3;
    public Transform[] roots4;
    public Transform[] roots5;
    public Transform[] roots6;
    public Transform[] roots7;
    public Transform[] roots8;
    public Transform[] thisRoot;
    public GameObject foodBox;
    Vector3 startPos;
    Vector3 targetPos;
    float timer = 0.0f;
    float speed = 5f;
    int rootIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("Condition", 3);
        foodBox.SetActive(false);

        rootIdx = UnityEngine.Random.Range(0, 7);
        switch (rootIdx)
        {
            case 0:
                thisRoot = roots1;
                break;
            case 1:
                thisRoot = roots2;
                break;
            case 2:
                thisRoot = roots3;
                break;
            case 3:
                thisRoot = roots4;
                break;
            case 4:
                thisRoot = roots5;
                break;
            case 5:
                thisRoot = roots6;
                break;
            case 6:
                thisRoot = roots7;
                break;
            case 7:
                thisRoot = roots8;
                break;
        }

        rootIdx= 0;
        startPos = new Vector3(10, 3);
        targetPos = thisRoot[rootIdx].position;
        transform.position = startPos;

        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ActivateImage()
    {
        thisImage[0].sprite = Foods[UnityEngine.Random.Range(0, Foods.Length)];
        thisImage[1].sprite = Drinks[UnityEngine.Random.Range(0, Drinks.Length)];
        foodBox.SetActive(true);
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enterd");

        if (other.gameObject.CompareTag("sits"))
        {
            ActivateImage();
        }
    }*/

    IEnumerator Move()
    {
        while (true)
        {
            transform.position = Vector3.MoveTowards(startPos, targetPos, speed * Time.deltaTime);
            startPos = transform.position;
            if (Math.Abs(Vector3.Distance(transform.position, targetPos)) <= 0)
            {
                if (rootIdx+1 >= thisRoot.Length)
                {
                    ActivateImage();
                    Debug.Log("Sit!");
                    break;
                }

                startPos = thisRoot[rootIdx].position;
                targetPos = thisRoot[++rootIdx].position;

                if (targetPos.x < startPos.x)
                    anim.SetInteger("Condition", 3);
                else if (targetPos.y < startPos.y)
                    anim.SetInteger("Condition", 2);
                else if (targetPos.x > startPos.x)
                    anim.SetInteger("Condition", 4);
                else if (targetPos.y > startPos.y)
                    anim.SetInteger("Condition", 1);
            }
            yield return null; //IEnumerator ¾ê Â¦ÁöÀÓ 
        }
    }

}
