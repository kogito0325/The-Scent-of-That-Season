using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;

public class CalenderScript : MonoBehaviour
{
    public GameObject[] days;
    public Sprite[] monthImages;
    public Text monthText;

    public int month;
    public SpriteRenderer monthImage;

    private void Start()
    {
        InitCalender();
    }

    public void InitCalender()
    {
        foreach (GameObject day in days)
            day.SetActive(false);

        month = GameManager.Instance.month;
        int dayStart = 1;
        int dayEnd = 35;
        switch (month)
        {
            case 3:
                dayStart = 4;
                dayEnd = 34;
                break;
            case 4:
                dayStart = 6;
                dayEnd= 35;
                break;
            case 5:
                dayStart = 2;
                dayEnd = 32;
                break;
            case 6:
                dayStart = 5;
                dayEnd = 34;
                break;

        }
        
        for (int i = dayStart - 1; i < dayEnd; i++)
            days[i].SetActive(true);

        monthImage.sprite = monthImages[month - 3];
        monthText.text = month.ToString() + "¿ù";
    }

    public void LocateTetromino()
    {
        GameObject tetromino = GameObject.Find("TestTetromino(Clone)");
        Vector3 tempPosition = new Vector3(0, 0, 0);
        int length = 0;
        foreach (GameObject tileVec in days)
        {
            if (tileVec.tag == "OnTiled")
            {
                tempPosition += tileVec.transform.position;
                length++;
                tileVec.tag = "Tile";
                tileVec.GetComponent<Collider2D>().enabled = false;
            }
        }
        if (tetromino.GetComponent<TetroScript>().eventNum == 0)
            GameManager.Instance.albs++;
        else
            GameManager.Instance.eventArr.Add(tetromino.GetComponent<TetroScript>().eventNum);
        tempPosition /= length;
        tetromino.transform.position = tempPosition;
        tetromino.GetComponent<TetroScript>().SwitchSize();
        tetromino.name = "TestTetromino";
        tetromino.GetComponent<TetroScript>().enabled = false;

        GameObject.Find("ScheduleManager").GetComponent<ScheduleManager>().tasks--;
    }
}
