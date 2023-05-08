using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * ��Ʈ�ι̳븦 �����ϴ� ��ư ������Ʈ
 * 
 * ����Ʈ�� �߰��Ǿ� Ŭ���ϸ� ���� ����� ����
 * 
 * InitBlock(num)
 * GetTetromino()
 */

public class TetroBtn : MonoBehaviour
{
    public int eventNum;  // ���� ��ȣ(ChapterTable {CHAPTER})(0:�˹�)
    public string eventName;  // �̺�Ʈ �̸�(ChapterTable {NAME})
    public int space;  // ����� ��ġ�ϱ� ���� �ʿ��� ĭ ��(ChapterTable {SPACE})
    public int money;  // �̺�Ʈ�� �����ϱ� ���� ��(ChapterTable {MONEY})

    public Text nameTxt;  // eventName -> Text
    public Text spTxt;  // space -> Text
    public Text mTxt;  // money -> Text

    public GameObject tetromino;  // ��ư Ŭ�� �� ���� �� ��� ������
    private GameObject scheduleManager;  // ������ �Ŵ���


    private void Awake()
    {
        scheduleManager = GameObject.Find("ScheduleManager");
    }

    public void InitBlock(int num)
    {   // �̺�Ʈ ��ȣ�� �ް� �̴ϼȶ���¡
        // ScheduleManager.InitSchedule()
        // ScheduleManager.ResetTetro()
        eventNum = num;

        tetromino = scheduleManager.GetComponent<ScheduleManager>().tetroPrefabs[num];
        eventName = GameManager.Instance.chapterTable[num]["NAME"].ToString();
        space = 4;
        money = int.Parse(GameManager.Instance.chapterTable[num]["MONEY"].ToString());

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
        GameObject newTetro = Instantiate(tetromino, Input.mousePosition, Quaternion.identity);
        newTetro.GetComponent<TetroScript>().eventNum = eventNum;
        GameManager.Instance.money -= money;

        // �˹� ��� ����ó��
        if (eventNum == 0)
        {
            newTetro.GetComponent<SpriteRenderer>().color = Color.black;
            GameManager.Instance.money += money*2;
        }

        scheduleManager.SendMessage("UpdateMoney");
        newTetro.GetComponent<TetroScript>().SwitchSize();
        scheduleManager.GetComponent<ScheduleManager>().nowTetro = newTetro;

        // �˹� ����� �ƴ϶�� Ŭ�� �� �̺�Ʈ ��Ͽ��� ����
        if (eventNum > 0)
            Destroy(gameObject);

        // �̺�Ʈ ����Ʈ �ݱ�
        GameObject.Find("ListPanel").SetActive(false);
    }
}
