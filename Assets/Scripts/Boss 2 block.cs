using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss2block : MonoBehaviour
{
    [SerializeField] private GameObject _blockWall;
    [SerializeField] private GameObject _boss;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _boss.SetActive(true);
            _blockWall.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
