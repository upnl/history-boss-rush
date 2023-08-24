using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _boss;
    private PlayerInput _frameInput;

    [SerializeField] private float _smoothTime = 0.3f;
    [SerializeField] private Vector3 _offset = new Vector3(0, 1);
    [SerializeField] private float _lookAheadModifier = 0.3f;
    [SerializeField] private float _bossModifier = 0.3f;
    [SerializeField] private float _lookAheadSpeed;

    private Vector3 _vel;
    private Vector3 _lookAheadVel;
    private Vector3 _bossOffset;
    private Vector3 _mousePosition;
    private Vector3 _lookAheadOffset;

    private void Start()
    {
        _frameInput = _player.GetComponent<PlayerInput>();
    }

    private void LateUpdate()
    {
        _mousePosition = _frameInput.FrameInput.MousePosition;

        if (_player != null)
        {
            var projectedPos = (_mousePosition - _player.position) * _lookAheadModifier;
            _lookAheadOffset = Vector3.SmoothDamp(_lookAheadOffset, projectedPos, ref _lookAheadVel, _lookAheadSpeed);
        }
        if (_boss != null)
        {
            var projectedPos = (_boss.position - _player.position) * _bossModifier;
            _bossOffset = Vector3.SmoothDamp(_bossOffset, projectedPos, ref _lookAheadVel, _lookAheadSpeed);
        }

        Step(_smoothTime);
    }

    private void OnValidate() => Step(0);

    private void Step(float time)
    {
        var goal = _player.position + _offset + _lookAheadOffset + _bossOffset;
        transform.position = Vector3.SmoothDamp(transform.position, goal, ref _vel, time);
    }
}
