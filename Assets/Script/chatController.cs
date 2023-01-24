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
        yield return StartCoroutine(NormalChat("유가현", "안녕"));
        yield return StartCoroutine(NormalChat("유가현", "우헤헤"));
        yield return StartCoroutine(NormalChat("유가현", "춥다"));
        yield return StartCoroutine(NormalChat("유가현", "진짜 졸라 춥네"));
    }
}
