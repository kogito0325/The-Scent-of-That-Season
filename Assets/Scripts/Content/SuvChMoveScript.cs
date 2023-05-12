using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


/*
 * �մ� ������Ʈ
 * 
 * ���� -> �̵� -> ���� -> �ֹ� -> ����
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
    Animator anim;  // �ִϸ�����
    public Sprite[] Foods;  // ���� ��������Ʈ��
    public Sprite[] Drinks;  // ���� ��������Ʈ��
    public Sprite[] Sits;  // �ɾ��ִ� ��� ��������Ʈ(0:����, 1:������)

    public SpriteRenderer[] thisImage;  // �ֹ� �ϴ� ����, ����

    // ��Ʈ��
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

    public Transform[] thisRoot; // ������ ��Ʈ

    public GameObject foodBox;  // ��ǳ��
    public GameObject canvas;  // ĵ���� ������Ʈ
    public GameObject hpBarPrefab;  // �ֹ� ��� �ð� �������� ������

    public GameObject[] faces;  // �ֹ� �� ��(0:���¾�, 1:ȭ����)

    ContentInventory player;  // �÷��̾� ������Ʈ
    RectTransform hpBar;  // �������� ��ġ
    ContentManager contentManager;  // ������ �Ŵ���
    SpriteRenderer spriteRenderer;  // ��������Ʈ ������ ������Ʈ

    float speed = 5f;  // �̵� �ӵ�
    float stdTime = 20f;  // �ֹ� ��� �ð�
    int rootIdx = 0;  // �������� ��Ʈ�� ���ϴ� �ε��� -> �����ڸ�(Ȧ��:����, ¦��:������)
    bool satisfied = false;  // ������ �ο� ����(true:�ùٸ� ���� / false:��� ����)
    bool timeOut = false;  // �ֹ� ��� �ο� ����(true:���ð� �ʰ� / false:��� ��)

    void InitRoots()
    {   // ��Ʈ�� ������ �ʱ�ȭ �ϴ� �Լ�
        // ������ ������ ����Դ� ����� �����鿡���� �ȸ����ϱ� Find()�� ���� ã�Ƽ� �ʱ�ȭ
        // 0 -> 1 -> 2 -> 3
        
        // ù��° ��ġ
        roots1[0] =
        roots2[0] =
        roots3[0] =
        roots4[0] =
        roots5[0] =
        roots6[0] =
        roots7[0] =
        roots8[0] = GameObject.Find("p1").transform;

        // �ι�° ��ġ
        roots1[1] =
        roots2[1] =
        roots3[1] =
        roots4[1] =
        roots5[1] =
        roots6[1] =
        roots7[1] =
        roots8[1] = GameObject.Find("p2").transform;

        // ����°, �׹�° ��ġ
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
    {   // ��Ʈ�� �����ϴ� �ڷ�ƾ �Լ�
        // ��ü ���� �� ����
        while (true)
        {   // �������� ��Ʈ ���ϱ�
            rootIdx = UnityEngine.Random.Range(0, 8);

            // ���� ��Ʈ�� �մ��� �̹� �ִٸ� �ٽ� �̱�
            // �����̸� ���ڸ����� ��ٸ�
            if (!contentManager.checkList[rootIdx])
            {
                contentManager.checkList[rootIdx] = true;
                break;
            }
            yield return null;
        }

        // �������� ���� ��Ʈ �ε��� ����
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
        rootIdx ++;  // �ڸ� ����

        // �̵� ����
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
        if (gameObject.name[3] == '1')  // ���� �������� �ٸ� ĳ����
            spriteRenderer.sprite = Sits[rootIdx%2==0?1:0];
        else  // ������ ĳ����
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

