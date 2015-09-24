using UnityEngine;
using System.Collections;

public class Result : MonoBehaviour {

    private static Result _result;
    public static Result GetResult() { return _result; }
    private static void SetResult(Result result) { _result = result; }

    public UILabel resultLabel;

    void Awake() {
        SetResult(this);
    }

    public void SetData(string result) {
        resultLabel.text = result;
    }

    public void OnClickEnd() {
        resultLabel.text = "";
    }
}
