using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayMusic(0);
    }

    public void LoadIngame()
    {
        GameManager.Instance.LoadNextScene(GameScene.IN_GAME, true);
    }
}
