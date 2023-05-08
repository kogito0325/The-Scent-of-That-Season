using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ContentManager : MonoBehaviour
{
    public GameObject[] mobPrefabs;
    public ContentInventory player;
    public Text lifeTxt;
    public Text moneyTxt;
    public bool[] checkList = new bool[8];

    // Start is called before the first frame update
    void Start()
    {
        System.Array.Clear(checkList, 0, checkList.Length);
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        lifeTxt.text = "¿µ¾÷ Á¾·á: " + player.life;
        moneyTxt.text = "µ·: " + GameManager.Instance.money.ToString();
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            int randIdx = Random.Range(0, mobPrefabs.Length);
            Instantiate(mobPrefabs[randIdx], transform.position, Quaternion.identity);
            yield return new WaitForSeconds(5);
        }
    }
}
