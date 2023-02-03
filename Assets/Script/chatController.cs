using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chatController : MonoBehaviour
{
    public Text ChatText;
    public Text CharacterName;

    public string writerText = "";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TextPractice());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator NormalChat(string narrator, string narration)
    {
        int a = 0;
        CharacterName.text = narrator;
        writerText = "";

        for(a = 0;a < narration.Length; a++)
        {
            writerText += narration[a];
            ChatText.text = writerText;
            yield return new WaitForSeconds(.1f);
        }

        while (true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                break;
            }
            yield return null;
        }
    }

    IEnumerator TextPractice()
    {
        string str = PlayerPrefs.GetString("Name");
        yield return StartCoroutine(NormalChat("À¯°¡Çö", "¾È³ç, " + str + "¿À´Ã ³¯¾¾°¡ ÃäÁö?"));
        yield return StartCoroutine(NormalChat("À¯°¡Çö", "Ãä´Ù"));
        yield return StartCoroutine(NormalChat("À¯°¡Çö", "ÁøÂ¥ Á¹¶ó Ãä³×"));
    }
}
