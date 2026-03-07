using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GoalPanel : MonoBehaviour
{
    public Image thisImage;
    public Sprite thisSprite;
    public TMP_Text thisText;
    public string thisString;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Setup();
    }

    void Setup()
    {
        thisImage.sprite = thisSprite;
        thisText.text = thisString;
    }

    // Update is called once per frame
    public void UpdateText(int claimed, int needed)
    {
        if(claimed >= needed)
        {
            thisText.text = "Done!";
        }
        else
        {
            thisText.text = claimed + "/" + needed;
        }
    }
}
