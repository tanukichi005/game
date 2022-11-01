using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtendNum : MonoBehaviour
{
    private Text heartText = null;
    private int oldHeartNum = 0;

　　　// Start is called before the first frame update 
    void Start()
    {
        heartText = GetComponent<Text>();
        if (PlayerController.instance != null)
        {
            heartText.text = ":" + PlayerController.instance.heartNum;
        }
        else
        {
            Debug.Log("ゲームマネージャー置き忘れてるよ！");
            Destroy(this); }
        }

　　　// Update is called once per frame
    void Update()
    {
        if (oldHeartNum != PlayerController.instance.heartNum)
        {
            heartText.text = ":" + PlayerController.instance.heartNum;
            oldHeartNum = PlayerController.instance.heartNum;
        }
    }
}