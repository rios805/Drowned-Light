using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
      Loader.Load("MainGame");
    }



    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}

