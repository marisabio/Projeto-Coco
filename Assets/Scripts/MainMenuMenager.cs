using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string nomeDoLevelDejogo;
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelCreditos;
    public void Jogar()
    {
       SceneManager.LoadScene(nomeDoLevelDejogo); 
    }
    public void AbrirCreditos()
    {
     painelMenuInicial.SetActive(false);
     painelCreditos.SetActive(true);
    }
    public void FecharCreditos()
    {
    painelMenuInicial.SetActive(true);
     painelCreditos.SetActive(false);     
    }

      public void SairJogo()
    {
    Debug.Log("Jogo fechou");
    Application.Quit();    
    }

}
