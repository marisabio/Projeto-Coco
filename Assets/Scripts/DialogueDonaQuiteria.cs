using UnityEngine;

public class DialogueDonaQuiteria : MonoBehaviour
{
    public Sprite profile;
    public string speechText;
    public string actorName;
    public LayerMask playerLayer;
    public GameObject dialogueControl;

    public void Interact()
    {
        Debug.Log("Teste");
        dialogueControl.GetComponent<DialogueControl>().Speech(profile, speechText, actorName);
    }


}
