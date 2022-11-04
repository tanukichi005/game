using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearTextManager : MonoBehaviour
{
    public GameObject clearText;
    int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        score = GameManager.finalscore;
        if (score == 5000)
        {
            clearText.GetComponent<Text>().text = "高専な おハコ";
        }
        else if(score <= 2500){
            clearText.GetComponent<Text>().text = "ひよっこな おハコ";
        }
        else if(score <= 5000)
        {
            clearText.GetComponent<Text>().text = "見習いの おハコ";
        }
        else if(score <= 10000)
        {
            clearText.GetComponent<Text>().text = "半人前の おハコ";
        }
        else if(score <= 15000)
        {
            clearText.GetComponent<Text>().text = "一人前の おハコ";
        }
        else if(score <= 25000)
        {
            clearText.GetComponent<Text>().text = "ベテランの おハコ";
        }
        else if(score <= 50000)
        {
            clearText.GetComponent<Text>().text = "伝説の おハコ";
        }
        else if(score >= 50000)
        {
            clearText.GetComponent<Text>().text = "おハコ 仙人";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
