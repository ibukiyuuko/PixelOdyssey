using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPause = false;

    public GameObject pauseMenuUI;
    //public GameObject loseMenuUI;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("menu");
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //Debug.Log("esc");
            if (isPause) Resume();
            else Pause();
        }
    }

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            isPause = false;
        }

        public void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            isPause = true;
        }

    public void BTMenu()
    {
        Time.timeScale = 1f;
        //SceneManager.LoadScene("GamePlayScene");
        SceneManager.LoadScene("MainMenu");
    }
    public void RestartG()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GamePlayScene");
        //SceneManager.LoadScene("MainMenu");
    }

    /*public void LoadMenu()
    {
        //Debug.Log("loadingMenu");
        SceneManager.LoadScene("Menu");
    }*/

    public void QuitGame()
    {
        //Debug.Log("QuitGame");
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
        Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
#endif
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
                        Applicatioin.Quit();
#elif (UNITY_WEBGL)
                        SceneManager.LoadScene("QuitScene");
#endif
    }
}
