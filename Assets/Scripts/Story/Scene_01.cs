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
        eventPos = 2;
    }
    public IEnumerator EventTwo()
    {
        //event 2
        nextBtn.SetActive(false);
        MainCharacter.SetActive(true);
        TextBox.SetActive(true);
        charName.GetComponent<TMPro.TMP_Text>().text = "Moore";
        // yield return new WaitForSeconds(2);

        textToSpeak = "The body was found this morning. The maid came in for her shift. Said she knocked like usual, but nobody answered. Normally the owner lets her in. But today the door was alredy open. So she walked in... and found him lying on the floor";
        TextBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreate.runTextPrint = true;
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(()=> textLength == currentTextLength);
        yield return new WaitForSeconds(0.5f);
        nextBtn.SetActive(true);
        eventPos = 3;
    }

    public IEnumerator EventThree()
    {
        //event 3
        nextBtn.SetActive(false);
        MainCharacter.SetActive(true);
        TextBox.SetActive(true);
        charName.GetComponent<TMPro.TMP_Text>().text = "Carter";
        // yield return new WaitForSeconds(2);

        textToSpeak = "Did she touch anything?";
        TextBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreate.runTextPrint = true;
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(()=> textLength == currentTextLength);
        yield return new WaitForSeconds(0.5f);
        nextBtn.SetActive(true);
        eventPos = 4;
    }
    
    public IEnumerator EventFour()
    {
        //event 4
        nextBtn.SetActive(false);
        MainCharacter.SetActive(true);
        TextBox.SetActive(true);
        charName.GetComponent<TMPro.TMP_Text>().text = "Moore";
        // yield return new WaitForSeconds(2);

        textToSpeak = "She says she panicked and ran out immediatly. Called the police from the hallway.";
        TextBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreate.runTextPrint = true;
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(()=> textLength == currentTextLength);
        yield return new WaitForSeconds(0.5f);
        nextBtn.SetActive(true);
        eventPos = 5;
    }
    
    public IEnumerator EventFive()
    {
        //event 5
        nextBtn.SetActive(false);
        MainCharacter.SetActive(true);
        TextBox.SetActive(true);
        charName.GetComponent<TMPro.TMP_Text>().text = "Carter";
        // yield return new WaitForSeconds(2);

        textToSpeak = "No signs of forced entry. Which means either he knew his killer...";
        TextBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreate.runTextPrint = true;
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(()=> textLength == currentTextLength);
        yield return new WaitForSeconds(0.5f);
        nextBtn.SetActive(true);
        eventPos = 6;
    }
    public IEnumerator EventSix()
    {
        //event 6
        nextBtn.SetActive(false);
        MainCharacter.SetActive(true);
        TextBox.SetActive(true);
        charName.GetComponent<TMPro.TMP_Text>().text = "Carter";
        // yield return new WaitForSeconds(2);

        textToSpeak = "... or someone wanted it to look that way.";
        TextBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreate.runTextPrint = true;
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(()=> textLength == currentTextLength);
        yield return new WaitForSeconds(0.5f);
        nextBtn.SetActive(true);
        eventPos = 7;
    }
    public IEnumerator EventSeven()
    {
        //event 7
        nextBtn.SetActive(false);
        MainCharacter.SetActive(true);
        TextBox.SetActive(true);
        charName.GetComponent<TMPro.TMP_Text>().text = "Carter";
        // yield return new WaitForSeconds(2);

        textToSpeak = "So let's we take a good look around.";
        TextBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
        currentTextLength = textToSpeak.Length;
        TextCreate.runTextPrint = true;
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(()=> textLength == currentTextLength);
        yield return new WaitForSeconds(0.5f);
        nextBtn.SetActive(true);
        eventPos = 8;
    }
    public void OnClick()
    {
        if(eventPos == 1)
        {
            StartCoroutine(EventOne());
        }else if(eventPos == 2)
        {
            StartCoroutine(EventTwo());
        }else if(eventPos == 3)
        {
            StartCoroutine(EventThree());
        }else if(eventPos == 4)
        {
            StartCoroutine(EventFour());
        }else if(eventPos == 5)
        {
            StartCoroutine(EventFive());
        }else if(eventPos == 6)
        {
            StartCoroutine(EventSix());
        }else if(eventPos == 7)
        {
            StartCoroutine(EventSeven());
        }
        
    }
}
