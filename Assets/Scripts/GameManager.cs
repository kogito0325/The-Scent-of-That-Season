using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //���ӸŴ����� �ν��Ͻ��� ��� ��������(static ���������� �����ϱ� ���� ����������� �ϰڴ�).
    //�� ���� ������ ���ӸŴ��� �ν��Ͻ��� �� instance�� ��� �༮�� �����ϰ� �� ���̴�.
    //������ ���� private����.
    private static GameManager instance = null;

    public List<Dictionary<string, object>> scriptTable;
    public List<Dictionary<string, object>> chapterTable;
    public List<Dictionary<string, object>> nameTable;

    public bool autoMode = false;
    public float coolTime = 2f;
    public int contextIdx = 0;
    public int month = 3;
    public int loveSummer = 0;
    public int loveFall = 0;
    public int loveWinter = 0;
    public int money = 0;

    void Awake()
    {
        if (null == instance)
        {
            //�� Ŭ���� �ν��Ͻ��� ź������ �� �������� instance�� ���ӸŴ��� �ν��Ͻ��� ������� �ʴٸ�, �ڽ��� �־��ش�.
            instance = this;

            //�� ��ȯ�� �Ǵ��� �ı����� �ʰ� �Ѵ�.
            //gameObject�����ε� �� ��ũ��Ʈ�� ������Ʈ�μ� �پ��ִ� Hierarchy���� ���ӿ�����Ʈ��� ��������, 
            //���� �򰥸� ������ ���� this�� �ٿ��ֱ⵵ �Ѵ�.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //���� �� �̵��� �Ǿ��µ� �� ������ Hierarchy�� GameMgr�� ������ ���� �ִ�.
            //�׷� ��쿣 ���� ������ ����ϴ� �ν��Ͻ��� ��� ������ִ� ��찡 ���� �� ����.
            //�׷��� �̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ�(���ο� ���� GameMgr)�� �������ش�.
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        InitGame();
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

    public void InitGame()
    {
        //contextIdx = PlayerPrefs.GetInt("contextIdx", 0); <- ����� �ؾߵż� �ϴ� �� ���� ������ �ʱ�ȭ ����
        //month = PlayerPrefs.GetInt("month", 3);
        /*loveSummer = PlayerPrefs.GetInt("loveSummer", 0);
        loveFall = PlayerPrefs.GetInt("loveFall", 0);
        loveWinter = PlayerPrefs.GetInt("loveWinter", 0);*/
        //money = PlayerPrefs.GetInt("money", 0);
        autoMode = true;
        coolTime = 0.5f;

        loveSummer = Random.Range(0, 100);
        loveFall= Random.Range(0, 100);
        loveWinter = Random.Range(0, 100);
        contextIdx = 1;
        month = Random.Range(3, 7);
        money = Random.Range(0, 10000000);

        scriptTable = CSVReader.Read("ScriptTable");
        chapterTable = CSVReader.Read("ChapterTable");
        nameTable = CSVReader.Read("CharacterNameTable");
    }

    public void PauseGame()
    {

    }

    public void ContinueGame()
    {

    }

    public void RestartGame()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void StopGame()
    {
        PlayerPrefs.SetInt("contextIdx", contextIdx);
        PlayerPrefs.SetInt("month", month);
        PlayerPrefs.SetInt("loveSummer", loveSummer);
        PlayerPrefs.SetInt("loveFall", loveFall);
        PlayerPrefs.SetInt("loveWinter", loveWinter);
        PlayerPrefs.SetInt("money", money);
    }
}