using UnityEngine;
using System.Collections;

public class TotalScore : MonoBehaviour {

    // 外部変数
    public GameObject TotalOr;  //!< 「ＯＲ」表示ラベルオブジェクト
    public UILabel TotalLabel;  //!< カードの合計数表示ラベル
    public UILabel TotalALabel; //!< A(11)を含んだカードの合計数表示ラベル

    public string[] TotalLabelObjectNameArr = new string[]{ //!< 合計表示オブジェクト名一覧
        "TotalOr",
        "Total",
        "TotalA",
    };

    // 内部変数

    /// <summary>
    /// Awake
    /// </summary>
	void Start () {
        // オブジェクトがなければ各種探索
        if (TotalOr == null)
            TotalOr = CommonUtil.SearchObjectChild(TotalLabelObjectNameArr[0], this.transform);
        if (TotalLabel == null)
            TotalLabel = CommonUtil.SearchObjectChild(TotalLabelObjectNameArr[1], this.transform).GetComponent<UILabel>();
        if (TotalALabel == null)
            TotalALabel = CommonUtil.SearchObjectChild(TotalLabelObjectNameArr[2], this.transform).GetComponent<UILabel>();

        // A用のオブジェクトを止める
        SetTotalASetActive(false);

        // 自身を設置
        (BJGame.GetSystem() as BJGame).totalScore = this;
	}

    public const int AScore = 10;
    /// <summary>
    /// 合計をセット
    /// </summary>
    /// <param name="score">合計点</param>
    /// <param name="A">エースフラグ</param>
    public void SetScore(int score, bool A) {
        // 合計を表示
        TotalALabel.text = (score + AScore).ToString();
        TotalLabel.text = score.ToString();
        // Aがあって「２１」を超えない範囲ならば発動
        if (A && !TotalALabel.gameObject.activeInHierarchy && (score + AScore) <= BlackJackSys.BLACK_JACK) {
            SetTotalASetActive(true);
        }
        else if(TotalALabel.gameObject.activeInHierarchy && (score + AScore) > BlackJackSys.BLACK_JACK){
            SetTotalASetActive(false);
        }
    }

    /// <summary>
    /// Ａの場合の合計ラベルのアクティブをセット
    /// </summary>
    /// <param name="act"></param>
    public void SetTotalASetActive(bool act) {
        TotalOr.SetActive(act);
        TotalALabel.gameObject.SetActive(act);
    }
}
