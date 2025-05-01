using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
      //  SceneManager.LoadScene("");
    }



    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}

