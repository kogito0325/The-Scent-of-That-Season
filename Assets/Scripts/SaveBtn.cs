using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SaveBtn : MonoBehaviour
{
    public Text sub;
    public Text date;
    public Image screenShot;

    public int saveIdx;


    private void Awake()
    {
        saveIdx = int.Parse(name[^2..^0]);
    }

    void Start()
    {
        if (DataManager.Instance.FindPath(saveIdx) != null)
            StartCoroutine(InitSaveBtn());
    }

    public void PushBtn()
    {
        if (GameManager.Instance.saveMode)
            SaveData(saveIdx);
        else
            LoadData(saveIdx);
    }

    public void SaveData(int num)
    {
        GameManager.Instance.SaveGame(num);
        StartCoroutine(InitSaveBtn());
    }

    public void LoadData(int num)
    {
        GameManager.Instance.InitGame(num);
    }

    IEnumerator InitSaveBtn()
    {
        yield return new WaitForSeconds(.2f);
        sub.text = saveIdx.ToString();
        date.text = DataManager.Instance.LoadDate(saveIdx);
        screenShot.sprite = DataManager.Instance.LoadPic(saveIdx);
    }
}
