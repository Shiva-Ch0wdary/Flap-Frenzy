using UnityEngine;
using UnityEngine.SceneManagement;
public class Na : MonoBehaviour
{
     public void GoToStartMenu()
    {
        
        SceneManager.LoadScene("Start Menu");

      
        SceneManager.sceneLoaded += OnStartMenuSceneLoaded;
    }


      public void GoToLevelsMenu()
    {
        
        SceneManager.LoadScene("Start Menu");

      
        SceneManager.sceneLoaded += OnLevelSceneLoaded;
    }

   
    private void OnStartMenuSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Start Menu")
        {
           
            GameObject levelMenuPanel = GameObject.Find("LevelsMenu");
            GameObject playMenuPanel = GameObject.Find("StartMenuPanel");

           
            if (levelMenuPanel != null && playMenuPanel != null)
            {
                levelMenuPanel.SetActive(true);
                playMenuPanel.SetActive(false);
            }

            SceneManager.sceneLoaded -= OnStartMenuSceneLoaded;
        }
    }




     private void OnLevelSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Start Menu")
        {
           
            GameObject levelMenuPanel = GameObject.Find("LevelsMenu");
            GameObject playMenuPanel = GameObject.Find("StartMenuPanel");

           
            if (levelMenuPanel != null && playMenuPanel != null)
            {
                levelMenuPanel.SetActive(false);
                playMenuPanel.SetActive(true);
            }

            SceneManager.sceneLoaded -= OnLevelSceneLoaded;
        }
    }

    public void RestartScene()
{
        AudioManager.Instance.PlayGameplayMusic();
        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();
    // Reload the active scene
    SceneManager.LoadScene(currentScene.name);

}
}
