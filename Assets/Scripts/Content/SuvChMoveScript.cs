using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


/*
 * 손님 오브젝트
 * 
 * 생성 -> 이동 -> 착석 -> 주문 -> 퇴장
 * 
 * InitRoots()
 * SelectRoot()
 * ActivateImage()
 * ActivateHpBar()
 * MoveProcess()
 * Move(startPos, endPos)
 * Sit()
 * Wait()
 * EscapeMap()
 * OnTriggerStay2D()
 * OnDestroy()
 */

public class SuvChMoveScript : MonoBehaviour
{
    Animator anim;  // 애니메이터
    public Sprite[] Foods;  // 음식 스프라이트들
    public Sprite[] Drinks;  // 음료 스프라이트들
    public Sprite[] Sits;  // 앉아있는 모습 스프라이트(0:왼쪽, 1:오른쪽)

    public SpriteRenderer[] thisImage;  // 주문 하는 음식, 음료

    // 루트들
    /*
     * 1 2  5 6
     * 
     * 3 4  7 8
     */
    public Transform[] roots1;
    public Transform[] roots2;
    public Transform[] roots3;
    public Transform[] roots4;
    public Transform[] roots5;
    public Transform[] roots6;
    public Transform[] roots7;
    public Transform[] roots8;

    public Transform[] thisRoot; // 선택한 루트

    public GameObject foodBox;  // 말풍선
    public GameObject canvas;  // 캔버스 오브젝트
    public GameObject hpBarPrefab;  // 주문 대기 시간 게이지바 프리펩

    public GameObject[] faces;  // 주문 후 얼굴(0:웃는얼굴, 1:화난얼굴)

    ContentInventory player;  // 플레이어 오브젝트
    RectTransform hpBar;  // 게이지바 위치
    ContentManager contentManager;  // 콘텐츠 매니저
    SpriteRenderer spriteRenderer;  // 스프라이트 렌더러 컴포넌트

    float speed = 5f;  // 이동 속도
    float stdTime = 20f;  // 주문 대기 시간
    int rootIdx = 0;  // 랜덤으로 루트를 정하는 인덱스 -> 앉은자리(홀수:왼쪽, 짝수:오른쪽)
    bool satisfied = false;  // 만족도 부울 변수(true:올바른 서빙 / false:대기 만료)
    bool timeOut = false;  // 주문 대기 부울 변수(true:대기시간 초과 / false:대기 중)

    void InitRoots()
    {   // 루트들 정보를 초기화 하는 함수
        // 기존의 씬에서 끌어왔던 방법이 프리펩에서는 안먹히니까 Find()로 직접 찾아서 초기화
        // 0 -> 1 -> 2 -> 3
        
        // 첫번째 위치
        roots1[0] =
        roots2[0] =
        roots3[0] =
        roots4[0] =
        roots5[0] =
        roots6[0] =
        roots7[0] =
        roots8[0] = GameObject.Find("p1").transform;

        // 두번째 위치
        roots1[1] =
        roots2[1] =
        roots3[1] =
        roots4[1] =
        roots5[1] =
        roots6[1] =
        roots7[1] =
        roots8[1] = GameObject.Find("p2").transform;

        // 세번째, 네번째 위치
        roots1[2] = GameObject.Find("Leftp").transform;
        roots1[3] = GameObject.Find("Leftp1").transform;

        roots2[2] = GameObject.Find("Leftp0").transform;
        roots2[3] = GameObject.Find("Leftp2").transform;

        roots3[2] = GameObject.Find("Leftp").transform;
        roots3[3] = GameObject.Find("Leftp3").transform;

        roots4[2] = GameObject.Find("Leftp0").transform;
        roots4[3] = GameObject.Find("Leftp4").transform;

        roots5[2] = GameObject.Find("Rightp0").transform;
        roots5[3] = GameObject.Find("Rightp1").transform;

        roots6[2] = GameObject.Find("Rightp").transform;
        roots6[3] = GameObject.Find("Rightp2").transform;

        roots7[2] = GameObject.Find("Rightp0").transform;
        roots7[3] = GameObject.Find("Rightp3").transform;

        roots8[2] = GameObject.Find("Rightp").transform;
        roots8[3] = GameObject.Find("Rightp4").transform;
    }

