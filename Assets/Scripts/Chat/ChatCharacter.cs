using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatCharacter : MonoBehaviour
{
    public Sprite[] clothes;
    public Sprite[] eyeborws;
    public Sprite[] eyes;
    public Sprite[] mouths;
    public Sprite[] efxs;
    
    public Image myClothe;
    public Image myEyebrow;
    public Image myEye;
    public Image myMouth;
    public Image myEfx1;
    public Image myEfx2;
    public Image myEfx3;

    public RectTransform rectTransform;

    public void SetFace(int eb, int e, int m, int f1, int f2, int f3)
    {
        myEyebrow.sprite = eyeborws[eb];
        myEye.sprite = eyes[e];
        myMouth.sprite = mouths[m];
        myEfx1.sprite = efxs[f1];
        myEfx2.sprite = efxs[f2];
        myEfx3.sprite = efxs[f3];
    }

    public void SetCloth(int cIdx)
    {
        myClothe.sprite = eyes[cIdx];
    }

    public void SetPos(int pnum)
    {
        rectTransform.localPosition = new Vector3((pnum - 2) * 600, -400, 0);
    }
    public void SetPos(float x, float y, float z)
    {
        rectTransform.localPosition = new Vector3(x, y, z);
    }
}
