using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   [Header("Componentes")]
   public GameObject dialogueObj;

public Image profile;
public TextMeshProUGUI speechText;
public TextMeshProUGUI actorNameText;

[Header("Configuracoes")]
public float typingSpeed;

public void Speech(Sprite p, string txt, string actorName)
    {
        dialogueObj.SetActive(true);
        profile.sprite = p;
        speechText.text = txt;
        actorNameText.text = actorName;
    }



}
