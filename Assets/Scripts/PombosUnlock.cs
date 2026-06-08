using UnityEngine;

public class PombosUnlock : MonoBehaviour
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
            PlayerPrefs.SetFloat("canDoubleJump", 1);
            PlayerPrefs.SetFloat("falouComPombos", 1);
        }
        else if (PlayerPrefs.GetFloat("falouComPombos") == 1)
        {
            col.enabled = false;
        }

    }
}
