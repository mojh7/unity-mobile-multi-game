using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviourSingleton<Logo>
{
    #region variables
    [SerializeField] private Image bridgeLogoImage;
    [SerializeField] private Image teamLogoImage;
    [SerializeField] private Sprite[] teamLogoSprites;
    [SerializeField] private float logoDisplayDuration;
    [SerializeField] private float teamLogoAnimationInterval;
    //public Image backGround;
    //public Image teamLogoImage;
    //[SerializeField] private Sprite[] logoSprite;
    #endregion

    #region get / set
    #endregion

    #region unityFunc
    private void Start()
    {
        ShowLogo();
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
        GameManager.Instance.LoadNextScene(GameScene.TITLE, false);
    }
    private void ShowLogo()
    {
        StartCoroutine(FadeLogo());
        StartCoroutine(TeamLogoAnimation());
    }
    #endregion

    #region coroutine
    IEnumerator FadeLogo()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.1f);
        for (int i = 0; i <= 15; i++)
        {
            bridgeLogoImage.color = new Color(1, 1, 1, (float)i / 15);
            teamLogoImage.color = new Color(1, 1, 1, (float)i / 15);
            yield return YieldInstructionCache.WaitForSeconds(0.02f);
        }
        AudioManager.Instance.PlaySound("logo", SFXType.COMMON);
    }

    IEnumerator TeamLogoAnimation()
    {
        float time = 0;
        int spriteIndex = 0;
        
        while(time < logoDisplayDuration)
        {
            teamLogoImage.sprite = teamLogoSprites[spriteIndex];
            spriteIndex = (spriteIndex + 1) % teamLogoSprites.Length;
            time += teamLogoAnimationInterval;
            yield return YieldInstructionCache.WaitForSeconds(teamLogoAnimationInterval);
        }
        LoadTitle();
    }

    #endregion
}
