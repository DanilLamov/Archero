using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private float _dampTime = 1.5f;
    [SerializeField] private Vector3 _offset;

    private Player _player;

    public void Initialize(Player player)
    {
        _player = player;
        transform.position = _player.transform.position + _offset;
    }

    public void GameUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 target = new Vector3(_offset.x, _offset.y, (_player ? _player.transform.position.z : 0f ) + _offset.z);
        Vector3 currentPosition = Vector3.Lerp(transform.position, target, _dampTime * Time.fixedDeltaTime);
        transform.position = currentPosition;
    }
}