    IEnumerator SelectRoot()
    {   // 루트를 선택하는 코루틴 함수
        // 객체 생성 시 실행
        while (true)
        {   // 랜덤으로 루트 정하기
            rootIdx = UnityEngine.Random.Range(0, 8);

            // 뽑은 루트의 손님이 이미 있다면 다시 뽑기
            // 만석이면 제자리에서 기다림
            if (!contentManager.checkList[rootIdx])
            {
                contentManager.checkList[rootIdx] = true;
                break;
            }
            yield return null;
        }

        // 랜덤으로 뽑은 루트 인덱스 적용
        switch (rootIdx)
        {
            case 0:
                thisRoot = roots1;
                break;
            case 1:
                thisRoot = roots2;
                break;
            case 2:
                thisRoot = roots3;
                break;
            case 3:
                thisRoot = roots4;
                break;
            case 4:
                thisRoot = roots5;
                break;
            case 5:
                thisRoot = roots6;
                break;
            case 6:
                thisRoot = roots7;
                break;
            case 7:
                thisRoot = roots8;
                break;
        }
        rootIdx ++;  // 자리 정보

        // 이동 시작
        StartCoroutine(MoveProcess());
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").GetComponent<ContentInventory>();
        contentManager = GameObject.Find("ContentManager").GetComponent<ContentManager>();
        foodBox.SetActive(false);
        InitRoots();

        StartCoroutine(SelectRoot());
    }

    private void ActivateImage()
    {
        thisImage[0].sprite = Foods[UnityEngine.Random.Range(0, Foods.Length)];
        thisImage[1].sprite = Drinks[UnityEngine.Random.Range(0, Drinks.Length)];
        foodBox.SetActive(true);
    }

    IEnumerator ActivateHpBar()
    {
        float orderTime = stdTime;
        canvas = GameObject.Find("Canvas");
        hpBar = Instantiate(hpBarPrefab, canvas.transform).GetComponent<RectTransform>();
        hpBar.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.4f, 0));
        while (!satisfied && !timeOut)
        {
            orderTime -= 1 * Time.deltaTime;
            hpBar.GetComponent<Image>().fillAmount = orderTime / stdTime;
            yield return null;
        }
        Destroy(hpBar.gameObject);
    }

    IEnumerator MoveProcess()
    {
        Vector3 startPos;
        Vector3 targetPos;
        for (int i = 0; i < thisRoot.Length; i++)
        {
            startPos = transform.position;
            targetPos = thisRoot[i].position;
            yield return StartCoroutine(Move(startPos, targetPos));
        }

        StartCoroutine(Sit());
    }

    IEnumerator Move(Vector3 startPos, Vector3 endPos)
    {
        if (endPos.x < startPos.x)
            anim.SetInteger("Condition", 3);
        else if (endPos.y < startPos.y)
            anim.SetInteger("Condition", 2);
        else if (endPos.x > startPos.x)
            anim.SetInteger("Condition", 4);
        else if (endPos.y > startPos.y)
            anim.SetInteger("Condition", 1);

        while (Math.Abs(Vector3.Distance(transform.position, endPos)) > 0)
        {
            transform.position = Vector3.MoveTowards(startPos, endPos, speed * Time.deltaTime);
            startPos = transform.position;
            yield return null;
        }
    }

    IEnumerator Sit()
    {
        ActivateImage();
        anim.enabled = false;
        if (gameObject.name[3] == '1')  // 왼쪽 오른쪽이 다른 캐릭터
            spriteRenderer.sprite = Sits[rootIdx%2==0?1:0];
        else  // 나머지 캐릭터
        {
            if (rootIdx % 2 == 1)
                spriteRenderer.flipX = true;
            spriteRenderer.sprite = Sits[0];
        }

        StartCoroutine(ActivateHpBar());
        StartCoroutine(Wait());
        yield return null;
    }

    IEnumerator Wait()
    {
        float orderTime = stdTime;

        while (true)
        {
            orderTime -= 1 * Time.deltaTime;
            if (satisfied || orderTime < 0)
            {
                thisImage[0].gameObject.SetActive(false);
                thisImage[1].gameObject.SetActive(false);
                faces[satisfied ? 0 : 1].SetActive(true);
                timeOut = true;
                break;
            }
            yield return null;
        }

        if (satisfied)
            GameManager.Instance.money += 1000;
        else
            player.life--;

        yield return new WaitForSeconds(3f);
        spriteRenderer.flipX = false;
        StartCoroutine(EscapeMap());
    }

    IEnumerator EscapeMap()
    {
        anim.enabled = true;
        speed /= 2f;
        yield return StartCoroutine(Move(transform.position, thisRoot[2].position));
        yield return StartCoroutine(Move(transform.position, thisRoot[1].position));
        yield return StartCoroutine(Move(transform.position, transform.position + new Vector3(0, -10, 0)));

        contentManager.checkList[--rootIdx] = false;
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player" && Input.GetKey(KeyCode.F) && !timeOut)
        {
            if (player.images[0].sprite == thisImage[0].sprite && player.images[1].sprite == thisImage[1].sprite)
            {
                satisfied = true;
                player.images[0].gameObject.SetActive(false);
                player.images[1].gameObject.SetActive(false);
            }
        }
    }
}

