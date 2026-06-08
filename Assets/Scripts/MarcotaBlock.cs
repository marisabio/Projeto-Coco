using UnityEngine;

public class MarcotaBlock : MonoBehaviour
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
            PlayerPrefs.SetFloat("falouComMarcota", 1);
        }
        else if (PlayerPrefs.GetFloat("falouComMarcota") == 1)
        {
            col.enabled = false;
        }

    }
}
