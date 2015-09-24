using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardData : MonoBehaviour {

    // 外部データ
    public UISprite imgSprite;    //!< 絵柄スプライト
    public UISprite symbolSprite; //!< 記号スプライト
    public UITexture backImgTex;  //!< 背後画像テクスチャ
    public UISprite CardNum;      //!< 数値表示スプライト

    public List<GameObject> CardNumSymbol = new List<GameObject>();  //!< 数値に応じた記号を表示するためのオブジェクト群

    public string[] GameObjectNameArr = new string[]{   //!< ゲームオブジェクト名配列
        "CardImg",
        "CardSymbol",
        "CardBackGround",
        "CardNumSymbol",
        "CardNum",
        "CardNumber",
    };

    // 内部変数
    private DeckClass.Card myData;  //!< 自身のデータ
    private bool invisible = false; //!< 消すフラグ
    private bool dealer = false;    //!< ディーラーフラグ
    private bool reverse = false;   //!< 裏返しフラグ

    /// <summary>
    /// Awake
    /// </summary>
    void Awake() {
        if (imgSprite == null)
            imgSprite = CommonUtil.SearchObjectChild(GameObjectNameArr[0], this.transform).GetComponent<UISprite>();
        if (symbolSprite == null)
            symbolSprite = CommonUtil.SearchObjectChild(GameObjectNameArr[1], this.transform).GetComponent<UISprite>();
        if (backImgTex == null)
            backImgTex = CommonUtil.SearchObjectChild(GameObjectNameArr[2], this.transform).GetComponent<UITexture>();
        if (CardNumSymbol.Count <= 0) {
            foreach (Transform trans in CommonUtil.SearchObjectChild(GameObjectNameArr[3], this.transform).transform) {
                CardNumSymbol.Add(trans.gameObject);
                trans.gameObject.SetActive(false);
            }
        }
        if (CardNum == null)
            CardNum = CommonUtil.SearchObjectChild(GameObjectNameArr[5], CommonUtil.SearchObjectChild(GameObjectNameArr[4], this.transform).transform).GetComponent<UISprite>();
    }

    /// <summary>
    /// Start
    /// </summary>
    void Start() {
        // カードの状態をセット
        SetCardImg();

        // 消すフラグがあれば停止する
        if (invisible) {
            this.gameObject.SetActive(false);
            invisible = false;
        }
        // ディーラーでないなら点数を加算
        else if (!dealer) {
            IncTotalScore();
        }
        // ディーラーであれば内部スコアとして残す
        else { (BJGame.GetSystem() as BJGame).nowDealerTotalScor += myData.number > 10 ? 10 : myData.number; }
    }

    /// <summary>
    /// 再度起こした場合に呼び出す処理
    /// </summary>
    public IEnumerator SetAwake() {
        if (!dealer) {
            IncTotalScore();
        }
        else {
            (BJGame.GetSystem() as BJGame).nowDealerTotalScor += myData.number > 10 ? 10 : myData.number;
        }
        yield return null;
    }

    /// <summary>
    /// SetData
    /// </summary>
    /// <param name="i_data">カード情報</param>
    public void SetData(DeckClass.Card i_data, bool deal, bool inv, bool rev) {
        // 自身のデータを登録
        myData = i_data;
        
        // 各種フラグ設置
        invisible = inv;
        dealer = deal;

        // 裏にする
        CardReverse(!rev);
    }

    /// <summary>
    /// スコアに数値を加算する
    /// </summary>
    public void IncTotalScore() {
        (BJGame.GetSystem() as BJGame).nowTotalScore += myData.number > 10 ? 10 : myData.number;
        (BJGame.GetSystem() as BJGame).SetScore(myData.numberName == BJGame.AceCardName);
    }

    /// <summary>
    /// カードを裏返す
    /// </summary>
    /// <param name="rev">裏返し判定</param>
    public void CardReverse(bool rev) {
        reverse = !rev;
        CommonUtil.SearchObjectChild(GameObjectNameArr[0], this.transform).SetActive(rev);
        CommonUtil.SearchObjectChild(GameObjectNameArr[1], this.transform).SetActive(rev);
        CommonUtil.SearchObjectChild(GameObjectNameArr[5], CommonUtil.SearchObjectChild(GameObjectNameArr[4], this.transform).transform).SetActive(rev);
        CommonUtil.SearchObjectChild(GameObjectNameArr[2], this.transform).GetComponent<UITexture>().mainTexture = BlackJackSys.cardBackImg[rev ? 1 : 0];
    }

    /// <summary>
    /// カードの状態をセット
    /// </summary>
    public void SetCardImg() {
        // JQKならば絵柄の情報があるので絵柄を切り替え
        if (myData.cardAtlus != null) {
            imgSprite.atlas = myData.cardAtlus;
            imgSprite.spriteName = myData.cardSpriteName;
        }
        else {
            // 絵柄がなければ表示を止める
            imgSprite.gameObject.SetActive(false);
            // ディーラーでないなら表示
            if (!reverse)
                CardNumSymbol[myData.number - 1].SetActive(true);
            foreach (Transform trans in CardNumSymbol[myData.number - 1].transform) {
                trans.gameObject.GetComponent<UISprite>().spriteName = myData.symbolName;
            }
        }
        symbolSprite.spriteName = myData.symbolName;
        CardNum.spriteName = myData.numberName;
    }
}
