using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ScheduleManager : MonoBehaviour
{
    public Image[] loveGazes;
    public Text[] loveAmount;
    public Text moneyTxt;

    public GameObject[] tetroPrefabs;
    public GameObject eventBox;
    public GameObject nowTetro;
    public GameObject listPanel;
    public GameObject calendar;

    public int loveSummer;
    public int loveFall;
    public int loveWinter;

    public int money;
    public int month;

    private void Awake()
    {
        loveSummer = GameManager.Instance.loveSummer;
        loveFall = GameManager.Instance.loveFall;
        loveWinter = GameManager.Instance.loveWinter;

        month = GameManager.Instance.month;
        GameManager.Instance.saveMode = true;
    }

    private void Start()
    {
        if (GameManager.Instance.task == "")
        {
            GameManager.Instance.doingEvent = false;
        }

        if (!GameManager.Instance.doingEvent)
        {
            InitSchedule();
            Destroy(GameManager.Instance.calendar);
        }
        else
        {
            Destroy(calendar);
            calendar = GameManager.Instance.calendar;
            calendar.SetActive(true);
            UpdateLoveGazes();
            UpdateLoves();
            UpdateMoney();
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GameManager.Instance.OpenSettingPage();
        }
    }

    private void UpdateLoveGazes()
    {   // ȣ���� ������ ����
        loveGazes[0].fillAmount = (float)loveSummer / 100;
        loveGazes[1].fillAmount = (float)loveFall / 100;
        loveGazes[2].fillAmount = (float)loveWinter / 100;
    }

    private void UpdateLoves()
    {   // ȣ���� ��ġ ����
        loveAmount[0].text = loveSummer.ToString() + "%";
        loveAmount[1].text = loveFall.ToString() + "%";
        loveAmount[2].text = loveWinter.ToString() + "%";
    }

    public void UpdateMoney()
    {   // ���� �ڻ� ����
        money = GameManager.Instance.money;
        moneyTxt.text = money.ToString();
    }

    private void InitSchedule()
    {
        // ������ ����
        UpdateLoveGazes();
        UpdateLoves();
        UpdateMoney();

        // �˹� �̺�Ʈ ����
        var instBox = Instantiate(eventBox);
        instBox.GetComponent<TetroBtn>().InitBlock(0);
        instBox.transform.SetParent(GameObject.Find("Content").transform);

        // �ó����� �̺�Ʈ ����
        for (int i = 1; int.Parse(GameManager.Instance.chapterTable[i]["MONTH"].ToString()) == GameManager.Instance.month; i++)
        {
            instBox = Instantiate(eventBox);
            instBox.GetComponent<TetroBtn>().InitBlock(i);
            instBox.transform.SetParent(GameObject.Find("Content").transform);
        }

        // �̺�Ʈ ����Ʈ �ݱ�
        listPanel.SetActive(false);
    }

    public void ResetTetro()
    {   // ���� ���� ���� ��� ����
        if (nowTetro != null)
        {
            if (nowTetro.GetComponent<TetroScript>().eventNum != 0)
            {
                var instBox = Instantiate(eventBox);
                instBox.GetComponent<TetroBtn>().InitBlock(nowTetro.GetComponent<TetroScript>().eventNum);
                instBox.transform.SetParent(GameObject.Find("Content").transform);
            }
            Destroy(nowTetro);
            nowTetro = null;
        }
    }

    public void ResetTetroAll()
    {   // ��ġ�� ��� ��� ����
        if (!GameManager.Instance.doingEvent)
        {
            GameManager.Instance.task = "";
            GameManager.Instance.LoadScheduleScene();
        }
    }
}
