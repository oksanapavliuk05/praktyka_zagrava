using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private GameObject losePanel;
    [SerializeField]
    private GameObject pausePanel;
    private Board board;

    public void Start()
    {
        board = FindFirstObjectByType<Board>();
    }

    public void WinGame()
    {
        winPanel.SetActive(true);
        board.currentState = GameState.wait;
    }

    public void LoseGame()
    {
        losePanel.SetActive(true);
        board.currentState = GameState.wait;
    }

    public void Retry()
    {
        SceneManager.LoadScene("Level");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void PauseMode()
    {
        pausePanel.SetActive(true);
        board.currentState = GameState.wait;
    }
    public void PauseModeOff()
    {
        pausePanel.SetActive(false);
        board.currentState = GameState.move;
    }
}
