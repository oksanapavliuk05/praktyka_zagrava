using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string sceneToLoad;
    public int currentLevel;
    private GameData gameData;

    private void Start()
    {
        gameData = GameData.gameData;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("LevelMap");
    }

    public void NextLevelWin()
    {
        if (gameData != null)
        {
            int nextLevelIndex = currentLevel;
            if (nextLevelIndex < gameData.saveData.isActive.Length)
            {
                gameData.saveData.isActive[nextLevelIndex] = true;
                gameData.Save();
            }
        }
        SceneManager.LoadScene(sceneToLoad);
    }

    public void NextLevelLose()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
