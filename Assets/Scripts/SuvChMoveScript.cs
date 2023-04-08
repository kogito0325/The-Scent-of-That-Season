using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SuvChMoveScript : MonoBehaviour
{
    public Sprite[] Foods;
    public Sprite[] Drinks;
    public Transform[] sits;
    public Transform thisSit;
    public SpriteRenderer[] thisImage;

    // Start is called before the first frame update
    void Start()
    {
        thisSit = sits[UnityEngine.Random.Range(0, sits.Length)];
        transform.position = thisSit.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void ActivateImage()
    {
        thisImage[0].sprite = Foods[UnityEngine.Random.Range(0, Foods.Length)];
        thisImage[1].sprite = Drinks[UnityEngine.Random.Range(0, Drinks.Length)];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enterd");
        
        if (other.gameObject.CompareTag("sits")) {
            ActivateImage();
        }
    }

}
