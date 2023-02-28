using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptReader : MonoBehaviour
{
    public Image emptyImage; // 캐릭터 이미지 홀더
    public Image emptyBackgroundImage; // 배경 이미지 홀덩
    public Image fadeImage; // 페이드인, 아웃 구현 위한 이미지(필요없음)

    public Sprite[] characterImages; // 캐릭터 이미지 리스트
    public Button[] choiceButtons; // 선택지 UI 리스트
    public Sprite[] backgroundImages; // 배경 이미지 리스트

    public Text ChatText; // 대사
    public Text CharacterName; // 캐릭터 이름

    public string writerText = ""; // 출력되고 있는 문자열
    public bool choosed = false; // 선택지 눌렀는지 체크하는 부울 변수

    List<Dictionary<string, object>> scriptTable; // 대사 테이블
    List<Dictionary<string, object>> chapterTable; // 챕터 테이블
    List<Dictionary<string, object>> nameTable; // 캐릭터 이름 테이블


    private void Awake()
    {
        // 테이블 초기화
        scriptTable = GameManager.Instance.scriptTable;
        chapterTable = GameManager.Instance.chapterTable;
        nameTable = GameManager.Instance.nameTable;
    }
    void Start()
    {
        // 게임 매니저의 현재 인덱스(contextIdx) 참조
        // 프로세스 진행
        StartCoroutine(Process(GameManager.Instance.contextIdx));
    }

    public IEnumerator TextPrint(string narrator, string narration, int imageIdx)
    {   // 실제로 캐릭터 대사를 문자열로 받아 출력하는 코루틴 함수
        // NormalChat 에서 돌아감

        float textSpeed = 0.02f; // 텍스트 출력 속도
        CharacterName.text = narrator;
        writerText = "";
        emptyImage.sprite = characterImages[imageIdx];

        for (int i = 0; i < narration.Length; i++)
        {   // 출력 속도에 따라 한글자씩 화면에 출력
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
    {   // 일반적인 대사를 출력하는 코루틴 함수
        string gottenName = PlayerPrefs.GetString("name"); // 컴에 저장된 이름 불러오기
        string name; // 캐릭터 이름
        string content; // 캐릭터가 말하는 대사
        string action; // 연출

        for (int i = startIdx; i <= endIdx; i++)
        {
            int nameIdx = int.Parse(scriptTable[i]["CHARACTER"].ToString()); // 대사치는 캐릭터 이름 인덱스 불러오기
            int imageIdx = int.Parse(scriptTable[i]["IMAGE"].ToString()); // 대사치는 캐릭터 사진 인덱스 불러오기
            name = nameTable[nameIdx]["NAME"].ToString(); // 네임 테이블에서 인덱싱
            action = scriptTable[i]["ACTION"].ToString();
            content = scriptTable[i]["CONTENT"].ToString();

            // 테이블에서 "주인공" 이면 저장된 이름으로 변경
            if (name.Equals("주인공"))
                name = gottenName;

            if (action != null)
                yield return StartCoroutine(ActionProcess(action, i)); // 연출이 있으면 연출 실행
            yield return StartCoroutine(TextPrint(name, content, imageIdx)); // 대사가 다 출력될 때까지 대기
        }
    }

    public IEnumerator Process(int contextIdx)
    {   // 인덱스를 받아 CONTENT에 따라 대사를 처리하는 코루틴 함수
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
            case "~이동":
                GameManager.Instance.UpdateIdx(endIdx); // 테이블에서 가리킨 인덱스로 이동
                break;
            case "~챕터끝":
                yield return StartCoroutine(EndProcess(startIdx, endIdx)); // 챕터를 끝내는 처리 대기
                break;
        }
        contextIdx = GameManager.Instance.contextIdx; // 한 프로세스 끝나면 컨텍스트 갱신

        yield return StartCoroutine(Process(contextIdx)); // 다음 CONTENT 시작
    }

    IEnumerator NormalChatProcess(int startIdx, int endIdx)
    {   // 일반 대화를 진행하는 프로세스 함수
        yield return StartCoroutine(NormalChat(startIdx, endIdx)); // 일반 대화 처리 대기
        GameManager.Instance.UpdateIdx(++endIdx);
    }

    IEnumerator ChoiceProcess(int startIdx, int endIdx)
    {   // 선택지를 진행하는 프로세스 함수
        for (int i = startIdx; i <= endIdx; i++)
        {
            ActivateButtons(i - startIdx, i); // 선택지 활성화
        }

        yield return StartCoroutine(GetNumber()); // 선택지 고를 때까지 대기
        startIdx = int.Parse(scriptTable[GameManager.Instance.contextIdx]["START_POINT"].ToString());
        endIdx = int.Parse(scriptTable[GameManager.Instance.contextIdx]["END_POINT"].ToString());
        yield return StartCoroutine(NormalChat(startIdx, endIdx)); // 고른 선택지에 따른 대사 출력
        GameManager.Instance.UpdateIdx(++endIdx);
    }

    IEnumerator EndProcess(int startIdx, int endIdx)
    {   // 챕터 끝을 알리고 진행하는 프로세스 함수
        GameManager.Instance.UpdateIdx(++endIdx);
        GameManager.Instance.RestartGame();
        yield break;
    }

    IEnumerator ActionProcess(string action, int nowIdx)
    {   // 연출을 받고 처리하는 프로세스 함수
        // action 은 NormalChat()에서 제공 받는 한글 string
        if (action == "페이드 아웃")
            emptyBackgroundImage.CrossFadeAlpha(0f, 1f, true);
        else if (action == "페이드 인")
        {
            // 페이드 인 할 때 배경이 바뀐다면 스프라이트 변경
            emptyBackgroundImage.sprite = backgroundImages[int.Parse(scriptTable[nowIdx]["BACKGROUND"].ToString())];
            emptyBackgroundImage.CrossFadeAlpha(1f, 1f, true);
        }
        yield return null;
    }

    void ActivateButtons(int idx, int choiceIdx)
    {   // 선택지를 활성화 하는 함수
        choiceButtons[idx].gameObject.SetActive(true);
        var btn = choiceButtons[idx].GetComponent<BtnManager>();
        btn.choiceIdx = choiceIdx;
        btn.choiceContent = scriptTable[choiceIdx]["CONTENT"].ToString();
        btn.choiceTxt.text = btn.choiceContent;
    }

    IEnumerator GetNumber()
    {   // 선택지 클릭을 기다리는 함수
        while (!choosed) // BtnManager.ChooseNum()
        {
            yield return null;
        }
        choosed = false;
        foreach (var button in choiceButtons)
            button.gameObject.SetActive(false); // 선택지 비활성화
    }
}
