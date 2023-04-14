using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;

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
    {   // ���� ���� ��ư - �̸� �Է� ȭ�� �ε�
        if (GameObject.Find("ScheduleManager").GetComponent<ScheduleManager>().tasks > 0)
            return;
        SceneManager.LoadScene("InputScene");
    }

    public void StartSchedule()
    {   // ������ ��ư - �޷� ȭ�� �ε�
        SceneManager.LoadScene("ScheduleScene");
    }

    public void EndGame()
    {   // ���� ������ ��ư - ���� ����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif
    }

    public void SaveName(InputField inputName)
    {   // �̸� �����ϴ� ��ư - ���� �� ��ȭ ȭ�� �ε�
        // Null �̰ų� ��� ������ �ȵ�
        // ���鹮�ڷθ� �̷�����־ �ȵ�
        if (string.IsNullOrEmpty(inputName.text) || string.IsNullOrWhiteSpace(inputName.text))
            PlayerPrefs.SetString("name", "�־�");
        else
            PlayerPrefs.SetString("name", inputName.text); // ���Ϳ� �̸� ����
        SceneManager.LoadScene("ChatScene"); // ��ȭâ���� �̵�
    }

    public void ChooseNum()
    {   // ������ ��ư - ScriptReader�� Ŭ���� ���� ����
        GameManager.Instance.UpdateIdx(choiceIdx);
        scriptReader.choosed = true;
    }

    public void SwitchEventList(GameObject list)
    {   // �̺�Ʈ ��� ��ư - ��� ���� �ݱ�
        if (!list.activeSelf)
            list.SetActive(true);
        else
            list.SetActive(false);
    }

    public void CancelUI(GameObject go)
    {   // ���ڷ� ���� UI�� ���� �Լ�
        go.SetActive(false);
    }
}