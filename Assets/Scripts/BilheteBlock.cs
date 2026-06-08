using UnityEngine;

public class BilheteBlock : MonoBehaviour
{
    private Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
    } 
    
    void Update()
    {
        if (PlayerPrefs.GetFloat("temBilhete") == 0)
        {
            col.enabled = false;
        }
        else if (PlayerPrefs.GetFloat("temBilhete") == 1)
        {
            col.enabled = true;
        }

    }
}
