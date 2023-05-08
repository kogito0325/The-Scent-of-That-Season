using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;


/*
 * 타이틀 화면을 관리하는 매니저
 * 
 * 타이틀 화면에서 일어나는 모든 상호작용을 관리한다.
 * 
 * FadeIn()
 */

public class TitleManager : MonoBehaviour
{
    bool btnOn;  // 클릭 상태(F:디폴트, T:클릭 후)
    float fadeCount;  // UI 페이드 인 속도
    public CanvasGroup canvas;  // UI캔버스 오브젝트(Canvas)

    [Header("페이드 인 속도 조절")]
    [SerializeField][Range(1, 1000)] int fadeSpeed;

    private void Awake()
    {
        btnOn = false;  // 처음엔 false
        canvas.interactable = false;  // 완전히 켜지기 전까지 상호작용 불가
        fadeCount = (float)fadeSpeed / 10000;  // 수치 조정(그대로 쓰면 너무 빠름)
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
    {   // UI를 페이드 인 시키는 코루틴 함수
        while (canvas.alpha < 1f)
        {
            canvas.alpha += fadeCount;
            yield return new WaitForSeconds(0.01f);
        }
        // 상호작용 가능
        canvas.interactable = true;
    }
}
