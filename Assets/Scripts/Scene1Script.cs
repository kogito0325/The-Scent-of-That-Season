using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene1Script : MonoBehaviour
{
    public Image emptyImage; //기존이미지 : 유가현
    public Sprite[] characterImages;

    public Text ChatText; // 대사
    public Text CharacterName; //캐릭터 이름

    public string writerText = "";
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TextPractice());
    }

    public IEnumerator NormalChat(string narrator, string narration, int Ch2)
    { 
        CharacterName.text = narrator;
        writerText = "";
        emptyImage.sprite = characterImages[Ch2];

        for (int i = 0; i < narration.Length; i++)
        {
            writerText += narration[i];
            ChatText.text = writerText;
            yield return new WaitForSeconds(.02f);
        }

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
            yield return null;
        }
    }
    IEnumerator TextPractice()
    {
        List<Dictionary<string, object>> data_Dialog = CSVReader.Read("TestTable");
        List<Dictionary<string, object>> name_Dialog = CSVReader.Read("CharacterNameTable");
        string str1 = PlayerPrefs.GetString("Name1");
        string str2 = PlayerPrefs.GetString("Name2");
        string name = str1 + str2;
        
        for (int i = 1; i < data_Dialog.Count; i++)
        {
            int nameIdx = int.Parse(data_Dialog[i]["CHARACTER"].ToString());
            name = name_Dialog[nameIdx]["NAME"].ToString();
            if (name == "주인공")
                name = str1 + str2;
            yield return StartCoroutine(NormalChat(name, data_Dialog[i]["SCRIPT"].ToString(), int.Parse(data_Dialog[i]["CHARACTER"].ToString())));
            Debug.Log(data_Dialog[i]["SCRIPT"].ToString());
        }
;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
