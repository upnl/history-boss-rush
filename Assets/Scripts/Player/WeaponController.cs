using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WeaponController : MonoBehaviour
{
    private IPlayerController _player;
    [SerializeField] private BoxCollider2D _hitbox;
    [SerializeField] private Transform _bulletSpawnPosition;
    [SerializeField] private GameObject _bulletPrefab;

    private LayerMask _hitboxLayer;

    #region Inner
    private bool _isAttacking;
    private bool _isShooting;
    #endregion

    #region External

    #endregion

    private void Awake()
    {
        _player = GetComponentInParent<IPlayerController>();

        _player.Attacked += OnAttacked;
        _player.Shotted += OnShooted;

        _isAttacking = false;
        _isShooting = false;
    }

    private void Update()
    {
        if (_isAttacking)
        {
            //Debug.Log(_hitbox.bounds.center);
            //Debug.Log(_hitbox.transform.eulerAngles.z);
            Collider2D[] weaponCols = Physics2D.OverlapBoxAll(_hitbox.bounds.center, 2 * _hitbox.bounds.extents, _hitbox.transform.eulerAngles.z, _hitboxLayer);

            // Debug.Log(weaponCols.Length);
            foreach (Collider2D collider in weaponCols)
            {
                if (collider.transform.root == transform.root)
                    continue;

                var objectController = collider.transform.root.GetComponent<Boss>();
                if (objectController != null)
                {
                    
                }


            }
        }

        else if (_isShooting)
        {
            //GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPosition.position, Quaternion.identity);
            //var bulletController = bullet.GetComponent<BulletController>();

            //bulletController.SetUp(_player.ObjectStats, _shootingDirection);
            //_isShooting = false;
        }
    }

    #region Player Event

    private float _attackAngle;
    private float _shootingAngle;
    private Vector2 _shootingDirection;

    private void OnAttacked(Vector2 attackDirection)
    {
        _attackAngle = CalculateAngle(attackDirection, transform.root.forward);
    }

    private void OnShooted(Vector2 shootingDirection, bool ammoLeft)
    {
        _isShooting = ammoLeft;
        _shootingDirection = shootingDirection;
        _shootingAngle = CalculateAngle(shootingDirection, transform.root.forward);
    }

    public static float CalculateAngle(Vector3 from, Vector3 to) => Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;

    #endregion

    #region Animator Event
    public void StartAttack()
    {
        // _hitbox.enabled = true;
        _isAttacking = true;
    }

    public void StopAttack()
    {
        // _hitbox.enabled = false;
        _isAttacking = false;
    }
    #endregion
}
