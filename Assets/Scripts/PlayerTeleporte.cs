using Unity.VisualScripting;
using UnityEngine;

public class PlayerTeleporte : MonoBehaviour
{

    private GameObject currentTeleporter;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentTeleporter != null)
            {
              transform.position = currentTeleporter.GetComponent<Teleporte>().GetDestination().position;
            }
        }  
    }

public void Teleporter()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
         currentTeleporter = collision.gameObject;   
        }
    }
     private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
         if (collision.gameObject == currentTeleporter)
            {
                currentTeleporter = null;
            }   
        } 
    }
}
