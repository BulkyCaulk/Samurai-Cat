using UnityEngine;

[ExecuteAlways]
public class ColliderVisualizer2D : MonoBehaviour
{

    public Color gizmoColor = Color.green;

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;


        foreach (var col in FindObjectsOfType<Collider2D>())
        {

            Gizmos.matrix = col.transform.localToWorldMatrix;

            switch (col)
            {
                case BoxCollider2D box:

                    Gizmos.DrawWireCube(box.offset, box.size);
                    break;

                case CircleCollider2D circle:

                    Gizmos.DrawWireSphere(circle.offset, circle.radius);
                    break;

                case PolygonCollider2D poly:

                    Vector2[] pts = poly.points;
                    for (int i = 0; i < pts.Length; i++)
                    {
                        Vector3 p1 = pts[i];
                        Vector3 p2 = pts[(i + 1) % pts.Length];
                        Gizmos.DrawLine(p1, p2);
                    }
                    break;

                
            }
        }
    }
}
