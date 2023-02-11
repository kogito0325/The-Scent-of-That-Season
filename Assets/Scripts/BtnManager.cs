using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class BtnManager : MonoBehaviour
{

    public int choiceIdx; // 선택지 버튼마다 부여되는 scriptIdx
    public ScriptReader scriptReader; // 스크립트 리더랑 상호작용 하기 위한 변수

    public List<Dictionary<string, object>> scriptTable;

    private void Awake()
    {
        // 테이블 초기화
        scriptTable = GameManager.Instance.scriptTable;
    }
    public void StartGame()
    {
        SceneManager.LoadScene("InputScene");
    }


    public void SaveName(InputField inputName)
    {
        if (string.IsNullOrEmpty(inputName.text) || string.IsNullOrWhiteSpace(inputName.text))
            Debug.Log("이름을 제대로 입력하여 주세요.");
        else
        {
            PlayerPrefs.SetString("name", inputName.text);
            SceneManager.LoadScene("ChatScene");
        }
    }

    public void chooseNum()
    {
        GameManager.Instance.UpdateIdx(choiceIdx);
        scriptReader.choosed = true;
    }
}