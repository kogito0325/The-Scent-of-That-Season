using UnityEngine;
using System.IO;
using System;

public class Data
{
    // ���ڿ� ����
    public string playerName = "�־�";
    public string inactiveBtns = "0,1";
    public string task = "";

    // �ε��� ����
    public int contextIdx = 1;  // ���� ���ؽ�Ʈ

    // ������ ����
    public int month = 3;  // ���� ��
    public int loveSummer = 0;  // ������ ȣ����
    public int loveFall = 0;  // ������ ȣ����
    public int loveWinter = 0;  // �Ѽ��� ȣ����
    public int money = 0;  // ���� �ڻ�

    // �Ǽ��� ����
    public float coolTime = 1f;  // �ڵ� �ѱ�� ���� (����: ��)
    public float textSpeed = 0.05f;  // �ؽ�Ʈ ��� �ӵ�

    // �� ����
    public bool autoMode = false;  // �ڵ� �ѱ�� on off
    public bool eventTurn = false;  // �̺�Ʈ �������� �˹� �������� �����ϴ� ����
    public bool doingEvent = false;  // ���� ������ �����ϴ� ����

}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance = null;
    public Data nowData = new();
    string path;  // C:\Users\{�����}\AppData\LocalLow\DefaultCompany\Dongseo Pub
    string timestamp;
    readonly string fileName = "save_";
    readonly string picName = "shot_";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }

        path = Application.persistentDataPath + "/";
    }

    public string[] FindPath(int num, char mod='a')
    {
        string[] filePath = new string[0];
        if (mod == 'a')
            filePath = Directory.GetFiles(path, "*_" + num.ToString("00") + '*');
        else if (mod == 's')
            filePath = Directory.GetFiles(path, fileName + num.ToString("00") + '*');
        else if (mod == 'p')
            filePath = Directory.GetFiles(path, picName + num.ToString("00") + '*');

        if (filePath.Length == 0)
        {
            //Debug.Log(path + num.ToString("00") + " is not exist (mode: " + mod + ')');
            return null;
        }
        return filePath;
    }

    public void SaveData(int num=0)
    {
        string[] filePath = FindPath(num, 'a');
        if (filePath != null)
            DeleteData(num);

        string data = JsonUtility.ToJson(nowData);
        File.WriteAllText(path + fileName + num.ToString("00"), data);

        timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        ScreenCapture.CaptureScreenshot(path + picName + num.ToString("00") + '.' + timestamp + ".png");
    }

    public void DeleteData(int num = 0)
    {
        string[] filePath = FindPath(num, 'a');

        if (filePath == null)
            return;
        
        foreach (var p in filePath)
            File.Delete(p);
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        foreach (var p in Directory.GetFiles(path, "*"))
            File.Delete(p);
    }

    public void LoadData(int num=0)
    {
        string[] filePath = FindPath(num, 's');
        if (filePath == null)
            return;
        string data = File.ReadAllText(filePath[0]);

        nowData = JsonUtility.FromJson<Data>(data);
    }

    public void RefreshData()
    {
        FileInfo fileInfo = new FileInfo(path);
        fileInfo.Refresh();
    }

    public string LoadDate(int num=0)
    {
        string[] filePath = FindPath(num, 'p');
        if (filePath == null)
            return null;
        string fileName = Path.GetFileName(filePath[0]);
        string date = fileName.Split('.')[1];
        
        return date;
    }

    public Sprite LoadPic(int num=0)
    {
        string[] filePath = FindPath(num, 'p');
        if (filePath == null)
            return null;

        Sprite sprite;
        Texture2D img;
        Rect rect;

        byte[] buffer = File.ReadAllBytes(filePath[0]);
        img = new Texture2D(1, 1, TextureFormat.RGB24, false);
        img.LoadImage(buffer);
        rect = new Rect(0, 0, img.width, img.height);
        sprite = Sprite.Create(img, rect, Vector2.one * 0.5f);
        return sprite;
    }
}
