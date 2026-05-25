using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string nomeDoLevelDejogo;
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelCreditos;

    [Header("Configuração do Controle")]
    [SerializeField] private GameObject botaoVoltarNosCreditos;
    [SerializeField] private GameObject botaoCreditosNoMenuPrincipal;
    public void Jogar()
    {
       SceneManager.LoadScene(nomeDoLevelDejogo); 
    }
    public void AbrirCreditos()
    {
     painelMenuInicial.SetActive(false);
     painelCreditos.SetActive(true);
     EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botaoVoltarNosCreditos);
    }
    public void FecharCreditos()
    {
    painelMenuInicial.SetActive(true);
     painelCreditos.SetActive(false);    
     EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botaoCreditosNoMenuPrincipal); 
    }

      public void SairJogo()
    {
    Debug.Log("Jogo fechou");
    Application.Quit();    
    }

}
