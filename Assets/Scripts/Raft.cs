using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raft : MonoBehaviour
{
    [SerializeField] private GameObject _raftWalls;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _raftWalls.SetActive(true);
        }
    }
}
