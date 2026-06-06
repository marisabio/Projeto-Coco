using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour
{
    public GameObject dialogueObj;
    //public Image[] profile;
    public TextMeshProUGUI speechText;
    public TextMeshProUGUI actorNameText;

    public string[] lines;
    //public string[] names;
    public float textSpeed;

    private int index;

    // public void Speech(Sprite p, string txt, string actorName)
    //{
    //if (!dialogueObj.activeSelf)
    //   {
    //      dialogueObj.SetActive(true);
    //      profile[i].sprite = p;
    //       speechText[i].text = txt;
    //      actorNameText[i].text = actorName;
    //  }
    //}

    void Update()
    {
        
    }

    public void StartDialogue()
    {
        if (!dialogueObj.activeSelf)
        {
            dialogueObj.SetActive(true);
            speechText.text = string.Empty;
            index = 0;
            StartCoroutine(TypeLine());
        }
        else if (speechText.text == lines[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            speechText.text = lines[index];
        }
    }

    private IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            speechText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if (index <lines.Length -1)
        {
            index++;
            speechText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueObj.SetActive(false);
        }
    }

}
