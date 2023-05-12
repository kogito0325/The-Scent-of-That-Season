using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ContentInventory : MonoBehaviour 
{ // Drink1: 양주, Drink2: 맥주, Drink3: 소주
    string[] foods = { "Egg", "Soup", "Fruit", "French", "Drink1", "Drink2", "Drink3" };
    GameObject thisFood = null;
    public Image[] images;
    public int life;
    // GameObject Helpbox;

    private void Start()
    {
        images[0].gameObject.SetActive(false);
        images[1].gameObject.SetActive(false);
        life = 10;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && thisFood)
        {
            Debug.Log("got " + thisFood.name + '!');
            if (thisFood.name[0] == 'D')
            {
                images[1].gameObject.SetActive(true);
                images[1].sprite = thisFood.GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                images[0].gameObject.SetActive(true);
                images[0].sprite = thisFood.GetComponent<SpriteRenderer>().sprite;
            }
        }

        if (life <= 0)
        {
            GameManager.Instance.PopTask();
            GameManager.Instance.CheckNextMonth();
            GameManager.Instance.LoadScheduleScene();
            Destroy(this.gameObject);
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

    private void OnTriggerExit2D(Collider2D collision) //콜라이더 범위 안에 벗어났을 때 음식 스프라이트 크기 변경 2
    {
        if (Array.Exists(foods, x => x == collision.gameObject.tag))
        {
            thisFood.transform.localScale *= 3f / 4f;
            GameObject.Find("Helpbox").transform.Find(thisFood.name + "Box").gameObject.SetActive(false);
            thisFood = null;
        }
    }
}


