using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public GameObject settingMenu;
    // Start is called before the first frame update
    void Start()
    {
        settingMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (settingMenu.activeSelf)
                settingMenu.SetActive(false);
            else
                settingMenu.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.Instance.autoMode = !GameManager.Instance.autoMode;
        }
    }
}
