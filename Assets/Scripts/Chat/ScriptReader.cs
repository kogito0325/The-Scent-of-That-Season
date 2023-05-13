using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ScriptReader : MonoBehaviour
{
    public ChatCharacter[] characters; // ĳ���͵�
    public Image emptyBackgroundImage; // ��� �̹��� Ȧ��
    public Image fadeImage; // ���̵���, �ƿ� ���� ���� �̹��� (�ʿ����)

    public Button[] choiceButtons; // ������ UI ����Ʈ
    public Sprite[] backgroundImages; // ��� �̹��� ����Ʈ

    public ChatManager chatManager;  // ê �Ŵ���

    public GameObject shaderEffect;

    public Text ChatText; // ���
    public Text CharacterName; // ĳ���� �̸�

    public string writerText = ""; // ��µǰ� �ִ� ���ڿ�

    public bool choosed = false; // ������ �������� üũ�ϴ� �ο� ����
    public bool quickPrint = false;

    List<Dictionary<string, object>> scriptTable; // ��� ���̺�
    List<Dictionary<string, object>> chapterTable; // é�� ���̺�
    List<Dictionary<string, object>> nameTable; // ĳ���� �̸� ���̺�
    List<Dictionary<string, object>> faceTable;


    private void Awake()
    {
        // ���̺� �ʱ�ȭ
        scriptTable = GameManager.Instance.scriptTable;
        chapterTable = GameManager.Instance.chapterTable;
        nameTable = GameManager.Instance.nameTable;
        faceTable= GameManager.Instance.faceTable;
    }
    void Start()
    {
        // ���� �Ŵ����� ���� �ε���(contextIdx) ����
        // ���μ��� ����
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
        // ������ ĳ���� ��縦 ���ڿ��� �޾� ����ϴ� �ڷ�ƾ �Լ�
        // NormalChat ���� ���ư�

        // ĳ���� ���� �ؽ�Ʈ ���� �ٲٱ�
        // �̸��� ???�� ĳ���Ϳ� ���缭 ���� �ٲٱ�
        int colorIdx = nameIdx < 5 ? nameIdx : 1;
        CharacterName.color = GameManager.Instance.HexColor(colorIdx);
        ChatText.color = GameManager.Instance.HexColor(colorIdx);
        CharacterName.text = narrator;
        writerText = "";

        quickPrint = false;

        for (int i = 0; i < narration.Length; i++)
        {   // ��� �ӵ��� ���� �ѱ��ھ� ȭ�鿡 ���
            writerText += narration[i];
            ChatText.text = writerText;
            if (quickPrint)  // ��� �߿� Ŭ�� �ϸ� ��� ���� ���
            {
                ChatText.text = narration;
                break;
            }
            yield return new WaitForSeconds(GameManager.Instance.textSpeed);
        }

        // �Ǽ��� �ѱ�� ����
        yield return new WaitForSeconds(0.3f);

        while (true)
        {   // �ѱ� �� ���� ���
            // (��Ŭ�� �ϰų�, �����̽��� �����ų�, �ڵ� �ѱ� ����� ��) and ���� ���°� �ƴ� ��
            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || GameManager.Instance.autoMode) && !GameManager.Instance.paused)
            {
                if (GameManager.Instance.autoMode)  // �ڵ� �ѱ� ���� ��Ÿ�� ������
                    yield return StartCoroutine(AutoCoolDown());
                break;
            }
            yield return null;
        }
    }

    public IEnumerator AutoCoolDown()
    {   // ��Ÿ�� ��ŭ ����ϴ� �ڷ�ƾ �Լ�
        float coolTime = GameManager.Instance.coolTime;
        yield return new WaitForSeconds(coolTime);
    }

    public IEnumerator NormalChat(int startIdx, int endIdx)
    {   // �Ϲ����� ��縦 ����ϴ� �ڷ�ƾ �Լ�
        string gottenName = GameManager.Instance.playerName; // �Ŀ� ����� �̸� �ҷ�����
        string name; // ĳ���� �̸�
        string content; // ĳ���Ͱ� ���ϴ� ���
        string action; // ����
        string sound; // ���� ����

        for (int i = startIdx; i <= endIdx; i++)
        {
            int nameIdx = int.Parse(scriptTable[i]["CHARACTER"].ToString()); // ���ġ�� ĳ���� �̸� �ε��� �ҷ�����
            int f1 = int.Parse(scriptTable[i]["FACE_1"].ToString()); // ���ġ�� ĳ���� ǥ�� �ε��� �ҷ�����
            int f2 = int.Parse(scriptTable[i]["FACE_2"].ToString()); // ���ġ�� ĳ���� ǥ�� �ε��� �ҷ�����
            int f3 = int.Parse(scriptTable[i]["FACE_3"].ToString()); // ���ġ�� ĳ���� ǥ�� �ε��� �ҷ�����
            name = nameTable[nameIdx]["NAME"].ToString(); // ���� ���̺��� �ε���
            action = scriptTable[i]["ACTION"].ToString(); // ���� �ҷ�����
            sound = scriptTable[i]["SOUND"].ToString(); // ���� ���� �ҷ�����
            content = scriptTable[i]["CONTENT"].ToString(); // ��� �ҷ�����

            // ���̺��� "���ΰ�" �̸� ����� �̸����� ����
            if (name.Equals("���ΰ�"))
                name = gottenName;
            content = content.Replace("���ΰ�", gottenName);

            chatManager.AddLog(name, content);

            characters[0].GetComponent<Image>().fillAmount = f1 > 0 ? 1 : 0;
            characters[1].GetComponent<Image>().fillAmount = f2 > 0 ? 1 : 0;
            characters[2].GetComponent<Image>().fillAmount = f3 > 0 ? 1 : 0;

            FaceProcess(f1, f2, f3);
            yield return StartCoroutine(SoundProcess(sound, i)); // ������ ������ ���� ����
            yield return StartCoroutine(ActionProcess(action, i)); // ������ ������ ���� ����
            yield return StartCoroutine(TextPrint(name, content, nameIdx)); // ��簡 �� ��µ� ������ ���
        }
    }

    public IEnumerator Process(int contextIdx)
    {   // �ε����� �޾� CONTENT�� ���� ��縦 ó���ϴ� �ڷ�ƾ �Լ�
        int startIdx = int.Parse(scriptTable[contextIdx]["START_POINT"].ToString());
        int endIdx = int.Parse(scriptTable[contextIdx]["END_POINT"].ToString());
        string process = scriptTable[contextIdx]["CONTENT"].ToString();
        switch (process)
        {
            case "~��ȭ":
                yield return StartCoroutine(NormalChatProcess(startIdx, endIdx)); // �Ϲ� ��ȭ�� ���� ������ ���
                break;
            case "~������":
                yield return StartCoroutine(ChoiceProcess(startIdx, endIdx)); // ���� �� ���� ������ �� ���� ������ ���
                break;
            case "~�̵�":
                GameManager.Instance.UpdateIdx(endIdx); // ���̺��� ����Ų �ε����� �̵�
                break;
            case "~é�ͳ�":
                yield return StartCoroutine(EndProcess(startIdx, endIdx)); // é�͸� ������ ó�� ���
                break;
        }
        contextIdx = GameManager.Instance.contextIdx; // �� ���μ��� ������ ���ؽ�Ʈ ����

        yield return StartCoroutine(Process(contextIdx)); // ���� CONTENT ����
    }

    IEnumerator NormalChatProcess(int startIdx, int endIdx)
    {   // �Ϲ� ��ȭ�� �����ϴ� ���μ��� �Լ�
        yield return StartCoroutine(NormalChat(startIdx, endIdx)); // �Ϲ� ��ȭ ó�� ���
        GameManager.Instance.UpdateIdx(++endIdx);
    }

    IEnumerator ChoiceProcess(int startIdx, int endIdx)
    {   // �������� �����ϴ� ���μ��� �Լ�
        for (int i = startIdx; i <= endIdx; i++)
        {
            ActivateButtons(i - startIdx, i); // ������ Ȱ��ȭ
        }

        yield return StartCoroutine(GetNumber()); // ������ �� ������ ���
        startIdx = int.Parse(scriptTable[GameManager.Instance.contextIdx]["START_POINT"].ToString());
        endIdx = int.Parse(scriptTable[GameManager.Instance.contextIdx]["END_POINT"].ToString());
        yield return StartCoroutine(NormalChat(startIdx, endIdx)); // �� �������� ���� ��� ���
        GameManager.Instance.UpdateIdx(++endIdx);
    }

    IEnumerator EndProcess(int startIdx, int endIdx)
    {   // é�� ���� �˸��� �����ϴ� ���μ��� �Լ�
        GameManager.Instance.UpdateIdx(++endIdx);
        GameManager.Instance.PopTask();
        GameManager.Instance.CheckNextMonth();

        // ù���� ���Ǽҵ� �� ������ �������� ���� ȣ���� ����
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
    {   // ���� ������ �ް� ó���ϴ� ���μ��� �Լ�
        // sound �� NormalChat()���� ���� �޴� �ѱ� ���ڿ� (���� {SOUND})
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
            case "��":
                soundIdx = 1;
                break;
            case "����":
                soundIdx = 2;
                break;
            case "¸�׶�":
                soundIdx = 3;
                break;
            case "��������":
                soundIdx = 4;
                break;
            case "������":
                soundIdx = 5;
                break;
            case "����":
                soundIdx = 6;
                break;
            case "�н�":
                soundIdx = 7;
                break;
            case "�ǰ��ǰ�":
                soundIdx = 8;
                break;
            case "��":
                soundIdx = 9;
                break;
        }
        if (soundIdx != 0)
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayClip(soundIdx);

        yield return null;
    }

    IEnumerator ActionProcess(string action, int nowIdx)
    {   // ������ �ް� ó���ϴ� ���μ��� �Լ�
        // action �� NormalChat()���� ���� �޴� �ѱ� ���ڿ� (���� {ACTION})
        if (action == "���̵� �ƿ�")
        {
            emptyBackgroundImage.CrossFadeAlpha(0f, 1f, true);
            yield return new WaitForSeconds(1f);
        }
        else if (action == "���̵� ��")
        {
            // ���̵� �� �� �� ����� �ٲ�ٸ� ��������Ʈ ����
            emptyBackgroundImage.sprite = backgroundImages[int.Parse(scriptTable[nowIdx]["BACKGROUND"].ToString())];
            emptyBackgroundImage.CrossFadeAlpha(1f, 1f, true);
            yield return new WaitForSeconds(1f);
        }
        else if (action == "ȭ�� ��鸲")
        {
            StartCoroutine(ShakeCamera(20, 1f));
            yield return new WaitForSeconds(1f);
        }
        else if (action == "ȭ�� Ȯ�� 1")
        {
            for (int i = 0; i < 3; i++)
            {
                characters[i].rectTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                characters[i].SetPos(characters[i].rectTransform.localPosition.x, -800, 0);
            }
        }
        else if (action == "ȭ�� Ȯ�� 2")
        {
            for (int i = 0; i < 3; i++)
            {
                characters[i].rectTransform.localScale = new Vector3(2f, 2f, 2f);
                characters[i].SetPos(characters[i].rectTransform.localPosition.x, -1200, 0);
            }
        }
        else if (action == "ȭ�� ����")
        {
            for (int i = 0; i < 3; i++)
            {
                characters[i].rectTransform.localScale = new Vector3(1f, 1f, 1f);
                characters[i].SetPos(characters[i].rectTransform.localPosition.x, -400, 0);
            }
        }
        else if (action == "ȭ�� ����")
        {
            emptyBackgroundImage.CrossFadeAlpha(0f, 0f, true);
        }
        else if (action == "������ ����")
            shaderCoroutine = StartCoroutine(ShaderControl());
        else if (action == "������ ��")
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
    {   // �������� Ȱ��ȭ �ϴ� �Լ�
        // btnIdx - ������ ���� [0, 1, 2]
        // choiceIdx - ���������� ������ �ε��� (~������ {START_POINT ~ END_POINT})
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
    {   // ������ Ŭ���� ��ٸ��� �Լ�
        while (!choosed) // BtnManager.ChooseNum()
        {
            yield return null;
        }
        choosed = false;
        foreach (var button in choiceButtons)
            button.gameObject.SetActive(false); // ������ ��Ȱ��ȭ
    }
}
