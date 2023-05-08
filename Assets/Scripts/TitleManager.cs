using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;


/*
 * Ÿ��Ʋ ȭ���� �����ϴ� �Ŵ���
 * 
 * Ÿ��Ʋ ȭ�鿡�� �Ͼ�� ��� ��ȣ�ۿ��� �����Ѵ�.
 * 
 * FadeIn()
 */

public class TitleManager : MonoBehaviour
{
    bool btnOn;  // Ŭ�� ����(F:����Ʈ, T:Ŭ�� ��)
    float fadeCount;  // UI ���̵� �� �ӵ�
    public CanvasGroup canvas;  // UIĵ���� ������Ʈ(Canvas)

    [Header("���̵� �� �ӵ� ����")]
    [SerializeField][Range(1, 1000)] int fadeSpeed;

    private void Awake()
    {
        btnOn = false;  // ó���� false
        canvas.interactable = false;  // ������ ������ ������ ��ȣ�ۿ� �Ұ�
        fadeCount = (float)fadeSpeed / 10000;  // ��ġ ����(�״�� ���� �ʹ� ����)
    }

    private void Start()
    {
        GameManager.Instance.ClosePage();
        GameManager.Instance.saveMode = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !btnOn)
        {
            btnOn = true;
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {   // UI�� ���̵� �� ��Ű�� �ڷ�ƾ �Լ�
        while (canvas.alpha < 1f)
        {
            canvas.alpha += fadeCount;
            yield return new WaitForSeconds(0.01f);
        }
        // ��ȣ�ۿ� ����
        canvas.interactable = true;
    }
}
