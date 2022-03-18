using UnityEngine;
using Random = UnityEngine.Random;

public class WorldArea : MonoBehaviour
{
    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 center = transform.position;
        Vector3 extents = _collider.bounds.extents;

        Vector3 localPos = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        );

        return localPos + center;
    }
}
