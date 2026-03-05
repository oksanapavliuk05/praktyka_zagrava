using UnityEngine;
using UnityEngine.UI;
public class GoalManager : MonoBehaviour
{
    [SerializeField]
    public Text movesText;
    // private string typeOfGoal;
    
    [SerializeField]
    private GameObject Sprite;
    private int movesClaimed = 30;
    private bool isDone;
    
    [SerializeField]
    public Text goalText;
    [SerializeField]
    public Sprite GoalSprite;
    [SerializeField]
    public string goalTag;
    public int numberNeeded = 30;
    public int numberClaimed = 0;
    void Start()
    {
        Image sp = Sprite.GetComponent<Image>();
        sp.sprite = GoalSprite;

    }

    // Update is called once per frame
    void Update()
    {
        if(numberClaimed >= numberNeeded)
        {
            goalText.text = "Done!";
        }else if(movesClaimed <=0)
        {
            movesText.text = "Lose!";
        }
        else
        {
            goalText.text = numberClaimed.ToString() + "/" + numberNeeded.ToString();
            movesText.text = movesClaimed.ToString();
        }
    }
    public void IncreaseClaimed(Dot dot, int value=1)
    {
        if(dot.tag == goalTag)
        {
            numberClaimed += value;
            
        }
    }
    public void IncreaseMove()
    {
        movesClaimed--;
    }
}
