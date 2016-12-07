using UnityEngine;
using System.Collections;



// サウンドデータ
[System.Serializable]
public class SoundData
{
    public string key;
    public AudioClip clip;
}

[System.Serializable]
public class SoundLoader : MonoBehaviour
{
    // BGMデータ
    public SoundData bgmData;
    // BGMデータ
    public SoundData bgmData2;
    // SEデータ
    public SoundData[] seData;
    // 開始時から鳴らす
    public bool g_StartPlay = false;
    // 開始時から鳴らす
    public bool g_StartPlay2 = true;
    void Start()
    {
        SoundManager.StopBgm();
        SoundManager.StopBgm2();

        // BGM読み込み
        SoundManager.LoadBgm(bgmData.key, bgmData.clip.name);

        // BGM読み込み
        if (bgmData2.key != "")
        {
            SoundManager.LoadBgm(bgmData2.key, bgmData2.clip.name);
        }

        // BGMを最初に鳴らす
        if (g_StartPlay)
        {
            SoundManager.PlayBgm(bgmData.key);
        }

        if (bgmData2.key != "" && g_StartPlay2)
        {
            SoundManager.PlayBgm2(bgmData2.key);
        }

        // SE読み込み
        for (int i = 0; i < seData.Length; ++i)
        {
            SoundManager.LoadSe(seData[i].key, seData[i].clip.name);
        }

        SoundManager.GetInstance().ChangePitch(SoundManager.SoundType.BGM, 1.0f);
        SoundManager.GetInstance().ChangePitch(SoundManager.SoundType.BGM2, 1.0f);
        SoundManager.GetInstance().ChangePitch(SoundManager.SoundType.SE, 1.0f, -1);
        SoundManager.GetInstance().ChangePitch(SoundManager.SoundType.SE, 1.0f, 0);
        SoundManager.GetInstance().ChangePitch(SoundManager.SoundType.SE, 1.0f, 1);
        SoundManager.GetInstance().ChangePitch(SoundManager.SoundType.SE, 1.0f, 2);
        SoundManager.GetInstance().ChangePitch(SoundManager.SoundType.SE, 1.0f, 3);

        SoundManager.GetInstance().ChangeVolume(SoundManager.SoundType.BGM, SoundManager.FIRST_VOLUME);
        SoundManager.GetInstance().ChangeVolume(SoundManager.SoundType.BGM2, SoundManager.FIRST_VOLUME);
        SoundManager.GetInstance().ChangeVolume(SoundManager.SoundType.SE, SoundManager.FIRST_VOLUME, -1);
        SoundManager.GetInstance().ChangeVolume(SoundManager.SoundType.SE, SoundManager.FIRST_VOLUME, 0);
        SoundManager.GetInstance().ChangeVolume(SoundManager.SoundType.SE, SoundManager.FIRST_VOLUME, 1);
        SoundManager.GetInstance().ChangeVolume(SoundManager.SoundType.SE, SoundManager.FIRST_VOLUME, 2);
        SoundManager.GetInstance().ChangeVolume(SoundManager.SoundType.SE, SoundManager.FIRST_VOLUME, 3);
    }

    void OnDestroy()
    {
    }
}
