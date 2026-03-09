using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public void StartLevel()
    {
        SceneManager.LoadScene("Level");
    }
    public void PlayStory()
    {
        SceneManager.LoadScene("Story");
    }
    public void StartGame()
    {
        SceneManager.LoadScene("LevelMap");
    }
    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}
