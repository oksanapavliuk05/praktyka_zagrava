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
    private GameData gameData;
    void Awake()
    {
        buttonImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
        gameData = GameData.gameData;
    }
    public void SetUp(int level)
    {
        // isActive = true;
        this.level = level + 1;
        levelText.text = this.level.ToString();
        LoadData();
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

    void LoadData()
    {
        if(gameData != null)
        {
            if (gameData.saveData.isActive[level - 1])
            {
                isActive = true;
            }
            else
            {
                isActive=false;
            }
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
