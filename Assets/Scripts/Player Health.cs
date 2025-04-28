using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _playerHealth;
    void Update()
    {
        //quit game 
        if(Input.GetKey("9") && Input.GetKey("1"))
        {
            Application.Quit();
        }
    }

    public void PlayerTakeDamage()
    {
        _playerHealth--;
        if(_playerHealth <= 0)
        {
            KillPlayer();
        }
    }
    private void KillPlayer()
    {
        Destroy(gameObject);
    }
}
