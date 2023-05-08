using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * 테트로미노를 생성하는 버튼 오브젝트
 * 
 * 리스트에 추가되어 클릭하면 일정 블록을 생성
 * 
 * InitBlock(num)
 * GetTetromino()
 */

public class TetroBtn : MonoBehaviour
{
    public int eventNum;  // 일정 번호(ChapterTable {CHAPTER})(0:알바)
    public string eventName;  // 이벤트 이름(ChapterTable {NAME})
    public int space;  // 블록을 배치하기 위해 필요한 칸 수(ChapterTable {SPACE})
    public int money;  // 이벤트를 진행하기 위한 돈(ChapterTable {MONEY})

    public Text nameTxt;  // eventName -> Text
    public Text spTxt;  // space -> Text
    public Text mTxt;  // money -> Text

    public GameObject tetromino;  // 버튼 클릭 시 생성 될 블록 프리펩
    private GameObject scheduleManager;  // 스케줄 매니저


    private void Awake()
    {
        scheduleManager = GameObject.Find("ScheduleManager");
    }

    public void InitBlock(int num)
    {   // 이벤트 번호를 받고 이니셜라이징
        // ScheduleManager.InitSchedule()
        // ScheduleManager.ResetTetro()
        eventNum = num;

        tetromino = scheduleManager.GetComponent<ScheduleManager>().tetroPrefabs[num];
        eventName = GameManager.Instance.chapterTable[num]["NAME"].ToString();
        space = 4;
        money = int.Parse(GameManager.Instance.chapterTable[num]["MONEY"].ToString());

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
        GameObject newTetro = Instantiate(tetromino, Input.mousePosition, Quaternion.identity);
        newTetro.GetComponent<TetroScript>().eventNum = eventNum;
        GameManager.Instance.money -= money;

        // 알바 블록 예외처리
        if (eventNum == 0)
        {
            newTetro.GetComponent<SpriteRenderer>().color = Color.black;
            GameManager.Instance.money += money*2;
        }

        scheduleManager.SendMessage("UpdateMoney");
        newTetro.GetComponent<TetroScript>().SwitchSize();
        scheduleManager.GetComponent<ScheduleManager>().nowTetro = newTetro;

        // 알바 블록이 아니라면 클릭 시 이벤트 목록에서 제거
        if (eventNum > 0)
            Destroy(gameObject);

        // 이벤트 리스트 닫기
        GameObject.Find("ListPanel").SetActive(false);
    }
}
