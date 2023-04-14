using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TetroBtn : MonoBehaviour
{
    public int eventNum;
    public string eventName;
    public int space;
    public int money;

    public Text nameTxt;
    public Text spTxt;
    public Text mTxt;

    public GameObject tetromino;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitBlock(int num)
    {
        eventNum = num;
        
        eventName = "아찔한 첫만남 " + eventNum.ToString();
        space = 4;
        money = eventNum * 10000;

        mTxt.text = "필요금액: " + money.ToString() + "원";
        if (num == 0)
        {
            eventName = "아르바이트";
            money = 10000;
            mTxt.text = "획득금액: " + money.ToString() + "원";
        }

        spTxt.text = "진행날짜: " + space.ToString() + "칸";
        nameTxt.text = eventName;
    }
    public void GetTetromino()
    {   // 이벤트 클릭하면 블럭 생성
        tetromino = Instantiate(tetromino);
        tetromino.GetComponent<TetroScript>().eventNum = eventNum;
        GameManager.Instance.money -= money;
        if (eventNum == 0)
        {
            tetromino.GetComponent<SpriteRenderer>().color = Color.black;
            GameManager.Instance.money += money*2;
        }
        GameObject.Find("ScheduleManager").SendMessage("UpdateMoney");
        tetromino.GetComponent<TetroScript>().SwitchSize();
        GameObject.Find("ListPanel").SetActive(false);
        gameObject.SetActive(false); // 블럭이 되면 이벤트 목록에서 제거
    }
}
