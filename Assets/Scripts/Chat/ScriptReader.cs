using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ScriptReader : MonoBehaviour
{
    public ChatCharacter[] characters; // 캐릭터들
    public Image emptyBackgroundImage; // 배경 이미지 홀더
    public Image fadeImage; // 페이드인, 아웃 구현 위한 이미지 (필요없음)

    public Button[] choiceButtons; // 선택지 UI 리스트
    public Sprite[] backgroundImages; // 배경 이미지 리스트

    public ChatManager chatManager;  // 챗 매니저

    public GameObject shaderEffect;

    public Text ChatText; // 대사
    public Text CharacterName; // 캐릭터 이름

    public string writerText = ""; // 출력되고 있는 문자열

    public bool choosed = false; // 선택지 눌렀는지 체크하는 부울 변수
    public bool quickPrint = false;

    List<Dictionary<string, object>> scriptTable; // 대사 테이블
    List<Dictionary<string, object>> chapterTable; // 챕터 테이블
    List<Dictionary<string, object>> nameTable; // 캐릭터 이름 테이블
    List<Dictionary<string, object>> faceTable;


    private void Awake()
    {
        // 테이블 초기화
        scriptTable = GameManager.Instance.scriptTable;
        chapterTable = GameManager.Instance.chapterTable;
        nameTable = GameManager.Instance.nameTable;
        faceTable= GameManager.Instance.faceTable;
    }
    void Start()
    {
        // 게임 매니저의 현재 인덱스(contextIdx) 참조
        // 프로세스 진행
        StartCoroutine(Process(GameManager.Instance.contextIdx));
    }

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !quickPrint)
        {
            quickPrint = true;
        }
    }

    public IEnumerator TextPrint(string narrator, string narration, int nameIdx)
    {
        // 실제로 캐릭터 대사를 문자열로 받아 출력하는 코루틴 함수
        // NormalChat 에서 돌아감

        // 캐릭터 마다 텍스트 색깔 바꾸기
        // 이름이 ???인 캐릭터에 맞춰서 색깔 바꾸기
        int colorIdx = nameIdx < 5 ? nameIdx : 1;
        CharacterName.color = GameManager.Instance.HexColor(colorIdx);
        ChatText.color = GameManager.Instance.HexColor(colorIdx);
        CharacterName.text = narrator;
        writerText = "";

        quickPrint = false;

        for (int i = 0; i < narration.Length; i++)
        {   // 출력 속도에 따라 한글자씩 화면에 출력
            writerText += narration[i];
            ChatText.text = writerText;
            if (quickPrint)  // 출력 중에 클릭 하면 즉시 전문 출력
            {
                ChatText.text = narration;
                break;
            }
            yield return new WaitForSeconds(GameManager.Instance.textSpeed);
        }

        // 실수로 넘기기 방지
        yield return new WaitForSeconds(0.3f);

        while (true)
        {   // 넘길 때 까지 대기
            // (왼클릭 하거나, 스페이스바 누르거나, 자동 넘김 모드일 때) and 멈춤 상태가 아닐 때
            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || GameManager.Instance.autoMode) && !GameManager.Instance.paused)
            {
                if (GameManager.Instance.autoMode)  // 자동 넘김 모드면 쿨타임 돌리기
                    yield return StartCoroutine(AutoCoolDown());
                break;
            }
            yield return null;
        }
    }

    public IEnumerator AutoCoolDown()
    {   // 쿨타임 만큼 대기하는 코루틴 함수
        float coolTime = GameManager.Instance.coolTime;
        yield return new WaitForSeconds(coolTime);
    }

    public IEnumerator NormalChat(int startIdx, int endIdx)
    {   // 일반적인 대사를 출력하는 코루틴 함수
        string gottenName = GameManager.Instance.playerName; // 컴에 저장된 이름 불러오기
        string name; // 캐릭터 이름
        string content; // 캐릭터가 말하는 대사
        string action; // 연출
        string sound; // 사운드 연출

        for (int i = startIdx; i <= endIdx; i++)
        {
            int nameIdx = int.Parse(scriptTable[i]["CHARACTER"].ToString()); // 대사치는 캐릭터 이름 인덱스 불러오기
            int f1 = int.Parse(scriptTable[i]["FACE_1"].ToString()); // 대사치는 캐릭터 표정 인덱스 불러오기
            int f2 = int.Parse(scriptTable[i]["FACE_2"].ToString()); // 대사치는 캐릭터 표정 인덱스 불러오기
            int f3 = int.Parse(scriptTable[i]["FACE_3"].ToString()); // 대사치는 캐릭터 표정 인덱스 불러오기
            name = nameTable[nameIdx]["NAME"].ToString(); // 네임 테이블에서 인덱싱
            action = scriptTable[i]["ACTION"].ToString(); // 연출 불러오기
            sound = scriptTable[i]["SOUND"].ToString(); // 사운드 연출 불러오기
            content = scriptTable[i]["CONTENT"].ToString(); // 대사 불러오기

            // 테이블에서 "주인공" 이면 저장된 이름으로 변경
            if (name.Equals("주인공"))
                name = gottenName;
            content = content.Replace("주인공", gottenName);

            chatManager.AddLog(name, content);

            characters[0].GetComponent<Image>().fillAmount = f1 > 0 ? 1 : 0;
            characters[1].GetComponent<Image>().fillAmount = f2 > 0 ? 1 : 0;
            characters[2].GetComponent<Image>().fillAmount = f3 > 0 ? 1 : 0;

            FaceProcess(f1, f2, f3);
            yield return StartCoroutine(SoundProcess(sound, i)); // 연출이 있으면 연출 실행
            yield return StartCoroutine(ActionProcess(action, i)); // 연출이 있으면 연출 실행
            yield return StartCoroutine(TextPrint(name, content, nameIdx)); // 대사가 다 출력될 때까지 대기
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
        GameManager.Instance.PopTask();
        GameManager.Instance.CheckNextMonth();

        // 첫만남 에피소드 후 선택한 선택지에 따라 호감도 증가
        foreach (var s in GameManager.Instance.inactiveBtns.Split(',').Where(x => int.Parse(x) >= 980))
        {
            int i = int.Parse(s);
            if (i - 980 == 0 && GameManager.Instance.loveSummer == 0)
                GameManager.Instance.loveSummer += 5;
            else if (i - 980 == 1 && GameManager.Instance.loveFall == 0)
                GameManager.Instance.loveFall += 5;
            else if (i - 980 == 2 && GameManager.Instance.loveWinter == 0)
                GameManager.Instance.loveWinter += 5;
        }
        
        GameManager.Instance.LoadScheduleScene();
        yield break;
    }

    IEnumerator SoundProcess(string sound, int nowIdx)
    {   // 사운드 연출을 받고 처리하는 프로세스 함수
        // sound 는 NormalChat()에서 제공 받는 한글 문자열 (엑셀 {SOUND})
        var str = scriptTable[nowIdx]["BGM"].ToString();
        if (str.Length != 0)
        {
            var bgmIdx = int.Parse(scriptTable[nowIdx]["BGM"].ToString());
            if (bgmIdx == 0)
                GameManager.Instance.bgmPlayer.mute = true;
            else if (bgmIdx > 0)
            {
                if (Array.IndexOf(GameManager.Instance.bgms, GameManager.Instance.bgmPlayer.clip) != bgmIdx)
                    GameManager.Instance.bgmPlayer.clip = GameManager.Instance.bgms[bgmIdx];
                GameManager.Instance.bgmPlayer.mute = false;
                StartCoroutine(GameManager.Instance.SoundFadeIn());
            }
        }
        int soundIdx = 0;
        switch (sound)
        {
            case "띵동":
                soundIdx = 1;
                break;
            case "딸랑":
                soundIdx = 2;
                break;
            case "쨍그랑":
                soundIdx = 3;
                break;
            case "웅성웅성":
                soundIdx = 4;
                break;
            case "삐이이":
                soundIdx = 5;
                break;
            case "뻐억":
                soundIdx = 6;
                break;
            case "털썩":
                soundIdx = 7;
                break;
            case "또각또각":
                soundIdx = 8;
                break;
            case "쾅":
                soundIdx = 9;
                break;
        }
        if (soundIdx != 0)
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayClip(soundIdx);

        yield return null;
    }

    IEnumerator ActionProcess(string action, int nowIdx)
    {   // 연출을 받고 처리하는 프로세스 함수
        // action 은 NormalChat()에서 제공 받는 한글 문자열 (엑셀 {ACTION})
        if (action == "페이드 아웃")
        {
            emptyBackgroundImage.CrossFadeAlpha(0f, 1f, true);
            yield return new WaitForSeconds(1f);
        }
        else if (action == "페이드 인")
        {
            // 페이드 인 할 때 배경이 바뀐다면 스프라이트 변경
            emptyBackgroundImage.sprite = backgroundImages[int.Parse(scriptTable[nowIdx]["BACKGROUND"].ToString())];
            emptyBackgroundImage.CrossFadeAlpha(1f, 1f, true);
            yield return new WaitForSeconds(1f);
        }
        else if (action == "화면 흔들림")
        {
            StartCoroutine(ShakeCamera(20, 1f));
            yield return new WaitForSeconds(1f);
        }
        else if (action == "화면 확대 1")
        {
            for (int i = 0; i < 3; i++)
            {
                characters[i].rectTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                characters[i].SetPos(characters[i].rectTransform.localPosition.x, -800, 0);
            }
        }
        else if (action == "화면 확대 2")
        {
            for (int i = 0; i < 3; i++)
            {
                characters[i].rectTransform.localScale = new Vector3(2f, 2f, 2f);
                characters[i].SetPos(characters[i].rectTransform.localPosition.x, -1200, 0);
            }
        }
        else if (action == "화면 복구")
        {
            for (int i = 0; i < 3; i++)
            {
                characters[i].rectTransform.localScale = new Vector3(1f, 1f, 1f);
                characters[i].SetPos(characters[i].rectTransform.localPosition.x, -400, 0);
            }
        }
        else if (action == "화면 꺼짐")
        {
            emptyBackgroundImage.CrossFadeAlpha(0f, 0f, true);
        }
        else if (action == "깜빡임 시작")
            shaderCoroutine = StartCoroutine(ShaderControl());
        else if (action == "깜빡임 끝")
        {
            StopCoroutine(shaderCoroutine);
            SetShaderDefault();
        }

        yield return null;
    }

    public void FaceProcess(int idx1, int idx2, int idx3)
    {
        int[] idxs = { idx1, idx2, idx3 };
        int eb, e, m, f1, f2, f3;
        for (int i = 0; i < idxs.Length;i++)
        {
            eb = int.Parse(faceTable[idxs[i]]["eyebrow"].ToString());
            e = int.Parse(faceTable[idxs[i]]["eye"].ToString());
            m = int.Parse(faceTable[idxs[i]]["mouth"].ToString());
            f1 = int.Parse(faceTable[idxs[i]]["efx1"].ToString());
            f2 = int.Parse(faceTable[idxs[i]]["efx2"].ToString());
            f3 = int.Parse(faceTable[idxs[i]]["efx3"].ToString());
            characters[i].SetFace(eb, e, m, f1, f2, f3);
        }
    }

    IEnumerator ShakeCamera(float _amount, float _duration)
    {
        Transform cameraPos = emptyBackgroundImage.transform;
        Vector3 originPos = cameraPos.localPosition;

        float timer = 0;
        while (timer <= _duration)
        {
            cameraPos.localPosition = (Vector3)UnityEngine.Random.insideUnitCircle * _amount * (_duration - timer) + originPos;

            timer += Time.deltaTime;
            yield return null;
        }
        cameraPos.localPosition = originPos;
    }

    Coroutine shaderCoroutine;
    IEnumerator ShaderControl()
    {
        Material blurMtr = shaderEffect.GetComponent<CanvasRenderer>().GetMaterial();
        float shaderTimeScale = 20f;
        while (true)
        {
            for (float i = 0; i < shaderTimeScale; i += shaderTimeScale * Time.deltaTime)
            {
                blurMtr.SetFloat("_Radius", i);
                yield return null;
            }

            for (float i = shaderTimeScale; i > 0; i -= shaderTimeScale * Time.deltaTime)
            {
                blurMtr.SetFloat("_Radius", i);
                yield return null;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void SetShaderDefault()
    {
        Material blurMtr = shaderEffect.GetComponent<CanvasRenderer>().GetMaterial();
        blurMtr.SetFloat("_Radius", 0);
    }

    void ActivateButtons(int btnIdx, int choiceIdx)
    {   // 선택지를 활성화 하는 함수
        // btnIdx - 선택지 순서 [0, 1, 2]
        // choiceIdx - 엑셀에서의 선택지 인덱스 (~선택지 {START_POINT ~ END_POINT})
        var btn = choiceButtons[btnIdx].GetComponent<BtnManager>();
        btn.gameObject.SetActive(true);
        btn.choiceIdx = choiceIdx;
        btn.choiceContent = scriptTable[choiceIdx]["CONTENT"].ToString();
        btn.choiceTxt.text = btn.choiceContent;
        string[] temp = GameManager.Instance.inactiveBtns.Split(',');
        foreach (var s in temp)
        {
            if (btn.choiceIdx == int.Parse(s))
            {
                btn.gameObject.SetActive(false);
                break;
            }
        }
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
