using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raft : MonoBehaviour
{
    [SerializeField] private GameObject _raftWalls;
    [SerializeField] private Transform _endPosition;
    private float _timer;
    private float _raftTravelTime;
    private Vector3 _startPosition;

    // Start is called before the first frame update
    void Start()
    {
        _timer = 0;
        _raftTravelTime = 10;
        _startPosition = this.transform.position;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _raftWalls.SetActive(true);
            Rigidbody2D playerRB = collision.GetComponent<Rigidbody2D>();
            playerRB.velocity = Vector2.zero;
            StartCoroutine(TravelByRaft(playerRB));
        }
    }

    IEnumerator TravelByRaft(Rigidbody2D playerRB)
    {
        
        while (_timer < _raftTravelTime)
        {
            this.transform.position = Vector3.Lerp(_startPosition, _endPosition.position, _timer / _raftTravelTime);
            

            _timer += Time.deltaTime;
            yield return null;
        }

        this.transform.position = _endPosition.position;
        
        _raftWalls.SetActive(false);

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }
}
