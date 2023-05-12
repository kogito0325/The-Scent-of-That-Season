using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{

    public int choiceIdx; // ������ ��ư���� �ο��Ǵ� scriptIdx
    public string choiceContent; // ������ �̸�
    public ScriptReader scriptReader; // ��ũ��Ʈ ������ ��ȣ�ۿ� �ϱ� ���� ����
    public Text choiceTxt; // ������ �ؽ�Ʈ ��� ��

    public List<Dictionary<string, object>> scriptTable; // ��ũ��Ʈ ���̺�

    private void Awake()
    {
        // ���̺� �ʱ�ȭ
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
    {   // ���� ���� ��ư
        if (GameManager.Instance.task == "")
            return;
        if (!GameManager.Instance.doingEvent)
        {
            GameManager.Instance.doingEvent = true;
            GameManager.Instance.OrderingTask();
        }
        GameManager.Instance.calendar = GameObject.Find("ScheduleManager").GetComponent<ScheduleManager>().calendar;
        DontDestroyOnLoad(GameManager.Instance.calendar);
        GameManager.Instance.calendar.SetActive(false);

        int eventNum = GameManager.Instance.ReturnFirstTask();
        Debug.Log(eventNum);
        
        if (eventNum != 0)
        {
            GameManager.Instance.contextIdx = int.Parse(GameManager.Instance.chapterTable[eventNum]["START_POINT"].ToString());
            if (GameManager.Instance.contextIdx == 1)
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
    {   // ������ ��ư - �޷� ȭ�� �ε�
        GameManager.Instance.LoadScheduleScene();
    }

    public void StartTitle()
    {
        GameManager.Instance.RestartGame();
    }

    public void EndGame()
    {   // ���� ������ ��ư - ���� ����
        GameManager.Instance.StopGame();
    }

    public void ResetGame()
    {
        GameManager.Instance.ResetGame();
    }

    public void SaveName(InputField inputName)
    {   // �̸� �����ϴ� ��ư - ���� �� ��ȭ ȭ�� �ε�
        // Null �̰ų� ��� ������ �ȵ�
        // ���鹮�ڷθ� �̷�����־ �ȵ�
        if (string.IsNullOrEmpty(inputName.text) || string.IsNullOrWhiteSpace(inputName.text))
            GameManager.Instance.playerName = "�־�";
        else
            GameManager.Instance.playerName = inputName.text;  // ���Ϳ� �̸� ����
        GameManager.Instance.LoadChatScene(); // ��ȭâ���� �̵�
    }

    public void DecideSave()
    {
        var saveBtn = GameObject.Find("DataBox_" + GameManager.Instance.saveIdx.ToString("00"));
        saveBtn.GetComponent<SaveBtn>().SaveData(int.Parse(saveBtn.name[^2..]));
    }

    public void DecideLoad()
    {
        var saveBtn = GameObject.Find("DataBox_" + GameManager.Instance.saveIdx.ToString("00"));
        saveBtn.GetComponent<SaveBtn>().LoadData(int.Parse(saveBtn.name[^2..]));
    }


    public void ChooseNum()
    {   // ������ ��ư - ScriptReader�� Ŭ���� ���� ����
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
    {   // �̺�Ʈ ��� ��ư - ��� ���� �ݱ�
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
    {   // ���ڷ� ���� UI�� ���� �Լ�
        go.SetActive(false);
        GameManager.Instance.paused = false;
    }

    public void OpenGalleryPage()
    {
        if (GameManager.Instance.galleryPage.activeSelf)
            GameManager.Instance.ClosePage();
        else
            GameManager.Instance.OpenGalleryPage();
    }

    public void OpenSettingPage()
    {
        if (GameManager.Instance.settingPage.activeSelf)
            GameManager.Instance.ClosePage();
        else
            GameManager.Instance.OpenSettingPage();
    }

    public void OpenLoadPage()
    {
        if (GameManager.Instance.loadPage.activeSelf)
            GameManager.Instance.ClosePage();
        else
            GameManager.Instance.OpenLoadPage();
    }
    public void OpenTitleCheckPage()
    {
        GameManager.Instance.CheckUI("Title");
    }

    public void OpenStartCheckPage()
    {
        if (GameManager.Instance.doingEvent)
            StartGame();
        else
            GameManager.Instance.CheckUI("Start");
    }

    public void ResetTetroAll()
    {   // ��ġ�� ��� ��� ���� - SheduleManager.ResetTetroAll()
        GameObject.Find("ScheduleManager").GetComponent<ScheduleManager>().ResetTetroAll();
    }
}