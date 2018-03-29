using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Physics2D : MonoBehaviour
{
    // コードを書き終わったらグラビティスケールを1にすること！
    // ジャンプ距離が長くて回転するのは仕様

    // 宣言
    // Inspectorで調整するためのプロパティ
    public float speed = 12f;      // プレイヤーキャラの速度
    public float jumpPower = 500f; // プレイヤーをジャンプさせるときのパワー
    public Sprite[] run;           // プレイヤーキャラの走りスプライト
    public Sprite[] jump;          // プレイヤーキャラのジャンプスプライト

    // 内部で使う変数
    int animIndex;  // プレイヤーキャラのアニメ再生インデックス
    bool grounded;  // 接地チェック
    bool goalCheck; // ゴールチェック
    float goalTime; // ゴールタイム

    // メッソージに対応したコード（ゲームエンジンから呼ばれるコード）
    // コンポーネントの実行開始
    // Use this for initialization
    void Start()
    {
        // 初期化
        animIndex = 0;
        grounded = false;
        goalCheck = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ゴールチェック
        if (collision.gameObject.name == "Stage_Gate")
        {
            // ゴール！！！
            goalCheck = true;
            goalTime = Time.timeSinceLevelLoad;
        }
    }

    // フレームの置き換え
    // Update is called once per frame
    void Update()
    {
        // 地面チェック
        Transform groundCheck = transform.Find("GroundCheck");
        grounded = (Physics2D.OverlapPoint(groundCheck.position) != null) ? true : false;

        if (grounded)
        {
            // ジャンプ
            if (Input.GetButtonDown("Fire1"))
            {
                // ジャンプ処理
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpPower));
                // ジャンプスプライト画像に切り替え
                GetComponent<SpriteRenderer>().sprite = jump[0];
            }
            else
            {
                // 走り処理
                animIndex++;
                if (animIndex >= run.Length)
                {
                    animIndex = 0;
                }
                // 走りスプライト画像に切り替え
                GetComponent<SpriteRenderer>().sprite = run[animIndex];
            }
        }

        if (transform.position.y < -10f)
        {
            // 穴に落ちたらステージを再読込してリセット
            SceneManager.LoadScene("StageB");
        }
    }

    private void FixedUpdate()
    {
        // 移動計算
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
        // カメラ移動
        GameObject goCam = GameObject.Find("MainCamera");
        goCam.transform.position = new Vector3(transform.position.x + 5f, goCam.transform.position.y, goCam.transform.position.z);
    }

    // unityGUIの表示
    private void OnGUI()
    {
        // デバッグテキスト
        GUI.TextField(new Rect(10, 10, 300, 60), "[Unity 2D Sample 3-1 B1,2]\nマウスの左ボタンを押すとジャンプ");
        if (goalCheck)
        {
            GUI.TextField(new Rect(10, 100, 330, 60), string.Format("***** Goal! *****\nTime {0}", goalTime));
        }

        if (GUI.Button(new Rect(10, 80, 100, 20), "リセット"))
        {
            SceneManager.LoadScene("StageB");
        }
    }
}
