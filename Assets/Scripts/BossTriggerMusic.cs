using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private BossMusicManager musicManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se quem entrou foi o jogador
        if (other.CompareTag("Player"))
        {
            musicManager.StartBossMusic();
            
            // Desativa o trigge para não rodar o código de novo
            GetComponent<Collider2D>().enabled = false;
        }
    }
}