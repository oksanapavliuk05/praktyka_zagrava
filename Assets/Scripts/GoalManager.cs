using UnityEngine;
using TMPro;
public class GoalManager : MonoBehaviour
{
    [SerializeField]
    private Goal[] allGoals;
    [SerializeField]
    private GameObject goalPrefab;
    [SerializeField]
    private GameObject goalIntroParent;
    [SerializeField]
    private GameObject goalGameParent;
    [SerializeField]
    private TMP_Text movesText;
    private int movesClaimed = 30;
    private bool isDone;
    private GoalPanel[] allPanel;
    private GoalPanel[] gamePanels;
    private GameManager gameManager;
    private GameData gameData;
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        gameData = GameData.gameData;
        // gameManager.Start();
        allPanel = new GoalPanel[allGoals.Length];
        gamePanels = new GoalPanel[allGoals.Length];
        for(int i = 0; i<allGoals.Length; i++)
        {
            GameObject goal = Instantiate(goalPrefab, goalIntroParent.transform);       
            goal.transform.SetParent(goalIntroParent.transform); 
            GoalPanel panel = goal.GetComponent<GoalPanel>();
            panel.thisSprite = allGoals[i].GoalSprite;
            panel.UpdateText(allGoals[i].NumberClaimed(), allGoals[i].NumberNeeded());
            allPanel[i] = panel;

            GameObject gameGoal = Instantiate(goalPrefab, goalGameParent.transform);   
            gameGoal.transform.SetParent(goalGameParent.transform);  
            GoalPanel gamePanel = gameGoal.GetComponent<GoalPanel>();
            gamePanel.thisSprite = allGoals[i].GoalSprite;
            gamePanel.UpdateText(allGoals[i].NumberClaimed(), allGoals[i].NumberNeeded());
            gamePanels[i] = gamePanel;
        }
    }

    private bool IsWin()
    {
        for(int i = 0; i < allGoals.Length; i++)
        {
            if (!allGoals[i].IsDone())
            {
                return false;
            }
        }
        return true;
    }

    void Update()
    {
        for(int i = 0; i<allGoals.Length; i++)
        {
            allPanel[i].UpdateText(allGoals[i].NumberClaimed(), allGoals[i].NumberNeeded());
            gamePanels[i].UpdateText(allGoals[i].NumberClaimed(), allGoals[i].NumberNeeded());

            if(movesClaimed <=0 && !allGoals[i].IsDone())
            {
                movesText.text = "Lose!";
                gameManager.LoseGame();
            }
            else
            {
                allPanel[i].UpdateText(allGoals[i].NumberClaimed(), allGoals[i].NumberNeeded());
                if(IsWin())
                {
                    movesText.text = "Win";
                    int currentLevelIndex = LevelManager.currentLevel -1;
                    int nextLevelIndex = LevelManager.currentLevel;
                    if (gameData != null)
                    {
                        
                        gameData.saveData.isFinished[currentLevelIndex] = true;
                        if (nextLevelIndex < gameData.saveData.isActive.Length)
                        {
                            gameData.saveData.isActive[nextLevelIndex - 1] = true;
                        }
                        gameData.Save();
                    }
                    gameManager.WinGame();
                }
                else
                {
                    movesText.text = movesClaimed.ToString();
                }
            }
        }
    }
    public void IncreaseClaimed(Dot dot)
    {
        for(int i = 0; i<allGoals.Length; i++)
        {
            if(dot.tag == allGoals[i].GoalTag())
            {
                allGoals[i].Increase();
            }
        }
    }
    public void IncreaseMove()
    {
        movesClaimed--;
    }
}
