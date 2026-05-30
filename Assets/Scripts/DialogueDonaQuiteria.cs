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
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radious, playerLayer); // Cria o campo do trigger

        if(hit != null)
        {
         Debug.Log("Detectou o objeto: " + hit.name); // teste pra saber se chamou aqui
         dc.Speech(profile, speechText, actorName);  // aqui era para aparecer a caixa de dialogo
        }
    }
    private void OnDrawGizmosSelected()
    {
      Gizmos.DrawWireSphere(transform.position, radious); // mostra o circulo do trigger
    }

}
