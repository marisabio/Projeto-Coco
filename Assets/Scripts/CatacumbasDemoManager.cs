using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CatacumbasDemoManager : MonoBehaviour
{
    public void EndDemo()
    {
        Invoke(nameof(ReloadDemo), 2);
    }

    private void ReloadDemo()
    {
        SceneManager.LoadScene(0);
    }
    
}
