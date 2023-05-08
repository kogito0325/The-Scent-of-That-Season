using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public GameObject backLog;
    public GameObject backLogContent;
    public GameObject logPrefab;
    public GameObject allUI;

    public Sprite[] autoImages;
    public Image autoImage;


    // Start is called before the first frame update
    void Start()
    {
        backLog.SetActive(false);
        allUI.SetActive(true);
        autoImage.sprite = autoImages[GameManager.Instance.autoMode ? 0 : 1];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (GameManager.Instance.SettingPage.activeSelf)
                GameManager.Instance.ClosePage();
            else
                GameManager.Instance.OpenSettingPage();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.Instance.autoMode = !GameManager.Instance.autoMode;
            autoImage.sprite = autoImages[GameManager.Instance.autoMode ? 0 : 1];
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
        return GameManager.Instance.SettingPage.activeSelf || backLog.activeSelf || !allUI.activeSelf;
    }
}
