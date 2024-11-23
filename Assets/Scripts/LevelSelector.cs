using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
 

    public void LoadCavesLevel()
    {
        AudioManager.Instance.PlayGameplayMusic();
        SceneManager.LoadScene("Day Caves");
    }

    public void LoadWinterLevel()
    {
        AudioManager.Instance.PlayGameplayMusic();
        SceneManager.LoadScene("Day Winter");
    }

    public void LoadSunnyLevel()
    {
        AudioManager.Instance.PlayGameplayMusic();
        SceneManager.LoadScene("Day Sunny");
    }
}
