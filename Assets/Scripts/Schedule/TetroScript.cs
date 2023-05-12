using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/*
 * ��Ʈ�ι̳� ������Ʈ
 * 
 * ��ư�� Ŭ���Ͽ� ������ ��Ʈ�ι̳� ��� ������
 * 
 * SwitchSize()
 * OnTriggerEnter2D()
 * OnTriggerExit2D()
 */

public class TetroScript : MonoBehaviour
{
    Color beforeColor;  // �޷� ĭ ���� ����(������)
    Vector3 beforeSize;  // ��� ���� ũ��
    public bool tiny;  // ũ�� ����(false:�������� / true:�۾�������)
    public int space;  // ����� ��ġ�ϱ� ���� �ʿ��� ĭ ��
    public int nowSpace;  // ���� ����� �����ϰ� �ִ� ĭ ��
    public int eventNum;  // ����� �̺�Ʈ ��ȣ(ChapterTable {CHAPTER} / 0:�˹�)
    public Vector3 position;  // �޷¿� ��ġ�� �� �ο��Ǵ� ��ġ
    public GameObject tetroBtnPrefab;  // ��ġ ��� �� ����Ʈ�� �ٽ� ���� ������
    
    void Awake()
    {
        beforeColor = new Color(.4980f, .4980f, .4980f, .5333f);
        beforeSize = transform.localScale;
        tiny = false;
        space = 4;
    }

    
    void Update()
    {
        transform.position = Input.mousePosition;  // ���콺 �����Ϳ� ��ġ ����
        if (Input.GetMouseButtonDown(0) && nowSpace == space)
        {   // ���� �ʿ��� ĭ ���� ���� ĭ ���� ���ٸ� ��ġ
            GameObject.Find("Calendar").GetComponent<CalenderScript>().LocateTetromino();
        }
        if (Input.GetMouseButtonDown(1))
            transform.Rotate(new Vector3(0, 0, -90));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   // �޷¿��� ������ �ִ� ĭ�� ���� ������ ǥ��
        if (collision.tag == "Tile")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            collision.tag = "OnTiled";
            nowSpace++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {   // �������� ���� ĭ�� ���� ��(beforeColor)���� ǥ��
        if (collision.tag == "OnTiled")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = beforeColor;
            collision.tag = "Tile";
            nowSpace--;
        }
    }

    public void SwitchSize()
    {   // ������ ����Ī(��ư Ŭ�� �� �۾����� ��ġ �� (���� ũ���) Ŀ��
        transform.localScale = tiny ? beforeSize : transform.localScale * 0.8f;
        tiny = !tiny;
    }
}
