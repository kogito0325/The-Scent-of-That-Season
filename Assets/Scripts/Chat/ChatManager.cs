using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public GameObject settingMenu;
    public GameObject backLog;
    public GameObject backLogContent;
    public GameObject logPrefab;
    public GameObject allUI;

    // Start is called before the first frame update
    void Start()
    {
        settingMenu.SetActive(false);
        backLog.SetActive(false);
        allUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (settingMenu.activeSelf)
                settingMenu.SetActive(false);
            else
                settingMenu.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.Instance.autoMode = !GameManager.Instance.autoMode;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (backLog.activeSelf)
                backLog.SetActive(false);
            else
                backLog.SetActive(true);
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (allUI.activeSelf)
                allUI.SetActive(false);
            else
                allUI.SetActive(true);
        }

        if (CheckPause())
            GameManager.Instance.paused = true;
        else
            GameManager.Instance.paused = false;
    }

    public void AddLog(string name, string content)
    {
        GameObject instLog = Instantiate(logPrefab);
        instLog.GetComponent<LogScript>().logName.text = name;
        instLog.GetComponent<LogScript>().logContent.text = content;
        instLog.transform.SetParent(backLogContent.transform);
    }

    private bool CheckPause()
    {
        return settingMenu.activeSelf || backLog.activeSelf || !allUI.activeSelf;
    }
}
