using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{



    public void Update()
    {

        if (Input.anyKeyDown )
        {
            PlayGame();
        }

    }
    

    public void PlayGame()
    {
        StartCoroutine(LoadGameScene());
    }
    IEnumerator LoadGameScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
