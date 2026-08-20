using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    public float radius = 3f;
    public LayerMask objectLayer;

    void Update()
    {
        Collider[] objects = Physics.OverlapSphere(
            transform.position,
            radius,
            objectLayer
        );

        Debug.Log("Total Objects: " + objects.Length);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}