using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public GameObject title;
    public GameObject btns;

    bool btnsOn;

    [Header("이동속도 조절")]
    [SerializeField][Range(1f, 30f)] float none = 20f;

    private void Awake()
    {
        btnsOn = false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }
}
