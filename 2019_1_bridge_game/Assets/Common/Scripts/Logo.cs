using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviourSingleton<Logo>
{
    #region variables
    [SerializeField]
    private Image bridgeLogoImage;
    //public Image backGround;
    //public Image teamLogoImage;
    //[SerializeField] private Sprite[] logoSprite;
    #endregion

    #region get / set
    #endregion

    #region unityFunc
    private void Start()
    {
        LoadLogo();
        
    }

    void Update()
    {
        // 빠른 스킵용
        if (Input.GetKeyDown(KeyCode.T))
            LoadTitle();
    }
    #endregion

    #region func
    private void LoadTitle()
    {
        GameManager.Instance.LoadTitle();
    }
    private void LoadLogo()
    {
        StartCoroutine(FadeLogo(bridgeLogoImage));
        AudioManager.Instance.PlaySound("bridgeOpeningSound", SFX_TYPE.COMMON);
    }
    #endregion


    #region coroutine
    IEnumerator FadeLogo(Image image)
    {
        if (image != null)
        {
            for (int i = 0; i <= 30; i++)
            {
                image.color = new Color(1, 1, 1, (float)i / 10);
                yield return YieldInstructionCache.WaitForSeconds(0.05f);
            }
            for (int i = 10; i >= 0; i--)
            {
                image.color = new Color(1, 1, 1, (float)i / 10);
                yield return YieldInstructionCache.WaitForSeconds(0.05f);
            }

            //if (image != teamLogoImage)
            //{
            //    StartCoroutine(AnimationLogo(teamLogoImage));
            //}
            //else
            LoadTitle();
        }
    }

    // team logo
    //IEnumerator AnimationLogo(Image image)
    //{
    //    if (image != null)
    //    {
    //        for (int i = 0; i < 10; i++)
    //        {
    //            backGround.color = new Color(0, 0, 0, (float)i / 10);
    //            yield return YieldInstructionCache.WaitForSeconds(0.05f);
    //        }
    //        image.gameObject.SetActive(true);
    //        backGround.color = new Color(0, 0, 0);
    //        for (int i = 0; i < logoSprite.Length; i++)
    //        {
    //            image.sprite = logoSprite[i];
    //            yield return YieldInstructionCache.WaitForSeconds(0.1f);
    //        }
    //        image.gameObject.SetActive(false);
    //        for (int i = 10; i >= 0; i--)
    //        {
    //            backGround.color = new Color(0, 0, 0, (float)i / 10);
    //            yield return YieldInstructionCache.WaitForSeconds(0.05f);
    //        }
    //    }
    //    if (image == teamLogoImage)
    //        LoadTitle();
    //}
    #endregion
}
