using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class DataSys {

    const string SAVE_KEY_OPTION_BGM_VOLUME = "save_key_option_bgm_volume"; //!< bgm音量のキー
    const string SAVE_KEY_OPTION_SE_VOLUME = "save_key_option_se_volume";   //!< se音量のキー

    private static float _bgmVolume;    //!< BGM音量
    private static float _seVolume;     //!< SE音量

    /// <summary>
    /// ロード
    /// </summary>
    protected static void Load() {
        
    }

    /// <summary>
    /// セーブ
    /// </summary>
    protected static void Save() {
        
    }

    /// <summary>
    /// オプション情報をロード
    /// </summary>
    protected static void LoadOptionData() {
        OPTION.SetVolume(PlayerPrefs.GetFloat(SAVE_KEY_OPTION_BGM_VOLUME), OPTION.Sound.BGM);
        OPTION.SetVolume(PlayerPrefs.GetFloat(SAVE_KEY_OPTION_SE_VOLUME), OPTION.Sound.SE);
    }

    /// <summary>
    /// オプション情報をセーブ
    /// </summary>
    protected static void SaveOptionData() {
        PlayerPrefs.SetFloat(SAVE_KEY_OPTION_BGM_VOLUME, OPTION.BGMVolume);
        PlayerPrefs.SetFloat(SAVE_KEY_OPTION_SE_VOLUME, OPTION.SEVolume);
    }
}
