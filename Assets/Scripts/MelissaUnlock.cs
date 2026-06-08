using UnityEngine;
using UnityEngine.SceneManagement;

public class MelissaUnlock : MonoBehaviour
{
    private Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
    } 
    
    void Update()
    {
        if (col.enabled == false)
        {
            PlayerPrefs.SetFloat("canEggAttack", 1);
            PlayerPrefs.SetFloat("falouComMelissa", 1);
            SceneManager.LoadScene(4);
        }
        else if (PlayerPrefs.GetFloat("falouComMelissa") == 1)
        {
            col.enabled = false;
        }

    }
}
