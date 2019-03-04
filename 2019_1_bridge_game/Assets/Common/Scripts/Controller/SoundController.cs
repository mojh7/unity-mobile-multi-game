using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/*
 * 사운드 분류 추가 할 때 해야 할 3가지
 * 1. SFX_TYPE Enum 추가
 * 2. AudioClip[] 만들기
 * 3. Play 함수에서 switch안에 내용 추가 하기
 */

 /* Play 호출 방식 2가지
  * 1. index 기반
  * 2. 파일 명 string key로 사용하여 호출
  */
  

public enum SFXType
{
    COMMON,
    UI,
}

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviourSingleton<SoundController>
{
    #region variables
    //[Header("[PlayerPrefs Key]")]
    //[SerializeField] private string saveKey = "Option_Sound";
    [SerializeField]
    private AudioClip[] commonSfxList;
    [SerializeField]
    private AudioClip[] uiSfxList;

    private Dictionary<string, AudioClip> commonSfxDictionary;
    private Dictionary<string, AudioClip> uiSfxDictionary;

    private AudioSource audioSource;
    private float volume;
    #endregion

    #region get / set
    public void SetVolume(float volume)
    {
        this.volume = volume;
        audioSource.volume = volume;
    }
    #endregion

    #region unityFunc
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (null == uiSfxList)
            return;

        commonSfxDictionary = new Dictionary<string, AudioClip>();
        for (int i = 0; i < commonSfxList.Length; i++)
        {
            if (!commonSfxDictionary.ContainsKey(commonSfxList[i].name))
            {
                commonSfxDictionary.Add(commonSfxList[i].name, commonSfxList[i]);
            }
        }
        uiSfxDictionary = new Dictionary<string, AudioClip>();
        for (int i = 0; i < uiSfxList.Length; i++)
        {
            if (!uiSfxDictionary.ContainsKey(uiSfxList[i].name))
            {
                uiSfxDictionary[uiSfxList[i].name] = uiSfxList[i];
            }
        }
    }
    #endregion

    #region func
    // 사운드 On/Off 여부 확인, 임시로 true
    public bool IsEnableSound()
    {
        return true;
        //return PlayerPrefs.GetInt(saveKey, 1) == 1 ? true : false;
    }

    /*
    // 사운드 설정 저장
    public void EnableSound(bool enable)
    {
        PlayerPrefs.SetInt(saveKey, enable ? 1 : 0);
        PlayerPrefs.Save();
    }

    // 사운드 On/Off 토글
    public void ToggleSound()
    {
        EnableSound(!IsEnableSound());
    }*/


    /// <summary>
    /// 사운드 재생, index 기반
    /// </summary>
    public void Play(int sfxIndex, SFXType sfxType)
    {
        if (sfxIndex < 0)
            return;

        AudioClip _clip = null;

        switch (sfxType)
        {
            case SFXType.COMMON:
                _clip = commonSfxList[sfxIndex];
                break;
            case SFXType.UI:
                _clip = uiSfxList[sfxIndex];
                break;
            default:
                break;
        }

        if (_clip == null)
            return;

        audioSource.PlayOneShot(_clip);
    }

    /// <summary>
    /// 사운드 재생, string key
    /// </summary>
    public void Play(string sfxName, SFXType sfxType)
    {
        if ("" == sfxName || "NONE" == sfxName)
            return;
        AudioClip _clip = null;
        switch (sfxType)
        {
            case SFXType.COMMON:
                _clip = commonSfxDictionary[sfxName];
                break;
            case SFXType.UI:
                _clip = uiSfxDictionary[sfxName];
                break;
            default:
                break;
        }
        if (_clip == null)
            return;

        audioSource.PlayOneShot(_clip);
    }
    #endregion
}
