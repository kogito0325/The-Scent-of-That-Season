using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{

    public int choiceIdx; // 선택지 버튼마다 부여되는 scriptIdx
    public string choiceContent; // 선택지 이름
    public ScriptReader scriptReader; // 스크립트 리더랑 상호작용 하기 위한 변수
    public Text choiceTxt; // 선택지 텍스트 담는 곳

    public List<Dictionary<string, object>> scriptTable; // 스크립트 테이블

    private void Awake()
    {
        // 테이블 초기화
        try
        {
            scriptTable = GameManager.Instance.scriptTable;
        }
        catch (NullReferenceException)
        {
            scriptTable = null;
        }
    }
    public void StartGame()
    {   // 게임 시작 버튼 - 이름 입력 화면 로드
        if (GameManager.Instance.task == "")
            return;
        GameManager.Instance.calendar = GameObject.Find("ScheduleManager").GetComponent<ScheduleManager>().calendar;
        DontDestroyOnLoad(GameManager.Instance.calendar);
        GameManager.Instance.calendar.SetActive(false);
        GameManager.Instance.doingEvent = true;
        GameManager.Instance.OrderingTask();
        int eventNum = int.Parse(GameManager.Instance.task.Split(',')[0]);
        GameManager.Instance.contextIdx = 
            int.Parse(GameManager.Instance.chapterTable[eventNum]["START_POINT"].ToString());
        if (eventNum > 0)
        {
            if (eventNum == 1)
                GameManager.Instance.LoadInputScene();
            else
                GameManager.Instance.LoadChatScene();
        }
        else
        {
            GameManager.Instance.LoadContentScene();
        }
    }

    public void StartSchedule()
    {   // 스케줄 버튼 - 달력 화면 로드
        GameManager.Instance.LoadScheduleScene();
    }

    public void EndGame()
    {   // 게임 끝내는 버튼 - 게임 종료
        GameManager.Instance.StopGame();
    }

    public void ResetGame()
    {
        GameManager.Instance.ResetGame();
    }

    public void SaveName(InputField inputName)
    {   // 이름 저장하는 버튼 - 저장 후 대화 화면 로드
        // Null 이거나 비어 있으면 안됨
        // 공백문자로만 이루어져있어도 안됨
        if (string.IsNullOrEmpty(inputName.text) || string.IsNullOrWhiteSpace(inputName.text))
            GameManager.Instance.playerName = "주안";
        else
            GameManager.Instance.playerName = inputName.text;  // 컴터에 이름 저장
        GameManager.Instance.LoadLoadingScene(); // 대화창으로 이동
    }

    public void ChooseNum()
    {   // 선택지 버튼 - ScriptReader로 클릭한 변수 전달
        GameManager.Instance.UpdateIdx(choiceIdx);
        GameManager.Instance.inactiveBtns += "," + choiceIdx.ToString();
        if (32 <= choiceIdx && choiceIdx <= 34)
        {
            GameManager.Instance.inactiveBtns += "," + (choiceIdx + 474).ToString();
            GameManager.Instance.inactiveBtns += "," + (choiceIdx + 474 + 474).ToString();
        }
        else if (32 + 474 <= choiceIdx && choiceIdx <= 34 + 474)
        {
            GameManager.Instance.inactiveBtns += "," + (choiceIdx + 474).ToString();
        }
        
        scriptReader.choosed = true;
    }

    public void SwitchEventList(GameObject list)
    {   // 이벤트 목록 버튼 - 목록 열고 닫기
        if (!list.activeSelf)
        {
            list.SetActive(true);
            GameObject.Find("ScheduleManager").GetComponent<ScheduleManager>().ResetTetro();
        }
        else
            list.SetActive(false);
    }

    public void SwitchSaveMode()
    {
        GameManager.Instance.saveMode = !GameManager.Instance.saveMode;
    }

    public void CancelUI(GameObject go)
    {   // 인자로 받은 UI를 끄는 함수
        go.SetActive(false);
        GameManager.Instance.paused = false;
    }

    public void OpenPage(GameObject page)
    {
        if (page.activeSelf)
        {
            page.SetActive(false);
        }
        else
        {
            page.SetActive(true);
        }
    }

    public void ResetTetroAll()
    {   // 배치된 모든 블록 리셋 - SheduleManager.ResetTetroAll()
        GameObject.Find("ScheduleManager").GetComponent<ScheduleManager>().ResetTetroAll();
    }
}