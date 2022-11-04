using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //インスタンス定義
    public static PlayerController instance = null;

    Rigidbody2D rbody; //Rigidbody2D型の変数
    float axisH = 0.0f; //入力
    public float speed = 3.0f; //移動速度

    public float jump = 9.0f; //ジャンプ力
    public LayerMask groundLayer; //着地できるレイヤー
    bool goJump = false; //ジャンプ開始フラグ
    bool onGround = false; //地面に立っているフラグ

    //アニメーション対応
    Animator animator; //アニメーター
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";

    public static string gameState = "playing"; //GAMEの状態

    public int score = 0; //スコア
    public int extendscore = 0; //Extend用スコア

    //ダウンフラグ
    public bool isDown = false;

    //点滅追加
    private bool isContinue = false; 
    private float continueTime = 0.0f;
    private float blinkTime = 0.0f; 
    private SpriteRenderer sr = null;

    //残機フラグ
        //public bool isUpdateHeart = false;

    //残機取得
    //[Header("現在の残機")] public int heartNum;
    //[Header("デフォルトの残機")] public int defaultHeartNum;

    public AudioClip meGet; // アイテム取得

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody2Dを取得する
        rbody = this.GetComponent<Rigidbody2D>();
        //Animatorを取得する
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;
        gameState = "playing"; //ゲーム中にする

        //インスタンス確保
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != "playing")
        {
            return;
        }

        //水平方向の入力を確認する
        axisH = Input.GetAxisRaw("Horizontal");
        //axisH = Input.GetAxisRaw("D_Pad_H");
        //向きの調整
        if (axisH > 0.0f)
        {
            //右移動
            Debug.Log("右移動");
            transform.localScale = new Vector2(1,1);
        }
        else if (axisH < 0.0f)
        {
            //左移動
            Debug.Log("左移動");
            transform.localScale = new Vector2(-1,1); // 左右反転させる
        }
        //キャラクターをジャンプさせる
        if (Input.GetButtonDown("Jump"))
        {
            Jump(); //ジャンプ
        }

        //コンティニュー処理
        if (isContinue)
        {
            //明滅　ついている時に戻る
            if(blinkTime > 0.2f)
            {
                sr.enabled = true;
                blinkTime = 0.0f;
            }
            //明滅　消えているとき
            else if (blinkTime > 0.1f)
            {
                sr.enabled = false;
            }
            //明滅　ついているとき
            else
            {
                sr.enabled = true;
            }

            //1秒たったら明滅終わり
            if(continueTime > 1.0f)
            {
                isContinue = false;
                blinkTime = 0f;
                continueTime = 0f;
                sr.enabled = true;
            }
            else
            {
                blinkTime += Time.deltaTime;
                continueTime += Time.deltaTime;
            }
        }

    }

    void FixedUpdate()
    {
        if (gameState != "playing")
        {
            return;
        }
        //地上判定
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundLayer);

        if (onGround || axisH != 0)
        {
            //地面の上or速度が0ではない
            //速度を変更する
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y);   
        }
        if (onGround && goJump)
        {
            //地面の上でジャンプキーが押された
            //ジャンプさせる
            Debug.Log("ジャンプ");
            Vector2 jumpPw = new Vector2(0,jump); //ジャンプさせるベクトルを作る
            rbody.AddForce(jumpPw,ForceMode2D.Impulse); //瞬間的な力を加える
            goJump = false; //ジャンプフラグおろす
        }

        if (onGround)
        {
            //地面の上
            if (axisH == 0)
            {
                nowAnime = stopAnime; //停止中
            }
            else
            {
                nowAnime = moveAnime; //移動
            }
        }
        else
        {
            //空中
            nowAnime = jumpAnime;
        }

        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime); //アニメーション再生
        }

        if (isDown)
        {
            nowAnime = deadAnime;
            oldAnime = nowAnime;
        }

    }
    //ジャンプ
    public void Jump()
    {
        goJump = true; //ジャンプフラグをたてる
        Debug.Log("ジャンプボタン押し");
    }

    //接触開始
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal(); //ゴール
        }
        else if (collision.gameObject.tag == "Dead")//残機ある状態でdeadに触れる
        {
            GameObject canvas = GameObject.Find("Canvas");
            //PlayerControllerを取得する
            GameManager Gmana = canvas.GetComponent<GameManager>();
            Gmana.SubHeartNum();
        }
        else if (collision.gameObject.tag == "ScoreItem")
        {
            //スコアアイテム
            //ItemDataを得る
            ItemData item = collision.gameObject.GetComponent<ItemData>();
            //スコアを得る
            score = item.value;
            extendscore += score;

            AudioSource soundPlayer = GetComponent<AudioSource>();
            soundPlayer.PlayOneShot(meGet);
            //Extend処理
            //if(extendscore >= 10000)
            //{
                //AddHeartNum();
                //extendscore = 0;
                //isUpdateHeart = true;
            //}

            //アイテムを削除する
            Destroy(collision.gameObject);
        }
    }
    //ゴール
    public void Goal()
    {
        
        GameObject goal = GameObject.Find("Goal");
        //PlayerControllerを取得する
        GoalSprite GoSp = goal.GetComponent<GoalSprite>();
        
        animator.Play(goalAnime);
        GoSp.Goaled();
        gameState = "gameclear";
        GameStop(); //GAME停止
    }
    //ゲームオーバー
    public void GameOver()
    {
        animator.Play(deadAnime);

        gameState = "gameover";
        GameStop(); //GAME停止
        //===================
        //ゲームオーバー演出
        //===================
        //プレイヤー当たり判定を消す
        GetComponent<BoxCollider2D>().enabled = false;
        //プレイヤーを上に少し跳ね上げる演出
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }
    //GAME停止
    void GameStop()
    {
        //Rigidbody2Dを取ってくる
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        //速度を0にして強制停止
        rbody.velocity = new Vector2(0, 0);
    }

    //ディレイ用コルーチン
    public IEnumerator time() 
    {
        //終わるまで待ってほしい処理を書く
        //例：敵が倒れるアニメーションを開始
        //2秒待つ
        yield return new WaitForSeconds(1.0f);
        //再開してから実行したい処理を書く
        //例：敵オブジェクトを破壊
        isDown = true;
    } 

   
    /// コンティニュー待機状態か
    public bool IsContinueWaiting()
    {
        return IsDownAnimEnd();
    }

    //ダウンアニメーションが完了しているかどうか**未実装
    private bool IsDownAnimEnd()
    {
        if(isDown)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// コンティニューする
    /// </summary>
    public void ContinuePlayer()
    {
        isDown = false;
        nowAnime = stopAnime;
        GetComponent<BoxCollider2D>().enabled = true;
        isContinue = true;
        gameState = "playing";
    }


     //残機関連
    /// 残機を１つ増やす
    //public void AddHeartNum()
    //{
        //if(heartNum < 99)
        //{
            //++heartNum;
            //isUpdateHeart = true;
        //}
    //}
     
    /// 残機を１つ減らす
    //public void SubHeartNum()
    //{
        //if(heartNum > 0)
        //{
            //--heartNum;
            //isUpdateHeart = true;
            //GameStop();
            //プレイヤー当たり判定を消す
            //GetComponent<CapsuleCollider2D>().enabled = false;
            //プレイヤーを上に少し跳ね上げる演出
            //rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            //animator.Play(deadAnime);
            //gameState = "down";
            //StartCoroutine(time());
        //}
        //else
        //{
            //GameOver(); //ゲームオーバーにする
        //}
    
        
    //}

    //damage処理
    public void Damage(){
        GameStop();
        //プレイヤー当たり判定を消す
        GetComponent<BoxCollider2D>().enabled = false;
        //プレイヤーを上に少し跳ね上げる演出
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        animator.Play(deadAnime);
        gameState = "down";
        StartCoroutine(time());
    }
}


