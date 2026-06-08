using UnityEngine;

public class InverseBilheteBlock : MonoBehaviour
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
            col.enabled = true;
        }
        else if (PlayerPrefs.GetFloat("temBilhete") == 1)
        {
            col.enabled = false;
        }

    }
}
