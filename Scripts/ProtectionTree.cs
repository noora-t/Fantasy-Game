using UnityEngine;

public class ProtectionTree : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Enemy")
            other.gameObject.GetComponent<EnemyMovement>().IsGoingToOriginalPosition = true;
    }
}
