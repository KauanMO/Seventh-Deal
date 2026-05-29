using UnityEngine;

public class CardRotation : MonoBehaviour
{
    private void Start()
    {
        float randomAngle = Random.Range(-3f, 3f);

        transform.localRotation = Quaternion.Euler(0, 0, randomAngle);
    }
}