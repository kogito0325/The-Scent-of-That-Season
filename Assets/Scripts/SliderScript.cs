using UnityEngine;
using UnityEngine.UI;

public enum SliderType { MasterVol, BgmVol, FxVol, PrintSpeed, AutoSpeed }

public class SliderScript : MonoBehaviour
{
    public SliderType myType;
    Slider mySlider;

    private void Awake()
    {
        mySlider = GetComponent<Slider>();
    }
    private void Start()
    {
        switch (myType)
        {
            case SliderType.MasterVol:
                mySlider.value = GameManager.Instance.masterVol;
                break;
            case SliderType.BgmVol:
                mySlider.value = GameManager.Instance.bgmVol;
                break;
            case SliderType.FxVol:
                mySlider.value = GameManager.Instance.fxVol;
                break;
            case SliderType.PrintSpeed:
                mySlider.value = GameManager.Instance.textSpeed;
                break;
            case SliderType.AutoSpeed:
                mySlider.value = GameManager.Instance.coolTime;
                break;
        }
    }

    public void ValueChange()
    {
        switch (myType)
        {
            case SliderType.MasterVol:
                GameManager.Instance.masterVol = mySlider.value;
                GameManager.Instance.ChangeVol();
                break;
            case SliderType.BgmVol:
                GameManager.Instance.bgmVol = mySlider.value;
                GameManager.Instance.ChangeVol();
                break;
            case SliderType.FxVol:
                GameManager.Instance.fxVol = mySlider.value;
                GameManager.Instance.ChangeVol();
                break;
            case SliderType.PrintSpeed:
                GameManager.Instance.textSpeed = mySlider.value;
                break;
            case SliderType.AutoSpeed:
                GameManager.Instance.coolTime = mySlider.value;
                break;
        }
    }
}
