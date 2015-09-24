using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommonSound : MonoBehaviour {

    // 定数
    public const string BGMPath    = "Sounds/BGM/";
    public const string SEPath     = "Sounds/SE/";

    void Awake() {
        if (this.gameObject.GetComponent<AudioSource>() == null) {
            this.gameObject.AddComponent<AudioSource>();
        }
    }


    /*******************************************************/
    /* !@brief  : BGM再生
     *  @param[in]  : clip      -> 流したい音楽データ
     *  @retval : なし
     *  @date   : 2014/03/12
     *  @author : コロソブス(korombus)
     *******************************************************/
    public void Play(AudioClip clip = null) {
        if (this.audio == null) {
            Debug.Log("NO AUDIO, Please confirm whether the GameObject with this script exists in this Scene");
            return;
        }

        // AudioClipがあればそれを流す
        if (clip != null) {
            DesignateMusicPlay(clip);
        }

        // 音源データがある場合のみ再生
        if (this.audio.clip != null) {
            this.audio.Play();
        } else {
            Debug.Log("NO Audio data");
        }
    }

    /*******************************************************/
    /* !@brief  : BGM再生
     *  @param[in]  : clipName  -> 流したい音楽データの名前
     *  @retval : なし
     *  @date   : 2014/03/12
     *  @author : コロソブス(korombus)
     *******************************************************/
    public void Play(string clipName, bool playSE = false) {
        // 音源名のみ来た場合検索してから流す
        if (clipName != null) {

            AudioClip soundData;
            if (playSE) {
                soundData = Resources.Load(SEPath + clipName) as AudioClip;
            } else {
                soundData = Resources.Load(BGMPath + clipName) as AudioClip;
            }

            if (soundData != null) {
                DesignateMusicPlay(soundData);
            } else {
                Debug.Log("No Audio, Please confirm whether AudioData exists in Assets/Sounds/BGM");
            }
        } else {
            Debug.Log("No Audio Name");
        }
    }
    /*******************************************************/
    /* !@brief  : 指定音源再生
     *  @param[in]  : clip      -> 流したい音楽データ
     *  @retval : なし
     *  @date   : 2014/03/12
     *  @author : コロソブス(korombus)
     *******************************************************/
    private void DesignateMusicPlay(AudioClip clip) {
        this.audio.clip = clip;
        this.audio.Play();
    }

    /*******************************************************/
    /* !@brief  : 音量調整
     *  @param[in]  : value      -> 音量
     *  @retval : なし
     *  @date   : 2014/03/12
     *  @author : コロソブス(korombus)
     *******************************************************/
    public void ChangeVolume(float value) {
        if (value > 1.0f || value < 0) {
            value = 1.0f;
        }
        this.audio.volume = value;
    }

    /*******************************************************/
    /* !@brief  : 音楽停止
     *  @param[in]  : なし
     *  @retval : なし
     *  @date   : 2014/03/13
     *  @author : コロソブス(korombus)
     *******************************************************/
    public void Stop() {
        if (this.audio.isPlaying) {
            this.audio.Stop();
        }
    }

    /*******************************************************/
    /* !@brief  : 音楽が流れているかチェック
     *  @param[in]  : なし
     *  @retval : なし
     *  @date   : 2014/03/18
     *  @author : コロソブス(korombus)
     *******************************************************/
    public bool CheckPlayingAudio() {
        return this.audio.isPlaying;
    }
}
