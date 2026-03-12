using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;   
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public bool isActive;
    private Color activeColor = new Color(1f, 1f, 1f, 1f);
    private Color lockedColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    private Image buttonImage;
    public Button myButton;
    //ui text
    public TMP_Text  levelText;
    public int level;
    public string levelToLoad;
    void Awake()
    {
        buttonImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
    }
    public void SetUp(int level)
    {
        isActive = true;
        level++;
        levelText.text = level.ToString();
        DecideSprite(); 

    }

    void DecideSprite()
    {
        if (isActive)
        {
            buttonImage.color = activeColor;
            myButton.enabled  = true;
            // levelText.enabled = true;
        }
        else
        {
            buttonImage.color = lockedColor;
            myButton.enabled  = false;
            // levelText.enabled = false;
            
        }
    }

    public void StartLevel()
    {
        if(LevelManager.hearts > 0)
        {
            LevelManager.hearts--;
            LevelManager.timer = 0;
            // level++;
            levelToLoad = "Level " + level;
            SceneManager.LoadScene(levelToLoad);
            
        }
    }
}
