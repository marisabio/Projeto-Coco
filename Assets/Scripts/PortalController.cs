using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    [SerializeField] int levelIndex;

    public void LoadNextLevel() 
    {
        SceneManager.LoadScene(levelIndex);
    }

}
