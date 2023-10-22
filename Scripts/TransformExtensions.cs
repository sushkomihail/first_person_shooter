using UnityEngine;

public static class TransformExtensions
{
    public static void SetLayer(this Transform transform, int layer)
    {
        transform.gameObject.layer = layer;
        foreach (Transform child in transform)
        {
            SetLayer(child, layer);
        }
    }

    public static Vector3 GlobalToLocalDirection(this Transform transform, Vector2 direction)
    {
        return transform.forward * direction.y + transform.right * direction.x;
    }
}