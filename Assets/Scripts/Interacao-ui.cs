using UnityEngine;
using TMPro;

public class AreaComPainel : MonoBehaviour
{
    [Header("Configurações da UI")]
    public GameObject painelDaArea; 
    public TextMeshProUGUI campoTextoUI; 

    [Header("Conteúdo Customizado")]
    [TextArea(2, 5)] 
    public string textoPersonalizado;

    private void Start()
    {
        if (painelDaArea != null)
        {
            painelDaArea.SetActive(false);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (campoTextoUI != null && painelDaArea != null)
            {
                campoTextoUI.text = textoPersonalizado; 
                painelDaArea.SetActive(true);           
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (painelDaArea != null)
            {
                painelDaArea.SetActive(false);
            }
        }
    }
}