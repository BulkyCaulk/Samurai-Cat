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
        _cameraOriginalPositionY = _cameraTransform.localScale.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
