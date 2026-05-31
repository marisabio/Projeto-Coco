using UnityEngine;

public class DialogueDonaQuiteria : MonoBehaviour
{
    public Sprite profile;
    public string speechText;
    public string actorName;
    public LayerMask playerLayer;
    public GameObject dialogueControl;

    // Eu troquei o sisteminha de trigger ser o de interegíveis que tinha feito antes. Agora apertando o botão f (pode ser outro) trigga o dialogo.
    // Acaba sendo mais simples. Pra situações especiais, dá pra criar outros tipos de trigger.
    // Se vc tiver dúvida sobre como funciona o sisteminha de interagíveis, só falar comigo. Mas não deixa de dar uma olhadinha em UnityEvents depois!

    public void Interact()
    {
        Debug.Log("Teste");
        dialogueControl.GetComponent<DialogueControl>().Speech(profile, speechText, actorName);
    }


}
