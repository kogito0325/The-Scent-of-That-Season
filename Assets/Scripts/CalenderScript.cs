using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalenderScript : MonoBehaviour
{
    public GameObject[] days;
    public Sprite[] monthImages;

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
        int dayEnd = 42;
        switch (month)
        {
            case 3:
                dayStart = 4;
                dayEnd = 34;
                break;
            case 4:
                dayStart = 7;
                dayEnd= 36;
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
    }
}
