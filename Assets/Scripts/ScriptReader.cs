using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEditorInternal;

public class ScriptReader : MonoBehaviour
{
    public Image emptyImage; // 캐릭터 이미지 홀더
    public Sprite[] characterImages; // 캐릭터 이미지 리스트
    public Button[] choiceButtons; // 선택지 UI 리스트

    public Text ChatText; // 대사
    public Text CharacterName; // 캐릭터 이름

    public string writerText = ""; // 출력되고 있는 문자열
    public bool choosed = false; // 선택지 눌렀는지 체크하는 부울 변수

    List<Dictionary<string, object>> scriptTable; // 대사 테이블
    List<Dictionary<string, object>> chapterTable; // 챕터 테이블
    List<Dictionary<string, object>> nameTable; // 캐릭터 이름 테이블

    // Start is called before the first frame update

    private void Awake()
    {
        // 테이블 초기화
        scriptTable = GameManager.Instance.scriptTable;
        chapterTable = GameManager.Instance.chapterTable;
        nameTable = GameManager.Instance.nameTable;
    }
    void Start()
    {
        // 게임 매니저의 현재 인덱스 참조
        // 프로세스 진행
        StartCoroutine(Process(GameManager.Instance.contextIdx));
    }

    public IEnumerator TextPrint(string narrator, string narration, int chr)
    { // 실제로 캐릭터 대사를 문자열로 받아 출력하는 코루틴 함수

        float textSpeed = 0.02f; // 텍스트 출력 속도
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
            if (Input.GetMouseButtonDown(0)) // 대사가 다 나오고 마우스 입력 대기
                break;
            yield return null;
        }
    }
    public IEnumerator NormalChat(int startIdx, int endIdx)
    { // 일반적인 대사를 출력하는 코루틴 함수
        string gottenName = PlayerPrefs.GetString("name");
        string name;
        string script;

        for (int i = startIdx; i <= endIdx; i++)
        {
            int nameIdx = int.Parse(scriptTable[i]["CHARACTER"].ToString());
            name = nameTable[nameIdx]["NAME"].ToString();
            script = scriptTable[i]["CONTENT"].ToString();
            if (name.Equals("주인공"))
                name = gottenName;
            yield return StartCoroutine(TextPrint(name, script, nameIdx)); // 대사가 다 출력될 때까지 대기
        }
    }

    public IEnumerator Process(int contextIdx)
    { // 인덱스를 받아 CONTENT에 따라 대사를 처리하는 코루틴 함수
        int startIdx = int.Parse(scriptTable[contextIdx]["START_POINT"].ToString());
        int endIdx = int.Parse(scriptTable[contextIdx]["END_POINT"].ToString());
        string process = scriptTable[contextIdx]["CONTENT"].ToString();
        switch (process)
        {
            case "~대화":
                yield return StartCoroutine(NormalChatProcess(startIdx, endIdx)); // 일반 대화가 끝날 때까지 대기
                break;
            case "~선택지":
                yield return StartCoroutine(ChoiceProcess(startIdx, endIdx)); // 선택 및 선택 구문이 다 끝날 때까지 대기
                break;
            case "~챕터끝":
                yield return StartCoroutine(EndProcess(startIdx, endIdx)); // 챕터를 끝내는 처리 대기
                break;
        }
        contextIdx = GameManager.Instance.contextIdx;

        yield return StartCoroutine(Process(contextIdx)); // 다음 CONTENT 시작
    }

    IEnumerator NormalChatProcess(int startIdx, int endIdx)
    {
        yield return StartCoroutine(NormalChat(startIdx, endIdx)); // 일반 대화 처리 대기
        GameManager.Instance.UpdateIdx(++endIdx);
    }

    IEnumerator ChoiceProcess(int startIdx, int endIdx)
    {
        int tempEndIdx = int.Parse(scriptTable[endIdx]["END_POINT"].ToString());
        for (int i = startIdx; i <= endIdx; i++)
        {
            ActivateButtons(i - startIdx, i); // 선택지 활성화
        }
        
        yield return StartCoroutine(GetNumber()); // 선택지 고를 때까지 대기
        startIdx = int.Parse(scriptTable[GameManager.Instance.contextIdx]["START_POINT"].ToString());
        endIdx = int.Parse(scriptTable[GameManager.Instance.contextIdx]["END_POINT"].ToString());
        yield return StartCoroutine(NormalChat(startIdx, endIdx)); // 고른 선택지에 따른 대사 출력
        GameManager.Instance.UpdateIdx(++tempEndIdx);
    }

    IEnumerator EndProcess(int startIdx, int endIdx)
    {
        GameManager.Instance.UpdateIdx(++endIdx);
        GameManager.Instance.RestartGame();
        yield break;
    }

    void ActivateButtons(int idx, int choiceIdx)
    {
        choiceButtons[idx].gameObject.SetActive(true);
        choiceButtons[idx].GetComponent<BtnManager>().choiceIdx = choiceIdx;
    }

    IEnumerator GetNumber()
    {
        while (!choosed) // BtnManager.ChooseNum()
        {
            yield return null;
        }
        choosed = false;
        foreach (var button in choiceButtons)
            button.gameObject.SetActive(false); // 선택지 비활성화
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
