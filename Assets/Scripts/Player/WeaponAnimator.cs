using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    private IPlayerController _player;
    private WeaponController _weaponController;
    private Animator _anim;
    private SpriteRenderer _renderer;
    private AudioSource _source;

    [SerializeField] private Transform _weaponHolder;
    [HideInInspector] public bool playerFliped; 

    private void Awake()
    {
        _player = transform.root.GetComponent<IPlayerController>();
        _weaponController = GetComponentInParent<WeaponController>();
        _anim = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _player.DashingChanged += OnDashing;
        _player.Attacked += OnAttacked;
        _player.AttackEnd += OnAttackEnd;
        // _player.Shotted += OnShooting;
    }

    private float _time = 0f;
    private void Update()
    {
        _time += Time.deltaTime;

        HandleAnimations();
    }

    #region Attack

    private bool _attacked;
    private float _attackAngle;
    private float _fixedAngle;
    private float _attackAnimTime = 0.66f;
    // private int _attackFlipDirection;

    private void OnAttacked(Vector2 attackDirection)
    {
        _attackAngle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;

        HandleFlipping();
        Quaternion q = Quaternion.AngleAxis(_fixedAngle, Vector3.forward);
        _weaponHolder.rotation = q;
        
        _attacked = true;
    }

    private void OnAttackEnd()
    {
        HandleFlipping(true);
        Quaternion q = Quaternion.AngleAxis(playerFliped ? 180f : 0f, Vector3.forward);
        _weaponHolder.rotation = q;
    }

    public void HandleFlipping(bool angleOverride = false)
    {
        if (angleOverride)
            _attackAngle = (playerFliped ? 180f : 0f);
        _fixedAngle = _attackAngle;

        if (_attackAngle > 90f)
        {
            if (playerFliped)
                _weaponHolder.localScale = new Vector3(1f, 1f, 1f);
            else
                _weaponHolder.localScale = new Vector3(-1f, 1f, 1f);

            _fixedAngle = _attackAngle - 180f;
        }
        else if (_attackAngle < -90f)
        {
            if (playerFliped)
                _weaponHolder.localScale = new Vector3(1f, 1f, 1f);
            else
                _weaponHolder.localScale = new Vector3(-1f, 1f, 1f);
            _fixedAngle = _attackAngle + 180f;
        }
        else
            if (playerFliped)
                _weaponHolder.localScale = new Vector3(-1f, 1f, 1f);
            else
                _weaponHolder.localScale = new Vector3(1f, 1f, 1f);

    }

    public static float CalculateAngle(Vector3 from, Vector3 to) => Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;

    #endregion

    #region Dashing
    private bool _dashed = false;
    private float _dashTime = 0.66f;

    private void OnDashing(bool dashing, Vector2 dashingDirection)
    {
        _dashed = true;
    }
    #endregion

    #region Shooting
    private bool _shotted;
    private float _shootingAngle;
    private float _shootFixedAngle;
    private float _shootingAnimTime = 0.23f;
    // private int _attackFlipDirection;

    private void OnShooting(Vector2 shootingDirection, bool AmmoLeft)
    {
        _attackAngle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;

        HandleFlipping();

        Quaternion q = Quaternion.AngleAxis(_fixedAngle, Vector3.forward);
        _weaponHolder.rotation = q;

        _shotted = true;
    }
    #endregion

    #region Animaitons

    private float _lockedTill;

    private void HandleAnimations()
    {
        var state = GetState();
        ResetFlags();
        if (state == _currentState) return;

        _anim.Play(state, 0);
        _currentState = state;

        int GetState()
        {
            if (_time < _lockedTill) return _currentState;

            if (_attacked)
            {
                return LockState(Attack, _attackAnimTime);
            }

            if(_dashed)
            {
                return LockState(Dash, _dashTime);
            }

            int LockState(int s, float t)
            {
                _lockedTill = _time + t;
                return s;
            }

            return Idle;
        }

        void ResetFlags()
        {
            _dashed = false;
            _attacked = false;
        }
    }

    private void UnlockAnimationLock() => _lockedTill = 0f;

    #endregion

    #region Cached Properties
    private int _currentState;

    private static readonly int Idle = Animator.StringToHash("WeaponIdle");
    private static readonly int Attack = Animator.StringToHash("WeaponAttack");
    private static readonly int Dash = Animator.StringToHash("WeaponDash");
    #endregion

    #region WeaponController
    public void StartAttack() => _weaponController.StartAttack();

    public void StopAttack() => _weaponController.StopAttack();
    #endregion
}
