using UnityEngine;
using TMPro; // Necessário para o texto TextMeshPro

public class ExibirUIPorProximidade : MonoBehaviour
{
    [Header("Componentes da UI")]
    [SerializeField] private GameObject painelDaUI;       //imagem do botao
    [SerializeField] private TextMeshProUGUI campoDeTexto; // o campo do texto

    [Header("Conteúdo Customizado desta Área")]
    [TextArea(2, 5)]
    [SerializeField] private string textoPersonalizado;    // botao personalizado

    private void Start()
    {
        
        if (painelDaUI != null)
        {
            painelDaUI.SetActive(false);
        }
    }

    // detectar o jogador na area
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se quem entrou foi o Player
        if (other.CompareTag("Player"))
        {
            if (painelDaUI != null && campoDeTexto != null)
            {
                campoDeTexto.text = textoPersonalizado; // Altera para o texto desta área
                painelDaUI.SetActive(true);             // Mostra a imagem com o texto
            }
        }
    }

    // Quando o jogador sai da área do colisor
    private void OnTriggerExit2D(Collider2D other)
    {
        // Verifica se quem saiu foi o Player
        if (other.CompareTag("Player"))
        {
            if (painelDaUI != null)
            {
                painelDaUI.SetActive(false); // Esconde a imagem e o texto
            }
        }
    }
}