using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelCreditos;

    public void Jogar()
    {
      SceneManager.LoadScene(1);
      PlayerPrefs.SetFloat("health", 3);
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
