using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScriptReader : MonoBehaviour
{
    public Image emptyImage; //기존이미지 : 유가현
    public Sprite[] characterImages;
    public Button[] choiceButtons;

    public Text ChatText; // 대사
    public Text CharacterName; //캐릭터 이름

    public string writerText = "";

    List<Dictionary<string, object>> scriptTable;
    List<Dictionary<string, object>> chapterTable;
    List<Dictionary<string, object>> choiceTable;
    List<Dictionary<string, object>> nameTable;
    List<Dictionary<string, object>> processTable; 

    // Start is called before the first frame update

    private void Awake()
    {
        scriptTable = CSVReader.Read("ScriptTable");
        chapterTable = CSVReader.Read("ChapterTable");
        choiceTable = CSVReader.Read("ChoiceTable");
        nameTable = CSVReader.Read("CharacterNameTable");
        processTable = CSVReader.Read("ProcessTable");
    }
    void Start()
    {
        StartCoroutine(NormalChat(1));
    }

    public IEnumerator TextPrint(string narrator, string narration, int chr)
    {
        float textSpeed = 0.02f;
        CharacterName.text = narrator;
        writerText = "";
        emptyImage.sprite = characterImages[chr];

        for (int i = 0; i < narration.Length; i++)
        {
            writerText += narration[i];
            ChatText.text = writerText;
            yield return new WaitForSeconds(textSpeed);
        }

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
                break;
            yield return null;
        }
    }
    IEnumerator NormalChat(int chapterIdx)
    {
        string gottenName = PlayerPrefs.GetString("name");
        string name;
        string script;
        int startIdx = int.Parse(chapterTable[chapterIdx]["START_POINT"].ToString());
        int endIdx = int.Parse(chapterTable[chapterIdx]["END_POINT"].ToString());

        for (int i = startIdx; i <= endIdx; i++)
        {
            int nameIdx = int.Parse(scriptTable[i]["CHARACTER"].ToString());
            name = nameTable[nameIdx]["NAME"].ToString();
            script = scriptTable[i]["SCRIPT"].ToString();
            if (name.Equals("주인공"))
                name = gottenName;
            yield return StartCoroutine(TextPrint(name, script, nameIdx));
            Debug.Log(script);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
