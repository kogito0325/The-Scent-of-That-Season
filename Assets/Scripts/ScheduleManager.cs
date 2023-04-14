using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ScheduleManager : MonoBehaviour
{
    public Image[] loveGazes;
    public Text[] loveAmount;
    public Text moneyTxt;

    public GameObject eventBox;

    public int loveSummer;
    public int loveFall;
    public int loveWinter;

    public int money;
    public int month;
    public int tasks;
    private void Awake()
    {
        loveSummer = GameManager.Instance.loveSummer;
        loveFall = GameManager.Instance.loveFall;
        loveWinter = GameManager.Instance.loveWinter;

        month = GameManager.Instance.month;

        tasks = 4;
    }

    private void Start()
    {
        InitSchedule();
    }

    private void UpdateLoveGazes()
    {
        loveGazes[0].fillAmount = (float)loveSummer / 100;
        loveGazes[1].fillAmount = (float)loveFall / 100;
        loveGazes[2].fillAmount = (float)loveWinter / 100;
    }

    private void UpdateLoves()
    {
        loveAmount[0].text = loveSummer.ToString() + "%";
        loveAmount[1].text = loveFall.ToString() + "%";
        loveAmount[2].text = loveWinter.ToString() + "%";
    }

    public void UpdateMoney()
    {
        money = GameManager.Instance.money;
        moneyTxt.text = money.ToString();
    }

    private void InitSchedule()
    {
        UpdateLoveGazes();
        UpdateLoves();
        UpdateMoney();

        var instBox = Instantiate(eventBox);
        instBox.GetComponent<TetroBtn>().InitBlock(0);
        instBox.transform.SetParent(GameObject.Find("Content").transform);

        if (month == 3)
        {
            for (int i = 1; i <= 3; i++)
            {
                instBox = Instantiate(eventBox);
                instBox.GetComponent<TetroBtn>().InitBlock(i);
                instBox.transform.SetParent(GameObject.Find("Content").transform);
            }
            GameObject.Find("ListPanel").SetActive(false);
        }
    }
}
