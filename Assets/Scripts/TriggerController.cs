using UnityEngine;
using UnityEngine.Events;

public class TriggerController : MonoBehaviour
{
    public UnityEvent onTrigger;

    private void OnTriggerEnter2D(Collider2D other)
        {
            // Verifica se quem entrou foi o Player
            if (other.CompareTag("Player"))
            {
                onTrigger.Invoke();
            }
        }
}
