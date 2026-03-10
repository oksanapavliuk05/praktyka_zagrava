using UnityEngine;
using System.Collections;
public class Scene_01 : MonoBehaviour
{
    [SerializeField]
    private GameObject FadeIn;
    
    [SerializeField]
    private GameObject TextBox;
    [SerializeField]
    private GameObject MainCharacter;
    
    [SerializeField]
    private GameObject Character2;  
    [SerializeField]
    private string textToSpeak;
    [SerializeField]
    private int currentTextLength;
    [SerializeField]
    private int textLength;
    [SerializeField]
    private GameObject mainTextObject;
    [SerializeField]
    private GameObject nextBtn;
    [SerializeField]
    private int eventPos = 0;
    [SerializeField]
    private GameObject charName;
    

    void Update()
    {
        textLength = TextCreate.charCount;
    }
    
      
    void Start()
    {
        StartCoroutine(EventStarter());
    }


    public IEnumerator EventStarter()
    { 
        //event 0
        yield return new WaitForSeconds(2);
        FadeIn.SetActive(false);
        Character2.SetActive(true);
        // yield return new WaitForSeconds(2);\
        mainTextObject.SetActive(true);
        textToSpeak = "Detective Carter, we didn't think you'd actually show up.";
        TextBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreate.runTextPrint = true;
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(()=> textLength == currentTextLength);
        yield return new WaitForSeconds(0.5f);
        nextBtn.SetActive(true);
        eventPos = 1;

    }

    public IEnumerator EventOne()
    {
        //event 1
        nextBtn.SetActive(false);
        MainCharacter.SetActive(true);
        TextBox.SetActive(true);
        charName.GetComponent<TMPro.TMP_Text>().text = "Carter";
        // yield return new WaitForSeconds(2);

        textToSpeak = "Moore, don't even hope for that. What do we have here?";
        TextBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreate.runTextPrint = true;
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(()=> textLength == currentTextLength);
        yield return new WaitForSeconds(0.5f);
        nextBtn.SetActive(true);
    }

    public void OnClick()
    {
        if(eventPos == 1)
        {
            StartCoroutine(EventOne());
        }
        
    }
}
