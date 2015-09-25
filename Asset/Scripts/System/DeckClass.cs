using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class DeckClass{

    /// <summary>
    /// 模様の種類
    /// </summary>
    public enum Symbol
    {
        HEART = 0,
        SPADE,
        CLUB,
        DIA,
    }

    /// <summary>
    /// 模様の名称一覧
    /// </summary>
    private string[] cardsSymbol = new string[]{ 
        "Heart",
        "Spade",
        "Club",
        "Dia",
    };

    /// <summary>
    /// カードの数字名
    /// </summary>
    private string[] cardNumberName = new string[]{
        "NONE",
        "A",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "10",
        "J",
        "Q",
        "K",
    };

    /// <summary>
    /// 初期カードイメージ用ディクショナリ
    /// </summary>
    private Dictionary<string, string> cardInitImgNameDic = new Dictionary<string, string>
    {
        {"J_Spade", "f040"},
        {"Q_Spade", "f058"},
        {"K_Spade", "f222"},
        {"J_Heart", "f039"},
        {"Q_Heart", "f062"},
        {"K_Heart", "f068"},
        {"J_Club" , "f041"},
        {"Q_Club" , "f061"},
        {"K_Club" , "f126"},
        {"J_Dia"  , "f042"},
        {"Q_Dia"  , "f060"},
        {"K_Dia"  , "f174"},
    };

    /// <summary>
    /// 単体のカードクラス
    /// </summary>
    public class Card
    {
        public int number;          //!< 数字
        public string numberName;   //!< 数字の名称
        public Symbol symbol;       //!< 記号
        public string symbolName;   //!< 記号名
        public UIAtlas cardAtlus;   //!< 使用するアトラス
        public string cardSpriteName;//!< 使用する絵柄のスプライト名
        public bool drew;           //!< 引かれたかどうかの判定
        public bool back;           //!< 裏面かどうか

        public Card(int i_num,string i_numNam, Symbol i_sym, string i_symNam, bool i_drew, bool i_back) {
            number      = i_num;
            numberName  = i_numNam;
            symbol      = i_sym;
            symbolName  = i_symNam;
            drew        = i_drew;
            back        = i_back;
        }
    }

    // 定数
    const int DECK_NUM = 53;            //!< デッキの最大数
    const int ONE_SYMBOL_LIMIT = 13;    //!< 一つの記号の最大数

    // 山札
    private Dictionary<int, Card> _deck = new Dictionary<int, Card>();
    public Dictionary<int, Card> Deck {
        get { return _deck; }
    }

    // 絵柄
    private Dictionary<int, UIAtlas> _cardImg = new Dictionary<int, UIAtlas>();
    public Dictionary<int, UIAtlas> CardImg {
        get { return _cardImg; }
    }

    /// <summary>
    /// 山札作成
    /// </summary>
    public virtual void Awake(){
        // 絵柄のアトラスを全て設置
        if (_cardImg.Count <= 0) {
            UIAtlas[] atlus = Resources.LoadAll<UIAtlas>("Atlases");
            foreach (var atl in atlus.Select((value, index) => new { index, value})) {
                _cardImg.Add(atl.index, atl.value);
            }
        }

        int cnt = 1;
        int sym = 0;
        for (int i = 1; i < DECK_NUM; i++) {
            _deck.Add(i, new Card(cnt, cardNumberName[cnt], (Symbol)sym, cardsSymbol[sym], false, false));
            // 絵柄のカードには絵柄の情報を設置
            if (cardNumberName[cnt] == cardNumberName[11] || cardNumberName[cnt] == cardNumberName[12] || cardNumberName[cnt] == cardNumberName[13]) {
                _deck[i].cardAtlus = GetCardImgAtlas(cardNumberName[cnt] + "_" + cardsSymbol[sym]);
                _deck[i].cardSpriteName = cardInitImgNameDic[cardNumberName[cnt] + "_" + cardsSymbol[sym]];
            }
            if (i % ONE_SYMBOL_LIMIT == 0) {
                cnt = 0;
                sym++;
            }
            cnt++;
        }
    }

    /// <summary>
    /// 指定されたスプライトがあるアトラスを取得
    /// </summary>
    /// <param name="key">スプライト名を取得するためのキー</param>
    /// <returns>UIAtlus</returns>
    private UIAtlas GetCardImgAtlas(string key) {
        UIAtlas atl = null;
        string imgName = cardInitImgNameDic[key];
        foreach (var data in CardImg) {
            if(data.Value.GetSprite(imgName) != null){
                atl = data.Value;
                break;
            }
        }
        return atl;
    }

    /// <summary>
    /// カードを引く
    /// </summary>
    /// <returns></returns>
    public Card DrawCard() {
        int cnt = 0;
        int randNum = 0;
        // 一枚も引けない場合は返す
        if (CheckAllowDrawCardNum() == 0) return null;
        while (cnt < DECK_NUM) {
            // 引かれていないカードをランダムで探す
            randNum = CommonUtil.CreateRandomNumber(DECK_NUM);
            if (!Deck[randNum].drew) {
                Deck[randNum].drew = true;
                break;
            }
            cnt++;
        }
        // カード最大でランダム検索したけれど見つからなかった場合は全検索する
        if (cnt >= DECK_NUM) {
            for (cnt = 1; cnt < DECK_NUM; cnt++) {
                if (!Deck[cnt].drew) {
                    Deck[cnt].drew = true;
                    randNum = cnt;
                    break;
                }
            }
        }
        return Deck[randNum];
    }

    /// <summary>
    /// まだ引けるカード枚数を試算
    /// </summary>
    /// <returns></returns>
    public int CheckAllowDrawCardNum() {
        int cnt = 0;
        foreach (var data in Deck) {
            if (!data.Value.drew) {
                cnt++;
            }
        }
        return cnt;
    }
}
