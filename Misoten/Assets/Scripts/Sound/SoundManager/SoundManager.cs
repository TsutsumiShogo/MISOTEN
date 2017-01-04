using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//
// サウンド管理クラス
// 
// ゲームつくろー！とかを参考（ほぼコピった）
// URL : http://qiita.com/2dgames_jp/items/20360f9797c7e8b166bc
//
// サウンドファイルは"Resouces/Sounds"の中にいれる
// スクリプトで "LoadBgm" もしくは "LoadSe" を呼ぶ（１度のみ
// 引数はキーとリソース名です。
//     例) SoundManager.LoadBgm("yuki", "test.mp3");
//         SoundManager.LoadSe("yuki", "test.mp3");
//     ※test.mp3を上記のフォルダに入れたとします。
// 
// これで読み込み終了です。
// 読み込みが終了したら "PlayBgm" もしくは "PlaySe"を使用し、音を鳴らすことができます。
// 
//     例) SoundManager.PlayBgm("yuki");
//         SoundManager.PlaySe("yuki");
//
// SEも同等の書き方で音を鳴らすことができますが、SEにはチャンネルというものもついているので
// 少し違う使い方も可能です。
//
//     例) SoundManager.PlaySe("yuki", 0);
//         SoundManager.PlaySe("yuki", 1);
//
// 0や1チャンネルで音を鳴らすことができます。
// これで何ができるかといいますと、
// 例えばチャンネルを指定せず音を同時に鳴らした場合、音が途切れたり、ノイズに
// なってしまう（らしいです。）
// そこでチャンネル指定を行い、別々のチャンネルで鳴らすことで
// 綺麗な音（？）を鳴らすことができます。
//
// この辺は上のURLにも載っています。
// デフォルトではチャンネルは４つです。
// 基本チャンネルは使用しなくても大丈夫だと思っています。
// 
// @creater : N.Suenaga
// @sicne   : 2016/05/11 17:35
// 
public class SoundManager
{
    // サウンドデータ
    class Data
    {
        // アクセスキー
        public string key;
        public string resName;
        public AudioClip clip;

        // コンストラクタ
        public Data(string _key, string _res)
        {
            key = _key;
            resName = "Sound/" + _res;
            // オーディオクリップを取得
            clip = Resources.Load(resName) as AudioClip;
        }
    }

    // チャンネル数
    const uint SE_CHANEL = 4;
    public const float FIRST_VOLUME = 0.6f;

    // サウンドタイプ
    public enum SoundType
    {
        BGM,
        BGM2,
        SE,
    }

    // シングルトン
    static SoundManager mSingleton = null;

    // インスタンス取得
    public static SoundManager GetInstance()
    {
        return mSingleton ?? (mSingleton = new SoundManager());
    }

    // サウンド再生用のゲームオブジェクト
    GameObject mSoundObject = null;
    // サウンドリソース
    AudioSource mSourceBgm = null;
    AudioSource mSourceBgm2 = null;
    AudioSource mSourceSe = null;
    AudioSource[] mSourceSeArray;
    // BGMにアクセスするためのテーブル
    Dictionary<string, Data> mPoolBgm = new Dictionary<string, Data>();
    // SEにアクセスするためのテーブル
    Dictionary<string, Data> mPoolSe = new Dictionary<string, Data>();

    // コンストラクタ
    public SoundManager()
    {
        // チャンネル
        mSourceSeArray = new AudioSource[SE_CHANEL];
    }

    // オーディオソースを取得
    public AudioSource GetAudioSource(SoundType _type, int channel = -1)
    {
        if (mSoundObject == null)
        {
            // ゲームオブジェクト生成
            mSoundObject = new GameObject("SoundManager");
            // 削除不可
            GameObject.DontDestroyOnLoad(mSoundObject);
            // AudioSource Create
            mSourceBgm = mSoundObject.AddComponent<AudioSource>();
            mSourceBgm2 = mSoundObject.AddComponent<AudioSource>();
            mSourceSe = mSoundObject.AddComponent<AudioSource>();
            for (int i = 0; i < SE_CHANEL; ++i)
            {
                mSourceSeArray[i] = mSoundObject.AddComponent<AudioSource>();
            }
        }

        // BGMのオーディオソースを返す
        if (_type == SoundType.BGM)
        {
            // BGMを返却
            return mSourceBgm;
        }
        else if (_type == SoundType.BGM2)
        {
            return mSourceBgm2;
        }
        else
        {
            // SE
            if (0 <= channel && channel < SE_CHANEL)
            {
                // チャンネル指定
                return mSourceSeArray[channel];
            }

            // デフォルト
            return mSourceSe;
        }
    }

    //
    // BGMを読み込み
    //
    // ただ毎回GetInstance（）を打つのが面倒なためstaticで呼び出し可能に
    public static void LoadBgm(string _key, string _resName)
    {
        GetInstance()._LoadBgm(_key, _resName);
    }

    //
    // SEを読み込み
    //
    public static void LoadSe(string _key, string _resName)
    {
        GetInstance()._LoadSe(_key, _resName);
    }

