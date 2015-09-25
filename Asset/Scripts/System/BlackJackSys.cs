using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackJackSys : CommonSys {

    // 定数
    public const int BLACK_JACK = 21;

    // 外部変数
    public static List<Texture> cardBackImg = new List<Texture>();  //!< カード背景画像一覧

    // 内部変数
    private DeckClass _deck;    //!< 山札

    /// <summary>
    /// Awake
    /// </summary>
    public override void Awake() {
        base.Awake();
        _deck = new DeckClass();
        // 山札の作成
        _deck.Awake();

        // カード背景画像がなければ取ってくる
        if (cardBackImg.Count <= 0) {
            cardBackImg.Add(Resources.Load<Texture>("Textures/BackIll"));
            cardBackImg.Add(Resources.Load<Texture>("Textures/cardBack"));
        }
    }

    /// <summary>
    /// 山札からカードを引く
    /// </summary>
    public DeckClass.Card DrawDeck() {
        return _deck.DrawCard();
    }

    /// <summary>
    /// シーン切り替え
    /// </summary>
    public IEnumerator ChengeScene(){
        Application.LoadLevel(0);
        yield return null;
    }
}
