using UnityEngine;

namespace Gizmos
{
    public static class GizmosExtensions
    {
        public static void DrawCircle(Vector3 center, Vector3 forwardDirection, Vector3 rightDirection,
            int vertexCount, float radius)
        {
            float deltaAngle = 360.0f / vertexCount;
            
            Vector3 startPosition = center + rightDirection * radius;
            Vector3 previousPosition = startPosition;

            for (int i = 1; i <= vertexCount - 1; i++)
            {
                float angleInRadians = deltaAngle * i * Mathf.Deg2Rad;
                Vector3 currentPosition = center + (forwardDirection * Mathf.Sin( angleInRadians) 
                                            + rightDirection * Mathf.Cos(angleInRadians)) * radius;
                UnityEngine.Gizmos.DrawLine(previousPosition, currentPosition);
                
                previousPosition = currentPosition;
            }
            
            UnityEngine.Gizmos.DrawLine(previousPosition, startPosition);
        }
    }
}