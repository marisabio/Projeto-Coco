using UnityEngine;

public class EggProjectile : MonoBehaviour
{
    private Rigidbody2D rb;

    public void Setup(Vector3 dir, float force)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }

}
