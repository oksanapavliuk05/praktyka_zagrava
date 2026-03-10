using UnityEngine;
using System.Collections;
public class TextCreate : MonoBehaviour
{
    public static TMPro.TMP_Text viewText;
    public static bool runTextPrint;
    public static int charCount;
    [SerializeField]
    private string transferText;
    [SerializeField]
    private int internalCount;    

    void Update()
    {
        internalCount = charCount;
        //number letter in text
        charCount = GetComponent<TMPro.TMP_Text>().text.Length;
        if(runTextPrint == true)
        {
            runTextPrint = false;
            viewText = GetComponent<TMPro.TMP_Text>();
            transferText = viewText.text;
            viewText.text = "";
            StartCoroutine(RoolText());
        }
    }

    public IEnumerator RoolText()
    {
        foreach(char c in transferText)
        {
            viewText.text += c;
            yield return new WaitForSeconds(0.03f);
        }
    }
}
