using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UIを使うのに必要

public class GameManager : MonoBehaviour
{
    public GameObject mainImage; //画像を持つGameObject
    public Sprite gameOverSpr; //GAME OVER画像
    public Sprite gameClearSpr; //GAME CLEAR画像
    public GameObject panel; //パネル
    public GameObject restartButton; //RESTARTボタン
    public GameObject nextButton; //NEXTボタン

    Image titleImage; //画像を表示しているImageコンポーネント

    // +++ 時間制限追加 +++
    public GameObject timeBar; //時間表示イメージ
    public GameObject timeText; //時間テキスト
    TimeController timeCnt; //TimeController

    // +++ スコア追加 +++
    public GameObject scoreText; //スコアテキスト
    public static int totalScore; //合計スコア
    public int stageScore = 0; //ステージスコア

    // +++ サウンド追加 +++
    public AudioClip meGameOver; //ゲームオーバー
    public AudioClip meGameClear; // ゲームクリア
    // Start is called before the first frame update
    void Start()
    {
        //画像を非表示にする
        Invoke("InactiveImage",1.0f);
        //ボタン(パネル)を非表示にする
        panel.SetActive(false);

        // +++ 時間制限追加 +++
        //TimeControllerを取得
        timeCnt = GetComponent<TimeController>();
        if(timeCnt != null)
        {
            if (timeCnt.gameTime == 0.0f)
            {
                timeBar.SetActive(false); //制限時間ナシなら隠す
            }
        }

        // +++ スコア追加 +++
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == "gameclear")
        {
            //ゲームクリア
            mainImage.SetActive(true); //画像を表示する
            panel.SetActive(true); //ボタン(パネル)を表示する
            //RESTARTボタンを無効化する
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameClearSpr; //画像を設定する
            PlayerController.gameState = "gameend";

            // +++ 時間制限追加 +++
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true; //時間カウント停止
                // +++ スコア追加 +++
                //整数に代入することで少数を切り捨てる
                int time = (int)timeCnt.displayTime;
                totalScore += time * 10; //残り時間をスコアに加える
            }

            // +++ スコア追加 +++
            totalScore += stageScore;
            stageScore = 0;
            UpdateScore();// スコア更新

            // +++ サウンド追加 +++
            // サウンド再生
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                //BGM停止
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameClear);
            }
        }
        else if (PlayerController.gameState == "gameover")
        {
            //ゲームオーバー
            mainImage.SetActive(true); //画像を表示する
            panel.SetActive(true); //ボタン(パネル)を表示する
            //Nextボタンを無効化する
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr; //画像を設定する
            PlayerController.gameState = "gameend";

            // +++ 時間制限追加 +++
            if(timeCnt != null)
            {
                timeCnt.isTimeOver = true; //時間カウント停止
            }

            // +++ サウンド再生追加 +++
            // サウンド再生
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                //BGM停止
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameOver);
            }
        }
        else if (PlayerController.gameState == "playing")
        {
            //ゲーム中
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            //PlayerControllerを取得する
            PlayerController playerCnt = player.GetComponent<PlayerController>();
            // +++ 時間制限追加 +++
            //タイムを更新する
            if (timeCnt != null)
            {
                if (timeCnt.gameTime > 0.0f)
                {
                    //整数に代入することで小数を切り捨てる
                    int time = (int)timeCnt.displayTime;
                    //タイム更新
                    timeText.GetComponent<Text>().text = time.ToString();
                    //タイムオーバー
                    if (time == 0)
                    {
                        playerCnt.GameOver(); //ゲームオーバーにする
                    }
                }
            }

            // +++ スコア追加 +++
            if(playerCnt.score != 0)
            {
                stageScore += playerCnt.score;
                playerCnt.score = 0;
                UpdateScore();
            }
        }
    }
    //画像を非表示にする    
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    // +++ スコア追加 +++
    void UpdateScore()
    {
        int score = stageScore + totalScore;
        scoreText.GetComponent<Text>().text = score.ToString();
    }
}