    //
    // BGMを読み込み
    //
    void _LoadBgm(string _key, string _resName)
    {
        if (mPoolBgm.ContainsKey(_key))
        {
            // データがすでにあるので消す
            mPoolBgm.Remove(_key);
        }
        // データを追加
        mPoolBgm.Add(_key, new Data(_key, _resName));
    }

    //
    // SEを読み込み
    //
    void _LoadSe(string _key, string _resName)
    {
        if (mPoolSe.ContainsKey(_key))
        {
            // データがすでにあるので消す
            mPoolSe.Remove(_key);
        }
        mPoolSe.Add(_key, new Data(_key, _resName));
    }

    //
    // BGMを再生
    //
    public static void PlayBgm(string _key)
    {
        GetInstance()._PlayBgm(_key);
    }

    //
    // BGMを再生
    //
    public static void PlayBgm2(string _key)
    {
        GetInstance()._PlayBgm2(_key);
    }

    //
    // SEを再生
    //
    public static void PlaySe(string _key)
    {
        GetInstance()._PlaySe(_key);
    }

    //
    // BGMを再生
    //
    void _PlayBgm(string _key)
    {
        if (!mPoolBgm.ContainsKey(_key))
        {
            
            return;
        }

        // 今なっているBGMストップ
        _StopBgm();

        // リソースを取得
        var data = mPoolBgm[_key];
        var source = GetAudioSource(SoundType.BGM);
        source.loop = true;
        source.volume = FIRST_VOLUME;
        source.clip = data.clip;
        source.Play();
    }

    //
    // BGMを再生
    //
    void _PlayBgm2(string _key)
    {
        if (!mPoolBgm.ContainsKey(_key))
        {
            
            return;
        }

        // 今なっているBGMストップ
        _StopBgm2();

        // リソースを取得
        var data = mPoolBgm[_key];
        var source = GetAudioSource(SoundType.BGM2);
        source.loop = true;
        source.volume = FIRST_VOLUME;
        source.clip = data.clip;
        source.Play();
    }

    //
    // BGMを停止
    //
    public static void StopBgm()
    {
        GetInstance()._StopBgm();
    }

    //
    // BGMを停止
    //
    public static void StopBgm2()
    {
        GetInstance()._StopBgm2();
    }



    //
    // BGMを停止
    //
    void _StopBgm()
    {
        // 今なっているオーディオソースを取得
        GetAudioSource(SoundType.BGM).Stop();
    }

    //
    // BGMを停止
    //
    void _StopBgm2()
    {
        // 今なっているオーディオソースを取得
        GetAudioSource(SoundType.BGM2).Stop();
    }

    //
    // SEを再生
    //
    public static void PlaySe(string _key, int _channel = -1)
    {
        GetInstance()._PlaySe(_key, _channel);
    }

    //
    // SEを再生
    //
    void _PlaySe(string _key, int _channel = -1)
    {
        if (!mPoolSe.ContainsKey(_key))
        {
            
            return;
        }

        var data = mPoolSe[_key];

        if (0 <= _channel && _channel < SE_CHANEL)
        {
            var source = GetAudioSource(SoundType.SE, _channel);
            source.clip = data.clip;
            source.volume = FIRST_VOLUME;
            source.Play();
            return;
        }
        else
        {
            var source = GetAudioSource(SoundType.SE);
            source.volume = FIRST_VOLUME;
            source.PlayOneShot(data.clip);
        }
    }

    // SEをループ再生
    void _PlaySeLoop(string _key, int _channel = -1)
    {
        if (!mPoolSe.ContainsKey(_key))
        {

            return;
        }

        var data = mPoolSe[_key];

        if (0 <= _channel && _channel < SE_CHANEL)
        {
            var source = GetAudioSource(SoundType.SE, _channel);
            source.clip = data.clip;
            source.volume = FIRST_VOLUME;
            if (!source.isPlaying){
                source.Play();
            }
            return;
        }
        else
        {
            var source = GetAudioSource(SoundType.SE);
            source.volume = FIRST_VOLUME;
            source.PlayOneShot(data.clip);
        }
    }

    //
    // 音の大きさを変更
    //
    public void ChangeVolume(SoundType _type, float _volume, int _channel = -1)
    {
        switch (_type)
        {
            case SoundType.BGM:
            case SoundType.BGM2:
                {
                    var source = GetAudioSource(_type);
                    source.volume = _volume;
                }
                break;
            case SoundType.SE:
                if (0 <= _channel && _channel < SE_CHANEL)
                {
                    var source = GetAudioSource(_type, _channel);
                    source.volume = _volume;
                }
                else
                {
                    var source = GetAudioSource(_type);
                    source.volume = _volume;
                }
                break;
        }
    }

    //
    // 音のピッチを変更
    //
    public void ChangePitch(SoundType _type, float _pitch, int _channel = -1)
    {
        switch (_type)
        {
            case SoundType.BGM:
            case SoundType.BGM2:
                {
                    var source = GetAudioSource(_type);
                    source.pitch = _pitch;
                }
                break;
            case SoundType.SE:
                if (0 <= _channel && _channel < SE_CHANEL)
                {
                    var source = GetAudioSource(_type, _channel);
                    source.pitch = _pitch;
                }
                else
                {
                    var source = GetAudioSource(_type);
                    source.pitch = _pitch;
                }
                break;
        }
    }
}
