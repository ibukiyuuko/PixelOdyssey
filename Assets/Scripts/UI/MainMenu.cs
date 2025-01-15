using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject startG;
    public GameObject loseMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartG()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GamePlayScene");
        
    }

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
