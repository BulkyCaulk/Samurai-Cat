using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectsToDraw;
    [SerializeField] private float _radius;
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        foreach(GameObject objectToDraw in _objectsToDraw)
        {
            Gizmos.DrawWireSphere(objectToDraw.transform.position, _radius);
        }
    }
}
