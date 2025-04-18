using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomCamera : MonoBehaviour
{
    [SerializeField] Transform _cameraTransform;
    [SerializeField] Transform _playerPosition;
    private float _cameraOriginalPositionY;
    private float _cameraNewPositionY;
    // Start is called before the first frame update
    void Start()
    {
        _cameraOriginalPositionY = _cameraTransform.position.y;
        _cameraNewPositionY = _cameraOriginalPositionY;
    }

    // Update is called once per frame
    void Update()
    {
        if(_cameraNewPositionY < _cameraOriginalPositionY)
        {
            //Debug.Log($"{_cameraNewPositionY} when its less than to original camera position");
            _cameraNewPositionY = _cameraOriginalPositionY;
        }
        if(_playerPosition.position.y >= _cameraOriginalPositionY)
        {
            //Debug.Log($"{_cameraNewPositionY} when its more than or equal to original camera position");
            _cameraNewPositionY = _playerPosition.position.y;
        }
        _cameraTransform.position = new Vector3(_cameraTransform.position.x, _cameraNewPositionY, _cameraTransform.position.z);
    }
}
