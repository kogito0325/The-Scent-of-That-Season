using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System;
using UnityEngine.U2D;

public class ContentInventory : MonoBehaviour 
{ // Drink1: 양주, Drink2: 맥주, Drink3: 소주
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
        if (Array.Exists(foods, x => x == collision.gameObject.tag)) //콜라이더 범위 안에 들어갔을 때 음식 스프라이트 크기 변경 1
        {
            thisFood = collision.gameObject;
            thisFood.transform.localScale *= 4f / 3f;

            GameObject.Find("Helpbox").transform.Find(thisFood.name + "Box").gameObject.SetActive(true);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //콜라이더 범위 안에 들어갔을 때 음식 스프라이트 크기 변경 2
    {
        if (Array.Exists(foods, x => x == collision.gameObject.tag))
        {
            thisFood.transform.localScale *= 3f / 4f;
            GameObject.Find("Helpbox").transform.Find(thisFood.name + "Box").gameObject.SetActive(false);
            thisFood = null;
        }
    }
}


