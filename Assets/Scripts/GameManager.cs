using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //���ӸŴ����� �ν��Ͻ��� ��� ���� ����
    //������ ���� private����.
    private static GameManager instance = null;

    // ���̺� ��ŷ
    public List<Dictionary<string, object>> scriptTable;
    public List<Dictionary<string, object>> chapterTable;
    public List<Dictionary<string, object>> nameTable;
    public List<Dictionary<string, object>> faceTable;

    // �ε��� ����
    public int contextIdx = 1;  // ���� ���ؽ�Ʈ

    // ������ ����
    public int month;  // ���� ��
    public int loveSummer;  // ������ ȣ����
    public int loveFall;  // ������ ȣ����
    public int loveWinter;  // �Ѽ��� ȣ����
    public int money;  // ���� �ڻ�

    // �Ǽ��� ����
    public float coolTime;  // �ڵ� �ѱ�� ���� (����: ��)
    [Header("�ؽ�Ʈ ��� �ӵ� ����")][Range(0, 0.4f)]
    public float textSpeed;  // �ؽ�Ʈ ��� �ӵ�
    public float masterVol;  // ������ ����
    public float bgmVol;  // ��� ����
    public float fxVol;  // ȿ���� ����

    // �� ����
    public bool autoMode;  // �ڵ� �ѱ�� on off
    public bool doingEvent;  // ���� ���� ������ �����ϴ� ����
    public bool paused;  // ��ȭ �� ���� â ������ �� Ŭ�� ��Ȱ��ȭ
    public bool saveMode;  // ���̺� ���(true:���� / false:�ҷ�����)

    // ���ڿ� ����
    public string playerName;
    public string inactiveBtns;
    public string task;

    // ����Ʈ�� ����
    public string[] pColors;

    // ������Ʈ�� ����
    public GameObject calendar;
    public GameObject LoadPage;
    public GameObject SettingPage;

    // ����� ����
    public AudioSource bgmPlayer;
    public AudioSource fxPlayer;

    // ��� ����Ʈ
    public AudioClip[] bgms;

    void Awake()
    {
        if (instance == null)
        {
            //�ν��Ͻ� ���� �� ���� �ν��Ͻ��� ���� ��� ���� ����.
            instance = this;

            //�� ��ȯ�� �Ǵ��� �ı����� �ʰ� �Ѵ�.
            //this.gameObject = gameObject (�򰥸� ����)
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //��ȯ�� ������ �̹� ��� ���� �Ŵ����� ���� ���, ���� ���� �ν��Ͻ� ����.
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        InitDefault();
    }

    //���� �Ŵ��� �ν��Ͻ��� ������ �� �ִ� ������Ƽ. static�̹Ƿ� �ٸ� Ŭ�������� ���� ȣ���� �� �ִ�.
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void UpdateIdx(int idx)
    {
        contextIdx = idx;
        Debug.Log("idx updated -> " + contextIdx.ToString());
    }

    public void InitDefault()
    {
        paused = false;
        pColors = new string[] { "#A6A6A6FF", "#FFFFFFFF", "#85D1FFFF", "#E1ECB9FF", "#F76E5EFF" };

        scriptTable = CSVReader.Read("ScriptTable");
        chapterTable = CSVReader.Read("ChapterTable");
        nameTable = CSVReader.Read("CharacterNameTable");
        faceTable = CSVReader.Read("FaceTable");

        playerName = "�־�";
        inactiveBtns = "0,1";
        task = "";

        contextIdx = 1;

        month = 3;
        loveSummer = 0;
        loveFall = 0;
        loveWinter = 0;
        money = 0;

        coolTime = 1f;
        textSpeed = 0.05f;

        masterVol = PlayerPrefs.GetFloat("mVol", 1f);
        bgmVol = PlayerPrefs.GetFloat("bVol", .4f);
        fxVol = PlayerPrefs.GetFloat("fVol", .4f);

        autoMode = false;
        doingEvent = false;
    }

    public void InitGame(int num=0)
    {
        if (DataManager.Instance.FindPath(num) == null)
            return;
        DataManager.Instance.LoadData(num);

        playerName = DataManager.Instance.nowData.playerName;
        inactiveBtns = DataManager.Instance.nowData.inactiveBtns;
        task = DataManager.Instance.nowData.task;

        loveSummer = DataManager.Instance.nowData.loveSummer;
        loveFall = DataManager.Instance.nowData.loveFall;
        loveWinter = DataManager.Instance.nowData.loveWinter;
        contextIdx = DataManager.Instance.nowData.contextIdx;
        month = DataManager.Instance.nowData.month;
        money = DataManager.Instance.nowData.money;

        doingEvent = DataManager.Instance.nowData.doingEvent;

        coolTime = DataManager.Instance.nowData.coolTime;
        textSpeed = DataManager.Instance.nowData.textSpeed;
        autoMode = DataManager.Instance.nowData.autoMode;

    }

    public void PauseGame()
    {
        paused = !paused;
    }

    public void RestartGame()
    {
        bgmPlayer.clip = bgms[0];
        bgmPlayer.Play();
        SceneManager.LoadScene("TitleScene");
        saveMode = false;
    }

    public void ResetGame()
    {
        DataManager.Instance.ResetData();
        RestartGame();
    }

    public void SaveGame(int num=0)
    {
        // ���ڿ� ����
        DataManager.Instance.nowData.playerName = playerName;
        DataManager.Instance.nowData.inactiveBtns = inactiveBtns;
        DataManager.Instance.nowData.task = task;

        // �ε��� ����
        DataManager.Instance.nowData.contextIdx = contextIdx;

        // ������ ����
        DataManager.Instance.nowData.month = month;
        DataManager.Instance.nowData.loveSummer = loveSummer;
        DataManager.Instance.nowData.loveFall = loveFall;
        DataManager.Instance.nowData.loveWinter = loveWinter;
        DataManager.Instance.nowData.money = money;

        // �Ǽ��� ����
        DataManager.Instance.nowData.coolTime = coolTime;
        DataManager.Instance.nowData.textSpeed = textSpeed;

        // �� ����
        DataManager.Instance.nowData.autoMode = autoMode;
        DataManager.Instance.nowData.doingEvent = doingEvent;

        DataManager.Instance.SaveData(num);
    }

    public void StopGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif
    }

    public AsyncOperation LoadChatScene()
    {
        bgmPlayer.clip = bgms[1];
        bgmPlayer.Play();
        return SceneManager.LoadSceneAsync("ChatScene");
    }

    public void LoadLoadingScene()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    public void LoadScheduleScene()
    {
        bgmPlayer.clip = bgms[0];
        bgmPlayer.Play();
        SceneManager.LoadScene("ScheduleScene");
    }

    public void LoadInputScene()
    {
        SceneManager.LoadScene("InputScene");
    }

    public void LoadContentScene()
    {
        bgmPlayer.clip = bgms[1];
        bgmPlayer.Play();
        SceneManager.LoadScene("ContentScene");
    }

    public void OpenLoadPage()
    {
        LoadPage.SetActive(true);
    }

    public void OpenSettingPage()
    {
        SettingPage.SetActive(true);
    }

    public void ClosePage()
    {
        SettingPage.SetActive(false);
        LoadPage.SetActive(false);
    }

    public void ClosePage(GameObject page)
    {
        page.SetActive(false);
    }

    public Color HexColor(int hexIdx)
    {   // ĳ���ͺ� ���� �������� �Լ� (�ܺο��� ȣ��)
        Color color;
        if (UnityEngine.ColorUtility.TryParseHtmlString(pColors[hexIdx], out color))
        {   // ����ڵ� ��ȯ �õ�
            return color;
        }

        // ��ȯ ���� �� ��� ��ȯ
        Debug.LogError("���� ����ڵ� ���� ����� �� �����ϴ�: " + pColors[hexIdx]);
        return Color.white;
    }

    public void CheckNextMonth()
    {
        if (task == "")
            month++;
    }

    public void PopTask()
    {
        string[] temp = task.Split(',');
        string tempTask = "";
        foreach (string s in temp[1..])
        {
            tempTask += ',' + s;
        }
        tempTask = tempTask[1..];
        task = tempTask;
    }

    public void OrderingTask()
    {   // �ƹ����Գ� ��ġ�� ���� ������ ���ġ�ϴ� �Լ�
        // ������ �ϳ��� �־��� ��� �״�� ��ȯ
        if (!task.Contains(','))
            return;

        // �˹ٰ� �ƴ� ������
        int[] taskArr = task.Split(',').Select(int.Parse).Where(x => x > 0).ToArray();
        Array.Sort(taskArr);  // ����

        // �˹� ����
        int zeros = task.Split(',').Where(s => s == "0").ToArray().Length;
        int idx = 0;

        // ���ġ�� ���� ���� ���� ��
        string finalArr = "";

        // �˹ٳ� �̺�Ʈ �� �� �ϳ� ������ �� ���� ������ ��ġ
        for (; zeros > 0 && idx < taskArr.Length; zeros--)
        {
            finalArr += "," + taskArr[idx++].ToString();
            finalArr += ",0";
        }

        // �˹ٰ� ���� �������� ���� ���� �̾ ��ġ
        if (zeros == 0)
            while (idx < taskArr.Length)
                finalArr += "," + taskArr[idx++].ToString();

        // ������ ���� �������� ���� �˹� �ѹ����� ����
        if (finalArr == "" && zeros > 0)
            finalArr = ",0";

        // ���ġ�� ���� ����
        // �޸��� �����ϴϱ� �ι�°���ں���([1..]) ����
        task = finalArr[1..];
    }

    public System.Collections.IEnumerator SoundFadeIn()
    {
        float _duration = coolTime;
        float originVol = bgmPlayer.volume;
        float timer = 0f;
        bgmPlayer.volume = 0;
        while (timer < _duration)
        {
            timer += Time.deltaTime;
            bgmPlayer.volume = timer / _duration * originVol;
            yield return null;
        }
        bgmPlayer.volume = originVol;
    }

    public void ChangeVol()
    {
        bgmPlayer.volume = bgmVol * masterVol;
        fxPlayer.volume = fxVol * masterVol;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("mVol", masterVol);
        PlayerPrefs.SetFloat("bVol", bgmVol);
        PlayerPrefs.SetFloat("fVol", fxVol);
    }
}