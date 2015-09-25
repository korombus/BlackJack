using UnityEngine;
using System.Linq;
using System.Collections;

public class BJGame : BlackJackSys {

    // 定数
    public const string AceCardName = "A";  //!< エースカード名

    // 外部変数
    public GameObject[] playerCards;    //!< プレイヤーのカードオブジェクト一覧
    public GameObject[] dealerCards;    //!< ディーラーのカードオブジェクト一覧
    public GameObject[] button;         //!< ボタンオブジェクト一覧

    public int nowTotalScore = 0;           //!< 現在のトランプの合計点
    public int nowDealerTotalScor = 0;      //!< ディーラーのドランプの合計点
    public TotalScore totalScore = null;    //!< 合計表示クラス

    // 内部変数
    private bool stand = false;     //!< プレイヤースタンドフラグ
    private bool battle = false;    //!< 決戦フラグ

    public override void Awake() {
        base.Awake();
        StartCoroutine(Create());
    }

    /// <summary>
    /// Create
    /// </summary>
    IEnumerator Create() {
        // プレイヤーのカードを配置
        StartCoroutine(InitSetCard(playerCards, false));
        // ディーラーのカードを配置
        StartCoroutine(InitSetCard(dealerCards, true));
        // ボタンに関数を設置
        foreach (GameObject obj in button) {
            obj.GetComponent<BJButton>().SetData(OnButton);
        }
        yield return null;
    }

    /// <summary>
    /// 初期カードをセット
    /// </summary>
    /// <param name="cards">カードのオブジェクト配列</param>
    IEnumerator InitSetCard(GameObject[] cards, bool dealer) {
        bool reverse = dealer;
        foreach (var obj in cards.Select((value, index) => new { index, value })) {
            DeckClass.Card card = DrawDeck();
            obj.value.GetComponent<CardData>().SetData(card, dealer, obj.index > 1, reverse);
            reverse = false;
        }
        yield return null;
    }

    /// <summary>
    /// 合計を表示
    /// </summary>
    /// <param name="A">エースフラグ</param>
    public void SetScore(bool A) {
        totalScore.SetScore(nowTotalScore, A);
    }

    const int MIN_DEALER_SCORE = 16;    //!< ディーラーの点数下限
    /// <summary>
    /// Update
    /// </summary>
    void Update() {
        // ディーラー処理
        if (stand) {
            // 16以上になるまで引き続ける
            if (nowDealerTotalScor <= MIN_DEALER_SCORE) {
                foreach (GameObject obj in dealerCards) {
                    // Aがあれば確認する
                    bool dealerA = CheckCardsInA(dealerCards);
                    if (dealerA && (nowDealerTotalScor + TotalScore.AScore) > MIN_DEALER_SCORE && (nowDealerTotalScor + TotalScore.AScore) <= BLACK_JACK) {
                        nowDealerTotalScor += TotalScore.AScore;
                        break;
                    }
                    if (!obj.activeInHierarchy) {
                        obj.SetActive(true);
                        StartCoroutine(obj.GetComponent<CardData>().SetAwake());
                        // 引き終わったら停止
                        if (nowDealerTotalScor > MIN_DEALER_SCORE) {
                            break;
                        }
                    }
                }
            }
            // 全部終了したら勝敗判定
            if (stand && !battle) {
                Debug.Log(nowDealerTotalScor);
                SetDealerEndState();
            }
        }

        // 勝敗判定
        if (battle) {
            if (nowTotalScore <= BLACK_JACK && (nowTotalScore > nowDealerTotalScor || nowDealerTotalScor > BLACK_JACK)) {
                Result.GetResult().SetData("Win!");
            }
            else {
                Result.GetResult().SetData("Lose");
            }
            // 判定が出たら決戦フラグを折る
            battle = false;
            // カードを表にする
            dealerCards[0].GetComponent<CardData>().CardReverse(true);
            dealerCards[0].GetComponent<CardData>().SetCardImg();
            // EndButton出現
            button[2].SetActive(true);
        }
    }

    /// <summary>
    /// ディーラーの終了ステータス
    /// </summary>
    private void SetDealerEndState() {
        stand = false;
        battle = true;
    }

    /// <summary>
    /// ボタン処理
    /// </summary>
    /// <param name="obj">ボタンオブジェクト</param>
    /// <param name="buttonVal">ボタン種別</param>
    /// <returns>bool</returns>
    public bool OnButton(GameObject obj, BJButton.ButtonVal buttonVal) {
        switch (buttonVal) {
            case BJButton.ButtonVal.HIT:
                // 表示していないカードを表示
                foreach (GameObject card in playerCards) {
                    if (!card.activeInHierarchy) {
                        card.SetActive(true);
                        // 加算処理
                        StartCoroutine(card.GetComponent<CardData>().SetAwake());
                        // 21以上になっていたらボタンを消す
                        if (nowTotalScore >= BLACK_JACK) {
                            obj.SetActive(false);
                        }
                        break;
                    }
                }
                break;
            case BJButton.ButtonVal.STAND:
                // プレイヤーの手札にＡがあるか調べる
                bool playerA = CheckCardsInA(playerCards);
                // 手札にＡがあって、10にしたときに21以内であればそちらの数値を採用
                if (playerA && (nowTotalScore + TotalScore.AScore) <= BLACK_JACK) {
                    nowTotalScore += TotalScore.AScore;
                }
                // スタンドしたらディーラーの処理を走らせる
                stand = true;
                foreach (GameObject buttons in button) {
                    buttons.SetActive(false);
                }
                break;
            case BJButton.ButtonVal.END:
                Result.GetResult().OnClickEnd();
                // シーン読みなおし
                StartCoroutine(ChengeScene());
                break;
        }
        return true;
    }

    /// <summary>
    /// 手札内にＡがあるか確認
    /// </summary>
    /// <param name="objs">手札オブジェクト一覧</param>
    /// <returns>bool</returns>
    private bool CheckCardsInA(GameObject[] objs) {
        foreach (GameObject obj in objs) {
            if (obj.activeInHierarchy && obj.GetComponent<CardData>().CardNum.spriteName == AceCardName) {
                return true;
            }
        }
        return false;
    }
}
