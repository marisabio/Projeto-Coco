using UnityEngine;

public class QuiteriaBlock : MonoBehaviour
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
            PlayerPrefs.SetFloat("falouComQuiteria", 1);
        }
        else if (PlayerPrefs.GetFloat("falouComQuiteria") == 1)
        {
            col.enabled = false;
        }

    }
}
