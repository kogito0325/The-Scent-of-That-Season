using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameScript : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField inputName;

    public void Save()
    {
        PlayerPrefs.SetString("Name", inputName.text);
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
