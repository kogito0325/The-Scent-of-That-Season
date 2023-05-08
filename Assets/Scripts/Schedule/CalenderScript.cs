using UnityEngine;
using UnityEngine.UI;

public class CalenderScript : MonoBehaviour
{
    public GameObject[] days;
    public GameObject tetroParent;
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
            case 7:
                dayStart = 1;
                dayEnd = 30;
                break;
            case 8:
                dayStart = 3;
                dayEnd = 33;
                break;
            case 9:
                dayStart = 6;
                dayEnd = 35;
                break;
            case 10:
                dayStart = 1;
                dayEnd = 31;
                break;
            case 11:
                dayStart = 4;
                dayEnd = 33;
                break;
            case 12:
                dayStart = 6;
                dayEnd = 35;
                break;
            case 13:
                dayStart = 2;
                dayEnd = 32;
                break;
            case 14:
                dayStart = 5;
                dayEnd = 33;
                break;
        } 
        
        for (int i = dayStart - 1; i < dayEnd; i++)
            days[i].SetActive(true);

        monthImage.sprite = monthImages[month - 3];
        monthText.text = month.ToString() + "¿ù";
    }

    public void LocateTetromino()
    {
        GameObject tetromino = GameObject.Find("ScheduleManager").GetComponent<ScheduleManager>().nowTetro;
        TetroScript tetroScript = tetromino.GetComponent<TetroScript>();
        Vector3 tempPosition = new Vector3(0, 0, 0);
        int length = 0;
        float minX, minY, maxX, maxY;
        minX = minY = maxX = maxY = 0;
        foreach (GameObject tileVec in days)
        {
            if (tileVec.tag == "OnTiled")
            {
                if (minX == minY && maxX == maxY && maxX == maxY && maxY == 0)
                {
                    minX = maxX = tileVec.transform.position.x;
                    minY = maxY = tileVec.transform.position.y;
                }

                minX = tileVec.transform.position.x < minX ? tileVec.transform.position.x : minX;
                minY = tileVec.transform.position.y < minY ? tileVec.transform.position.y : minY;
                maxX = tileVec.transform.position.x > maxX ? tileVec.transform.position.x : maxX;
                maxY = tileVec.transform.position.y > maxY ? tileVec.transform.position.y : maxY;
                length++;
                tileVec.tag = "Tile";
                tileVec.GetComponent<Collider2D>().enabled = false;
            }
        }
        
        tempPosition = new Vector3((minX + maxX)/2, (minY + maxY)/2);
        tetromino.transform.position = tempPosition;
        tetroScript.position= tempPosition;
        tetroScript.SwitchSize();
        tetromino.transform.SetParent(tetroParent.transform);
        if (GameManager.Instance.task != "")
            GameManager.Instance.task += ",";
        GameManager.Instance.task += tetroScript.eventNum.ToString();
        
        tetroScript.enabled = false;

        GameObject.Find("ScheduleManager").GetComponent<ScheduleManager>().nowTetro = null;
    }

    public void LocateTetromino(float x, float y, float z, int num)
    {
        GameObject tetromino = Instantiate(GameObject.Find("ScheduleManager").GetComponent<ScheduleManager>().tetroPrefabs[num]);
        TetroScript tetroScript = tetromino.GetComponent<TetroScript>();
        Vector3 tempPosition = new Vector3(x, y, z);

        tetromino.transform.position = tempPosition;
        tetroScript.eventNum = num;
        tetroScript.position = tempPosition;
        tetromino.transform.SetParent(tetroParent.transform);
        tetroScript.enabled = false;

        GameObject.Find("ScheduleManager").GetComponent<ScheduleManager>().nowTetro = null;
    }
}
