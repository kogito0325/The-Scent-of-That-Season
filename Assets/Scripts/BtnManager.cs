using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class BtnManager : MonoBehaviour
{

    public int choiceIdx; // 선택지 버튼마다 부여되는 scriptIdx
    public string choiceContent; // 선택지 이름
    public ScriptReader scriptReader; // 스크립트 리더랑 상호작용 하기 위한 변수
    public Text choiceTxt; // 선택지 텍스트 담는 곳
    public GameObject tetromino; // 이벤트 클릭하면 나오는 블럭

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
        SceneManager.LoadScene("InputScene");
    }

    public void StartSchedule()
    {   // 스케줄 버튼 - 달력 화면 로드
        SceneManager.LoadScene("ScheduleScene");
    }

    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif
    }

    public void SaveName(InputField inputName)
    {   // 이름 저장하는 버튼 - 저장 후 대화 화면 로드
        // Null 이거나 비어 있으면 안됨
        // 공백문자로만 이루어져있어도 안됨
        if (string.IsNullOrEmpty(inputName.text) || string.IsNullOrWhiteSpace(inputName.text))
            Debug.Log("이름을 제대로 입력하여 주세요.");
        else
        {
            PlayerPrefs.SetString("name", inputName.text); // 컴터에 이름 저장
            SceneManager.LoadScene("ChatScene"); // 대화창으로 이동
        }
    }

    public void ChooseNum()
    {   // 선택지 버튼 - ScriptReader로 클릭한 변수 전달
        GameManager.Instance.UpdateIdx(choiceIdx);
        scriptReader.choosed = true;
    }

    public void SwitchEventList(GameObject list)
    {   // 이벤트 목록 버튼 - 목록 열고 닫기
        if (!list.activeSelf)
            list.SetActive(true);
        else
            list.SetActive(false);
    }

    public void GetTetromino()
    {   // 이벤트 클릭하면 블럭 생성
        tetromino = Instantiate(tetromino);
        tetromino.GetComponent<TetroScript>().SwitchSize();
        gameObject.SetActive(false); // 블럭이 되면 이벤트 목록에서 제거
    }
}