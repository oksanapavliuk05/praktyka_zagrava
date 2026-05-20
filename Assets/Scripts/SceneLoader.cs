using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private string sceneToLoad;
    private GameData gameData;

    private void Start()
    {
        gameData = GameData.gameData;
    }

    public void NextLevelWin()
    {
        int nextLevelIndex = LevelManager.currentLevel + 1;

        if (gameData != null)
        {
            // позначаємо поточний рівень як завершений
            gameData.saveData.isFinished[LevelManager.currentLevel - 1] = true;

            // відкриваємо наступний рівень
            if (nextLevelIndex <= gameData.saveData.isActive.Length)
            {
                gameData.saveData.isActive[nextLevelIndex - 1] = true;
            }

            gameData.Save();
        }

        LevelManager.hearts--;
        LevelManager.currentLevel = nextLevelIndex;

        sceneToLoad = "Level " + nextLevelIndex;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void NextLevelLose()
    {
        
        LevelManager.hearts--;
        sceneToLoad = "Level " + LevelManager.currentLevel;
        SceneManager.LoadScene(sceneToLoad);
        
    }
    public void Retry()
    {
        LevelManager.hearts--;
        sceneToLoad = "Level " + LevelManager.currentLevel;
        SceneManager.LoadScene(sceneToLoad);
    }
    public void LevelMap()
    {
        SceneManager.LoadScene("LevelMap");
    }
    public void MainMenu()
    {
        if (gameData != null)
        {
            int currentLevel = LevelManager.currentLevel;
            int nextLevel = currentLevel + 1;

            // відмічаємо рівень як пройдений
            gameData.saveData.isFinished[currentLevel - 1] = true;

            // відкриваємо наступний
            if (nextLevel <= gameData.saveData.isActive.Length)
            {
                gameData.saveData.isActive[nextLevel - 1] = true;
            }

            gameData.Save();
        }

        SceneManager.LoadScene("LevelMap");
    }
    public void PlayStory()
    {
        SceneManager.LoadScene("Story");
    }
    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}
