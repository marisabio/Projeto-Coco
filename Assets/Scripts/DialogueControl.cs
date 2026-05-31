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

    // Agora é verificado se a caixa de diálogo está ativa. Se for o caso, quando o jogador apertar o botão de interação de novo a caixa vai desligar.
    // O gameObject que precisa ser colocado no inspetor é a caixa de diálogo em si.
    // Mas a gente ainda vai precisar fazer com q o diálogo tenha mais do q só uma fala.

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
