using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene1Script : MonoBehaviour
{
    public Image emptyImage; //기존이미지 : 유가현
    public Sprite changeSprit0;
    public Sprite changeSprit1; //바꿀 이미지 : 지구하
    public Sprite changeSprit2; // 주인공

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
        if (Ch2 == 0){ //유가현
            emptyImage.sprite= changeSprit0;
        }
        if (Ch2 == 1){ //지구하
            emptyImage.sprite = changeSprit1;
        }

        if (Ch2 == 2){//주인공 
            emptyImage.sprite = changeSprit2;
        }
        for (int i = 0; i < narration.Length; i++)
        {
            writerText += narration[i];
            ChatText.text = writerText;
            yield return new WaitForSeconds(.1f);
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
        string str1 = PlayerPrefs.GetString("Name1");
        string str2 = PlayerPrefs.GetString("Name2");
        yield return StartCoroutine(NormalChat(str1 + str2, "날씨가 쌀쌀해졌네..", 2));
        yield return StartCoroutine(NormalChat("유가현", "안녕, " + str1 + str2 + "! 오늘 날씨가 춥지?",0));
        yield return StartCoroutine(NormalChat("유가현", str2 + " 진짜 춥다. 그치?",0));
        yield return StartCoroutine(NormalChat("유가현", "(진짜 졸라 춥네)",0));
        yield return StartCoroutine(NormalChat("지구하", "이게 뭐가 추워!",1));
        yield return StartCoroutine(NormalChat("지구하", "하나도 안춥지," + str2 + "?",1));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
