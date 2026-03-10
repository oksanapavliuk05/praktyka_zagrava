using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text heartText;
    private static int hearts = 5;
    private int maxHearts = 10;
    private float everytime = 108000f; 
    private static float timer;

    void Update()
    {
        heartText.text = hearts.ToString();

        if(hearts < maxHearts)
        {
            timer += Time.deltaTime;

            if(timer >= everytime)
            {
                hearts++;
                timer = 0;
            }
        }
    }
    public void StartLevel()
    {
        hearts--;
        timer = 0;
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
