using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text heartText;
    private int hearts;
    void Start()
    {
        hearts = 5;
    }

    void Update()
    {
        heartText.text = hearts.ToString();
    }
    public void StartLevel()
    {
        hearts--;
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
