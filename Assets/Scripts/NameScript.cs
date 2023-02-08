using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameScript : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField inputName; //성
    public InputField inputName2; //이름

    public void Save()
    {
        PlayerPrefs.SetString("Name1", inputName.text); //성
        PlayerPrefs.SetString("Name2", inputName2.text); //이름
        SceneManager.LoadScene("GameScene");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
