using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //게임매니저의 인스턴스를 담는 정적 변수
    //보안을 위해 private으로.
    private static GameManager instance = null;

    // 테이블 링킹
    public List<Dictionary<string, object>> scriptTable;
    public List<Dictionary<string, object>> chapterTable;
    public List<Dictionary<string, object>> nameTable;
    public List<Dictionary<string, object>> faceTable;

    // 인덱스 변수
    public int contextIdx = 1;  // 현재 컨텍스트

    // 정수형 변수
    public int month;  // 현재 달
    public int loveSummer;  // 지구하 호감도
    public int loveFall;  // 유가현 호감도
    public int loveWinter;  // 한서령 호감도
    public int money;  // 보유 자산

    // 실수형 변수
    public float coolTime;  // 자동 넘기기 간격 (단위: 초)
    [Header("텍스트 출력 속도 조절")][Range(0, 0.4f)]
    public float textSpeed;  // 텍스트 출력 속도
    public float masterVol;  // 마스터 볼륨
    public float bgmVol;  // 브금 볼륨
    public float fxVol;  // 효과음 볼륨

    // 논리 변수
    public bool autoMode;  // 자동 넘기기 on off
    public bool doingEvent;  // 일정 진행 중인지 구분하는 변수
    public bool paused;  // 대화 중 설정 창 열었을 때 클릭 비활성화
    public bool saveMode;  // 세이브 모드(true:저장 / false:불러오기)

    // 문자열 변수
    public string playerName;
    public string inactiveBtns;
    public string task;

    // 리스트형 변수
    public string[] pColors;

    // 오브젝트형 변수
    public GameObject calendar;
    public GameObject LoadPage;
    public GameObject SettingPage;

    // 오디오 변수
    public AudioSource bgmPlayer;
    public AudioSource fxPlayer;

    // 브금 리스트
    public AudioClip[] bgms;

    void Awake()
    {
        if (instance == null)
        {
            //인스턴스 생성 시 기존 인스턴스가 없는 경우 새로 대입.
            instance = this;

            //씬 전환이 되더라도 파괴되지 않게 한다.
            //this.gameObject = gameObject (헷갈림 방지)
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //전환한 씬에서 이미 사용 중인 매니저가 있을 경우, 새로 만든 인스턴스 삭제.
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        InitDefault();
    }

    //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 맘껏 호출할 수 있다.
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

        playerName = "주안";
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
        // 문자열 변수
        DataManager.Instance.nowData.playerName = playerName;
        DataManager.Instance.nowData.inactiveBtns = inactiveBtns;
        DataManager.Instance.nowData.task = task;

        // 인덱스 변수
        DataManager.Instance.nowData.contextIdx = contextIdx;

        // 정수형 변수
        DataManager.Instance.nowData.month = month;
        DataManager.Instance.nowData.loveSummer = loveSummer;
        DataManager.Instance.nowData.loveFall = loveFall;
        DataManager.Instance.nowData.loveWinter = loveWinter;
        DataManager.Instance.nowData.money = money;

        // 실수형 변수
        DataManager.Instance.nowData.coolTime = coolTime;
        DataManager.Instance.nowData.textSpeed = textSpeed;

        // 논리 변수
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
    {   // 캐릭터별 색상값 가져오는 함수 (외부에서 호출)
        Color color;
        if (UnityEngine.ColorUtility.TryParseHtmlString(pColors[hexIdx], out color))
        {   // 헥사코드 변환 시도
            return color;
        }

        // 변환 실패 시 흰색 반환
        Debug.LogError("다음 헥사코드 값을 사용할 수 없습니다: " + pColors[hexIdx]);
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
    {   // 아무렇게나 배치한 일정 순서를 재배치하는 함수
        // 일정을 하나만 넣었을 경우 그대로 반환
        if (!task.Contains(','))
            return;

        // 알바가 아닌 일정들
        int[] taskArr = task.Split(',').Select(int.Parse).Where(x => x > 0).ToArray();
        Array.Sort(taskArr);  // 정렬

        // 알바 갯수
        int zeros = task.Split(',').Where(s => s == "0").ToArray().Length;
        int idx = 0;

        // 재배치한 일정 순서 담을 곳
        string finalArr = "";

        // 알바나 이벤트 둘 중 하나 떨어질 때 까지 번갈아 배치
        for (; zeros > 0 && idx < taskArr.Length; zeros--)
        {
            finalArr += "," + taskArr[idx++].ToString();
            finalArr += ",0";
        }

        // 알바가 먼저 떨어지면 남은 일정 이어서 배치
        if (zeros == 0)
            while (idx < taskArr.Length)
                finalArr += "," + taskArr[idx++].ToString();

        // 일정이 먼저 떨어지면 남은 알바 한번으로 통일
        if (finalArr == "" && zeros > 0)
            finalArr = ",0";

        // 재배치된 일정 대입
        // 콤마로 시작하니까 두번째글자부터([1..]) 대입
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