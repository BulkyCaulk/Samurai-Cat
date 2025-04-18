using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlatforms : MonoBehaviour
{
    public float SpawnTimeSpan { get ; set ; }
    private float _speed;
    // Start is called before the first frame update
    void Start()
    {
        _speed = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector2.down * _speed * Time.deltaTime);
        //Debug.Log(this.transform.position);
        //Debug.Log(SpawnTimeSpan);
        SpawnTimeSpan -= Time.deltaTime;
    }
}
