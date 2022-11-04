using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //シーンの切り替えに必要

public class ChangeScene : MonoBehaviour
{
    [Header("フェード")] public FadeController fade;
    
    [Header("現在の残機")] public int heartNum;

    public static int hoge = 0;
    public string sceneName; //読み込むシーン名

    // Start is called before the first frame update
    void Start()
    {

        hoge = heartNum;

    }
        
    

    // Update is called once per frame
    void Update()
    {
        if(fade.IsFadeOutComplete())
        {
            Load();
            fade.FlagFalse();
        }
    }

    //フェードさせる
    public void Fade()
    {
        fade.StartFadeOut();
    }

    //ロードする
    public void Load()
    {
        Debug.Log("call");
        SceneManager.LoadScene(sceneName);
    }

    //Resetする
    public void Reset()
    {
        GameManager.defaultHeartNum = hoge;
        GameManager.totalScore = 0;
        fade.StartFadeOut();
    }
}
    
