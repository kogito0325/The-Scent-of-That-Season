using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TetroScript : MonoBehaviour
{
    Color beforeColor;
    Vector3 beforeSize;
    public bool tiny;
    public int space;
    public int nowSpace;
    public int eventNum;

    
    void Awake()
    {
        beforeColor = new Color(.4980f, .4980f, .4980f, .5333f);
        beforeSize = transform.localScale;
        tiny = false;
        space = 4;
    }

    
    void Update()
    {
        transform.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(0) && nowSpace == space)
        {
            GameObject.Find("Calendar").GetComponent<CalenderScript>().LocateTetromino();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tile")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            collision.tag = "OnTiled";
            nowSpace++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "OnTiled")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = beforeColor;
            collision.tag = "Tile";
            nowSpace--;
        }
    }

    public void SwitchSize()
    {
        if (!tiny)
        {
            tiny = true;
            transform.localScale = transform.localScale * 0.8f;
        }
        else if (tiny)
        {
            tiny = false;
            transform.localScale = beforeSize;
        }
    }
}
