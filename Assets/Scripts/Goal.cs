using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Goal 
{    
    private bool isDone;
    public Sprite GoalSprite;
    [SerializeField]
    private string goalTag;
    public int numberNeeded = 0;
    private int numberClaimed = 0;
    public int NumberNeeded()
    {
        return numberNeeded;
    }
    public int NumberClaimed()
    {
        return numberClaimed;
    }
    public string GoalTag()
    {
        return goalTag;
    }
    public bool IsDone()
    {
        return isDone;
    }
    public int Increase()
    {
        numberClaimed += 1;
        if(numberClaimed >= numberNeeded)
        {
            isDone = true;
        }
        return numberClaimed;
    }
}
