using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
    }
    public void IncreaseScore(int value)
    {
        score+=value;
    }
}
