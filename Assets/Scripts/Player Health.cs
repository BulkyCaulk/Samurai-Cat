using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _playerHealth;
    public int Player_Health {get { return _playerHealth; } }
    void Update()
    {
        //quit game 
        if(Input.GetKey("9") && Input.GetKey("1"))
        {
            Application.Quit();
        }
        Debug.Log(_playerHealth);
    }

    public void PlayerTakeDamage()
    {
        _playerHealth--;
        if(_playerHealth <= 0)
        {
            // this may break...
            GameManager.Instance.ReloadToCheckpoint();
        }
    }
    private void KillPlayer()
    {
        Destroy(gameObject);
    }
}
