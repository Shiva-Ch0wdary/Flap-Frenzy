using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
 

    public void LoadLevel1()
    {
        AudioManager.Instance.PlayGameplayMusic();
        SceneManager.LoadScene("Day Caves");
    }

    public void LoadLevel2()
    {
        AudioManager.Instance.PlayGameplayMusic();
        SceneManager.LoadScene("Day Winter");
    }

    public void LoadLevel3()
    {
        AudioManager.Instance.PlayGameplayMusic();
        SceneManager.LoadScene("Day Sunny");
    }
}
