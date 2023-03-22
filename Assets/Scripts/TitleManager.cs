using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    bool btnOn;
    float fadeCount;
    public CanvasGroup canvas;

    [Header("페이드 인 속도 조절")]
    [SerializeField][Range(1, 1000)] int fadeSpeed;

    private void Awake()
    {
        btnOn = false;
        canvas.interactable = false;
        fadeCount = (float)fadeSpeed / 10000;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !btnOn)
        {
            btnOn = true;
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        while (canvas.alpha < 1f)
        {
            canvas.alpha += fadeCount;
            yield return new WaitForSeconds(0.01f);
        }
        canvas.interactable = true;
    }
}
