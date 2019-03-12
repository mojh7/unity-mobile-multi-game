using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * AudioManager Class
 * Music과 Sound 둘 다 관리하는 Class로 싱글톤을 사용합니다. 
 */
/* http://cafe.naver.com/unityhub
 * 제목 : 첫작품에 사용된 배경음악/사운드매니저 공유해봅니다
 */

// 다른데서 가져와서 수정 조금한 코드



public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    #region variables
    private MusicController musicController;
    private SoundController soundController;
    private float sfxVolume;

    [Header("디버그용, 배경음악 안 듣고 싶을 때 꺼주세요")]
    public bool canPlayMusic;
    #endregion variables

    #region get / set
    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    public void SetMusicVolume(float volume)
    {
        musicController.SetVolume(volume);
    }

    public void SetSoundVolume(float volume)
    {
        sfxVolume = volume;
        soundController.SetVolume(volume);
    }
    #endregion

    #region unityFunc
    void Awake()
    {
        musicController = MusicController.Instance;
        soundController = SoundController.Instance;
        sfxVolume = 1f;
    }
    #endregion

    #region func
    /*
    // 음악 설정
    */
    public bool IsEnableMusic()
    {
        return musicController.IsEnableMusic();
    }

    public bool IsEnableSound()
    {
        return soundController.IsEnableSound();
    }

    public void PauseMusic()
    {
        musicController.Pause();
    }

    public void ResumeMusic()
    {
        musicController.Resume();
    }

    public void StopMusic()
    {
        musicController.Stop();
    }

    /// <summary>
    /// 배경음악 index로 실행
    /// </summary>
    /// <param name="clipindex"></param>
    /// <param name="ignoresame"></param>
    public void PlayMusic(int musicIndex, bool ignoresame = false)
    {
        if (false == canPlayMusic)
            return;
        musicController.Play(musicIndex, ignoresame);
    }

    //TODO : string을 key로 하여 배경음악 실행 만들기

    /// <summary>
    /// 사운드(효과음) index로 실행
    /// </summary>
    /// <param name="soundIndex">0이상의 사운드 인덱스</param>
    /// <param name="soundtype">출력할 사운드 타입</param>
    public void PlaySound(int sfxIndex, SFXType sfxtype)
    {
        soundController.Play(sfxIndex, sfxtype);
    }

    /// <summary>
    /// 사운드(효과음) string을 key로 사용하여 실행
    /// </summary>
    /// <param name="soundIndex">0이상의 사운드 인덱스</param>
    /// <param name="soundtype">출력할 사운드 타입</param>
    public void PlaySound(string sfxName, SFXType soundtype)
    {
        soundController.Play(sfxName, soundtype);
    }
    #endregion
}
