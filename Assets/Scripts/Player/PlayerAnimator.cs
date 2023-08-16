using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerAnimator : MonoBehaviour
{
    private IPlayerController _player;
    private Animator _anim;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;

    [SerializeField] private WeaponAnimator _weaponAnimator;

    private void Awake()
    {
        _player = GetComponentInParent<IPlayerController>();
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _player.DashingChanged += OnDashingChanged;
    }

    #region Dashing
    private bool isDashing = false;
    private float dashingDirectionDegree;
    private float dashingAnimationTime = 0.75f;

    private void OnDashingChanged(bool idDashing, Vector2 dashDirection)
    {
        isDashing = true;
        dashingDirectionDegree = CalculateDegreeWithVector(dashDirection);
    }
    #endregion

    private float playerDirectionDegree;

    private void Update()
    {
        // Debug.Log(playerDirectionDegree);
        playerDirectionDegree = CalculateDegreeWithVector(_player.PlayerDirection);
        HandleSpriteFlipping();
        HandleAnimations();
    }

    private void HandleSpriteFlipping()
    {
        // if (IsAttacking) _renderer.flipX = _renderer.flipX = _attackFlipDirection == -1;            
        if (Mathf.Abs(_player.PlayerInput.x) > 0.1f)
        {
            if (_player.PlayerInput.x < 0)
            {
                _spriteRenderer.flipX = true; 
                _weaponAnimator.playerFliped = true;
                // transform.parent.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                _spriteRenderer.flipX = false;
                _weaponAnimator.playerFliped = false;
                // transform.parent.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    #region Animations
    private float _lockedTill;

    private void HandleAnimations()
    {
        var state = GetState();
        ResetFlags();
        if (state == _currentState) return;

        _anim.Play(state, 0); //_anim.CrossFade(state, 0, 0);
        _currentState = state;

        int GetState()
        {
            if (Time.time < _lockedTill) return _currentState;

            //if (_attacked)
            //{
            //    if (_attackAngle < 45f || _attackAngle > 360f - 45f)
            //        return LockState(AttackDown, _attackAnimTime);
            //    if ((_attackAngle >= 45f && _attackAngle < 135f) || (_attackAngle >= 225 && _attackAngle < 315))
            //        return LockState(AttackForward, _attackAnimTime);

            //    return LockState(AttackUp, _attackAnimTime);
            //}

            // Dash
            if (isDashing)
            {
                if (dashingDirectionDegree < 40f || dashingDirectionDegree > 360f - 40f)
                    return LockState(DashNorth, dashingAnimationTime);
                if (dashingDirectionDegree < 180f + 40f && dashingDirectionDegree > 180f - 40f)
                    return LockState(DashSouth, dashingAnimationTime);
                return LockState(DashSide, dashingAnimationTime);
            }

            // Idle and Run
            if (playerDirectionDegree < 40f || playerDirectionDegree > 360f - 40f)
                return _player.PlayerInput.magnitude <= 0.1f ? IdleNorth : RunNorth;
            if (playerDirectionDegree < 180f + 40f && playerDirectionDegree > 180f - 40f)
                return _player.PlayerInput.magnitude <= 0.1f ? IdleSouth : RunSouth;
            return _player.PlayerInput.magnitude <= 0.1f ? IdleSide : RunSide;

            int LockState(int s, float t)
            {
                _lockedTill = Time.time + t;
                return s;
            }
        }

        void ResetFlags()
        {
            isDashing = false;
        }
    }
    #endregion

    #region Cached

    private int _currentState;

    private static readonly int IdleSide = Animator.StringToHash("IdleSide");
    private static readonly int IdleNorth = Animator.StringToHash("IdleNorth");
    private static readonly int IdleSouth = Animator.StringToHash("IdleSouth");

    //private static readonly int IdleSide = Animator.StringToHash("IdleSide");
    //private static readonly int IdleNorth = Animator.StringToHash("RunNorth");
    //private static readonly int IdleSouth = Animator.StringToHash("RunSouth");

    private static readonly int RunSide = Animator.StringToHash("RunSide");
    private static readonly int RunNorth = Animator.StringToHash("RunNorth");
    private static readonly int RunSouth = Animator.StringToHash("RunSouth");

    private static readonly int DashSide = Animator.StringToHash("DashSide");
    private static readonly int DashNorth = Animator.StringToHash("DashNorth");
    private static readonly int DashSouth = Animator.StringToHash("DashSouth");
    private static readonly int DashBack = Animator.StringToHash("DashBack");
    #endregion

    #region Utils
    private float CalculateDegreeWithVector(Vector2 lookDirection)
        => (270f + Mathf.Atan2(lookDirection.y, lookDirection.x) / Mathf.PI * 180f) % 360f;
    #endregion
}
