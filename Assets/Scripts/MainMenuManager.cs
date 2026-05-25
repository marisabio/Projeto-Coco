using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelCreditos;
    [SerializeField] private GameObject iniciarJogo;
    [SerializeField] private GameObject fecharPainelCreditos;

    public void Jogar()
    {
      SceneManager.LoadScene(1);
      PlayerPrefs.SetFloat("health", 3);
    }
    public void AbrirCreditos()
    {
      painelMenuInicial.SetActive(false);
      painelCreditos.SetActive(true);

      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(fecharPainelCreditos);
    }
    public void FecharCreditos()
    {
      painelMenuInicial.SetActive(true);
      painelCreditos.SetActive(false);

      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(iniciarJogo);     
    }

    public void SairJogo()
    {
      Debug.Log("Jogo fechou");
      Application.Quit();    
    }

}
