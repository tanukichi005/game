using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject objPrefab; //発生させるプレハブデータ
    public float delayTime = 3.0f; //遅延時間
    public float fireSpeedX = -4.0f; //発射ベクトルX
    public float fireSpeedY = 0.0f; //発射ベクトルY
    public float length = 8.0f;

    GameObject player; //プレイヤー
    GameObject gateObj; //発射口
    float passedTimes = 0; //経過時間

    // Start is called before the first frame update
    void Start()
    {
        //発射口オブジェクトを取得
        Transform tr = transform.Find("gate");
        gateObj = tr.gameObject;
        //プレイヤー
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //発射時間判定
        passedTimes += Time.deltaTime;
        //距離チェック
        if (CheckLength(player.transform.position))
        {
            if (passedTimes > delayTime)
            {
                //発射
                passedTimes = 0;
                //発射位置
                Vector3 pos = new Vector3(gateObj.transform.position.x,gateObj.transform.position.y,transform.position.z);
                //PrefabからGameObjectを作るお
                GameObject obj = Instantiate(objPrefab,pos,Quaternion.identity);
                //発射方向
                Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();
                Vector2 v = new Vector2(fireSpeedX,fireSpeedY);
                rbody.AddForce(v,ForceMode2D.Impulse);
            }
        }
    }
    //距離チェック
    bool CheckLength(Vector2 targetPos)
    {
        bool ret = false;
        float d = Vector2.Distance(transform.position, targetPos);
        if (length >= d)
        {
            ret = true;
        }
        return ret;
    }
}
