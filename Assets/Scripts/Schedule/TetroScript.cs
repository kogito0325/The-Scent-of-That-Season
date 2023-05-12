using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/*
 * 테트로미노 오브젝트
 * 
 * 버튼을 클릭하여 생성된 테트로미노 블록 프리펩
 * 
 * SwitchSize()
 * OnTriggerEnter2D()
 * OnTriggerExit2D()
 */

public class TetroScript : MonoBehaviour
{
    Color beforeColor;  // 달력 칸 원래 색깔(검은색)
    Vector3 beforeSize;  // 블록 원래 크기
    public bool tiny;  // 크기 상태(false:원래상태 / true:작아진상태)
    public int space;  // 블록을 배치하기 위해 필요한 칸 수
    public int nowSpace;  // 현재 블록이 접촉하고 있는 칸 수
    public int eventNum;  // 블록의 이벤트 번호(ChapterTable {CHAPTER} / 0:알바)
    public Vector3 position;  // 달력에 배치할 때 부여되는 위치
    public GameObject tetroBtnPrefab;  // 배치 취소 시 리스트에 다시 넣을 프리펩
    
    void Awake()
    {
        beforeColor = new Color(.4980f, .4980f, .4980f, .5333f);
        beforeSize = transform.localScale;
        tiny = false;
        space = 4;
    }

    
    void Update()
    {
        transform.position = Input.mousePosition;  // 마우스 포인터에 위치 고정
        if (Input.GetMouseButtonDown(0) && nowSpace == space)
        {   // 만약 필요한 칸 수와 현재 칸 수가 같다면 배치
            GameObject.Find("Calendar").GetComponent<CalenderScript>().LocateTetromino();
        }
        if (Input.GetMouseButtonDown(1))
            transform.Rotate(new Vector3(0, 0, -90));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   // 달력에서 접촉해 있는 칸은 붉은 색으로 표시
        if (collision.tag == "Tile")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            collision.tag = "OnTiled";
            nowSpace++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {   // 접촉하지 않은 칸은 원래 색(beforeColor)으로 표시
        if (collision.tag == "OnTiled")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = beforeColor;
            collision.tag = "Tile";
            nowSpace--;
        }
    }

    public void SwitchSize()
    {   // 사이즈 스위칭(버튼 클릭 시 작아지고 배치 시 (원래 크기로) 커짐
        transform.localScale = tiny ? beforeSize : transform.localScale * 0.8f;
        tiny = !tiny;
    }
}
