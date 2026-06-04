using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour
{
    [Header("Componentes")]
    public GameObject dialogueObj;
    public Image profile;
    public TextMeshProUGUI speechText;
    public TextMeshProUGUI actorNameText;

    [Header("Configuracoes")]
    public float typingSpeed;

    public void Speech(Sprite p, string txt, string actorName)
    {
        if (!dialogueObj.activeSelf)
        {
            dialogueObj.SetActive(true);
            profile.sprite = p;
            speechText.text = txt;
            actorNameText.text = actorName;
        }
        else if (dialogueObj.activeSelf)
        {
            dialogueObj.SetActive(false);
        }
    }

}
