using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;


public class BJButton : MonoBehaviour {

    // enum
    public enum ButtonVal
    {
        NONE,
        HIT,
        STAND,
        END,
    }

    // 外部変数
    public ButtonVal m_myVal;   //!< 自身の識別

    // 内部変数
    private Func<GameObject, ButtonVal, bool> m_func;  //!< デリゲート関数

    /// <summary>
    /// Start
    /// </summary>
    void Start() {
        if (m_myVal == ButtonVal.END) {
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// SetData
    /// </summary>
    /// <param name="func">デリゲート関数</param>
    public void SetData(Func<GameObject, ButtonVal, bool> func) {
        m_func = func;
    }

    /// <summary>
    /// ボタンクリック
    /// </summary>
    public void ButtonClick() {
        if (m_func != null)
            m_func(this.gameObject, m_myVal);
    }
}
