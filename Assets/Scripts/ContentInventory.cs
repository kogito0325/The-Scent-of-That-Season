using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System;
using UnityEngine.U2D;

public class ContentInventory : MonoBehaviour 
{ // Drink1: ����, Drink2: ����, Drink3: ����
    string[] foods = { "Egg", "Soup", "Fruit", "French", "Drink1", "Drink2", "Drink3" };
    GameObject thisFood = null;
    // GameObject Helpbox;

    private void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && thisFood)
        {
            Debug.Log("got " + thisFood.name + '!');
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Array.Exists(foods, x => x == collision.gameObject.tag)) //�ݶ��̴� ���� �ȿ� ���� �� ���� ��������Ʈ ũ�� ���� 1
        {
            thisFood = collision.gameObject;
            thisFood.transform.localScale *= 4f / 3f;

            GameObject.Find("Helpbox").transform.Find(thisFood.name + "Box").gameObject.SetActive(true);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //�ݶ��̴� ���� �ȿ� ���� �� ���� ��������Ʈ ũ�� ���� 2
    {
        if (Array.Exists(foods, x => x == collision.gameObject.tag))
        {
            thisFood.transform.localScale *= 3f / 4f;
            GameObject.Find("Helpbox").transform.Find(thisFood.name + "Box").gameObject.SetActive(false);
            thisFood = null;
        }
    }
}


