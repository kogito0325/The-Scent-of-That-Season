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
        
        eventName = "������ ù���� " + eventNum.ToString();
        space = 4;
        money = eventNum * 10000;

        mTxt.text = "�ʿ�ݾ�: " + money.ToString() + "��";
        if (num == 0)
        {
            eventName = "�Ƹ�����Ʈ";
            money = 10000;
            mTxt.text = "ȹ��ݾ�: " + money.ToString() + "��";
        }

        spTxt.text = "���೯¥: " + space.ToString() + "ĭ";
        nameTxt.text = eventName;
    }
    public void GetTetromino()
    {   // �̺�Ʈ Ŭ���ϸ� �� ����
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
        gameObject.SetActive(false); // ���� �Ǹ� �̺�Ʈ ��Ͽ��� ����
    }
}
