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
    private TMP_Text movesText;
    private int movesClaimed = 30;
    private bool isDone;
    private GoalPanel[] allPanel;
    void Start()
    {
        allPanel = new GoalPanel[allGoals.Length];
        for(int i = 0; i<allGoals.Length; i++)
        {
            GameObject goal = Instantiate(goalPrefab, goalIntroParent.transform);       
            GoalPanel panel = goal.GetComponent<GoalPanel>();
            panel.thisSprite = allGoals[i].GoalSprite;
            panel.UpdateText(allGoals[i].NumberClaimed(), allGoals[i].NumberNeeded());
            allPanel[i] = panel;
        }
    }
    void Update()
    {
        for(int i = 0; i<allGoals.Length; i++)
        {
            if(movesClaimed <=0)
            {
                movesText.text = "Lose!";
            }
            else
            {
                allPanel[i].UpdateText(allGoals[i].NumberClaimed(), allGoals[i].NumberNeeded());
                movesText.text = movesClaimed.ToString();
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
