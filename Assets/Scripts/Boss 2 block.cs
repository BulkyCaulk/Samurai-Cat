using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss2block : MonoBehaviour
{
    [SerializeField] private GameObject _blockWall;
    [SerializeField] private GameObject _boss;
    [SerializeField] private GameObject _platDeadBoss;
    private float _timeElapsed;
    private float _lerpDurp;
    private bool triggered;
    private Vector3 start;
    private Vector3 end;

    void Start()
    {
        triggered = false;
        _timeElapsed = 0;
        _lerpDurp = 5;
        start = new Vector3(_platDeadBoss.transform.position.x, _platDeadBoss.transform.position.y, _platDeadBoss.transform.position.z);
        end = new Vector3(start.x, 15, start.z);
    }

    void Update()
    {
        if (_boss == null)
        {
            if (_platDeadBoss.transform.position == end)
            {
                return;
            }
            while (_timeElapsed < _lerpDurp)
            {
                _platDeadBoss.transform.position = Vector3.Lerp(start, end, _timeElapsed / _lerpDurp);
                _timeElapsed += Time.deltaTime;
            }
            _platDeadBoss.transform.position = end;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !triggered)
        {
            triggered = true;
            _boss.SetActive(true);
            _blockWall.SetActive(true);
            //this.gameObject.SetActive(false);
        }
    }
}
