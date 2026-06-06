using UnityEngine;

public class Teleporte : MonoBehaviour
{
[SerializeField] private Transform destination;
   public Transform GetDestination()
    {
        return destination;
    }
    
}
