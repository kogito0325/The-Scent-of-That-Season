using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceTest : MonoBehaviour
{
    public ChatCharacter[] characters;
    public ChatCharacter nowCharacter;

    public Text chrTxt;
    public Text clothTxt;
    public Text eyebrowTxt;
    public Text eyeTxt;
    public Text mouthTxt;
    public Text efx1Txt;
    public Text efx2Txt;
    public Text efx3Txt;

    int chrIdx = 0;
    int clothIdx = 1;
    int eyebrowIdx = 1;
    int eyeIdx = 1;
    int mouthIdx = 1;
    int efx1Idx = 0;
    int efx2Idx = 0;
    int efx3Idx = 0;

    private void Start()
    {
        CharacterUpdate();
        FaceUpdate();
    }

    public void ClothControl(int num)
    {
        clothIdx += num;
        if (clothIdx >= nowCharacter.clothes.Length)
            clothIdx = nowCharacter.clothes.Length-1;
        else if (clothIdx < 0)
            clothIdx = 0;
        FaceUpdate();
    }

    public void EyebrowControl(int num)
    {
        eyebrowIdx += num;
        if (eyebrowIdx >= nowCharacter.eyeborws.Length)
            eyebrowIdx = nowCharacter.eyeborws.Length - 1;
        else if (eyebrowIdx < 0)
            eyebrowIdx = 0;
        FaceUpdate();
    }

    public void EyeControl(int num)
    {
        eyeIdx += num;
        if (eyeIdx >= nowCharacter.eyes.Length)
            eyeIdx = nowCharacter.eyes.Length - 1;
        else if (eyeIdx < 0)
            eyeIdx = 0;
        FaceUpdate();
    }

    public void MouthControl(int num)
    {
        mouthIdx += num;
        if (mouthIdx >= nowCharacter.mouths.Length)
            mouthIdx = nowCharacter.mouths.Length - 1;
        else if (mouthIdx < 0)
            mouthIdx = 0;
        FaceUpdate();
    }

    public void Efx1Control(int num)
    {
        efx1Idx += num;
        if (efx1Idx >= nowCharacter.efxs.Length)
            efx1Idx = nowCharacter.efxs.Length - 1;
        else if (efx1Idx < 0)
            efx1Idx = 0;
        FaceUpdate();
    }

    public void Efx2Control(int num)
    {
        efx2Idx += num;
        if (efx2Idx >= nowCharacter.efxs.Length)
            efx2Idx = nowCharacter.efxs.Length - 1;
        else if (efx2Idx < 0)
            efx2Idx = 0;
        FaceUpdate();
    }

    public void Efx3Control(int num)
    {
        efx3Idx += num;
        if (efx3Idx >= nowCharacter.efxs.Length)
            efx3Idx = nowCharacter.efxs.Length - 1;
        else if (efx3Idx < 0)
            efx3Idx = 0;
        FaceUpdate();
    }

    public void CharacterControl(int num)
    {
        chrIdx += num;
        if (chrIdx >= characters.Length)
            chrIdx = characters.Length-1;
        else if (chrIdx < 0)
            chrIdx = 0;
        CharacterUpdate();
    }

    public void CharacterUpdate()
    {
        foreach (var character in characters)
        {
            character.gameObject.SetActive(false);
        }
        nowCharacter = characters[chrIdx];
        nowCharacter.gameObject.SetActive(true);
        chrTxt.text = nowCharacter.name;
    }

    public void FaceUpdate()
    {
        nowCharacter.myClothe.sprite = nowCharacter.clothes[clothIdx];
        nowCharacter.myEyebrow.sprite = nowCharacter.eyeborws[eyebrowIdx];
        nowCharacter.myEye.sprite = nowCharacter.eyes[eyeIdx];
        nowCharacter.myMouth.sprite = nowCharacter.mouths[mouthIdx];
        nowCharacter.myEfx1.sprite = nowCharacter.efxs[efx1Idx];
        nowCharacter.myEfx2.sprite = nowCharacter.efxs[efx2Idx];
        nowCharacter.myEfx3.sprite = nowCharacter.efxs[efx3Idx];

        clothTxt.text = clothIdx.ToString();
        eyebrowTxt.text = eyebrowIdx.ToString();
        eyeTxt.text = eyeIdx.ToString();
        mouthTxt.text = mouthIdx.ToString();
        efx1Txt.text = efx1Idx.ToString();
        efx2Txt.text = efx2Idx.ToString();
        efx3Txt.text = efx3Idx.ToString();
    }
}
