using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stageController : MonoBehaviour
{

    [Header("プレイヤーゲームオブジェクト")] public GameObject playerObj;
    [Header("コンティニュー位置")] public GameObject[] continuePoint;

    private PlayerController p;

     // Start is called before the first frame update
    void Start()
    {
        if(playerObj != null && continuePoint != null && continuePoint.Length > 0)
        {
            playerObj.transform.position = continuePoint[0].transform.position;
            p = playerObj.GetComponent<PlayerController>();
            if(p == null)
            {
            Debug.Log("プレイヤーじゃない物がアタッチされているよ！");
            }   
        }
        else
        {
            Debug.Log("設定が足りてないよ！");
        }
    }

    //ディレイ用コルーチン
    IEnumerator time() 
    {
        //終わるまで待ってほしい処理を書く
        //例：敵が倒れるアニメーションを開始
        //2秒待つ
        yield return new WaitForSeconds(1.0f);
        //再開してから実行したい処理を書く
        //例：敵オブジェクトを破壊
    } 


    // Update is called once per frame
    void Update()
    {
        if(p != null && p.IsContinueWaiting()) 
        {
            if(continuePoint.Length > 0)
            {  
                playerObj.transform.position = continuePoint[0].transform.position;
                p.ContinuePlayer(); //New
            }
            else
            {  
                Debug.Log("コンティニューポイントの設定が足りてないよ！");
            }
        }
    }
}
