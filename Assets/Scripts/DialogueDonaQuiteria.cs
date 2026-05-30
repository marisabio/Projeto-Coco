using UnityEngine;

public class DialogueDonaQuiteria : MonoBehaviour
{
public Sprite profile;
public string speechText;
public string actorName;

public LayerMask playerLayer;
public float radious;
private DialogueControl dc;

    private void Start()
    {
     dc = FindObjectOfType<DialogueControl>();   
    }

    private void FixedUpdate()
    {
        Interact();
    }
    public void Interact()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radious, playerLayer);

        if(hit !=null)
        {
         dc.Speech(profile, speechText, actorName);  
        }
    }
}
