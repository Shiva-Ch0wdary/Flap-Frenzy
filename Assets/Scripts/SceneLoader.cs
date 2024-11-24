using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
     public void LoadDayWinterScene()
     {
        SceneManager.LoadScene("Day Winter");
     }

     public void LoadDayCavesScene()
     {
        SceneManager.LoadScene("Day Caves");
     }

     public void LoadDaySunnyScene()
     {
        SceneManager.LoadScene("Day Sunny");
     }
}
