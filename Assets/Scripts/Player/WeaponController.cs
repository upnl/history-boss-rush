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

    [SerializeField] private LayerMask _hitboxLayer;

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
        Vector2 direction = (_player.MousePosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        scale.y = direction.x < 0 ? -1 : 1;

        transform.localScale = scale;

        

        //else if (_isShooting)
        //{
        //    //GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPosition.position, Quaternion.identity);
        //    //var bulletController = bullet.GetComponent<BulletController>();

        //    //bulletController.SetUp(_player.ObjectStats, _shootingDirection);
        //    //_isShooting = false;
        //}
    }


    private void DebugDrawBox(Vector2 point, Vector2 size, float angle, Color color, float duration)
    {

        var orientation = Quaternion.Euler(0, 0, angle);

        // Basis vectors, half the size in each direction from the center.
        Vector2 right = orientation * Vector2.right * size.x / 2f;
        Vector2 up = orientation * Vector2.up * size.y / 2f;

        // Four box corners.
        var topLeft = point + up - right;
        var topRight = point + up + right;
        var bottomRight = point - up + right;
        var bottomLeft = point - up - right;

        // Now we've reduced the problem to drawing lines.
        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);
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

        DebugDrawBox(_hitbox.bounds.center, _hitbox.bounds.size, _hitbox.transform.eulerAngles.z, Color.white, 0.5f);
        Collider2D weaponCol = Physics2D.OverlapBox(_hitbox.bounds.center, _hitbox.bounds.size, _hitbox.transform.eulerAngles.z, _hitboxLayer);

        if(weaponCol != null)
        {
            var objectController = weaponCol.transform.GetComponent<Boss>();
            if (objectController != null)
            {
                Debug.Log("BossHit");
                objectController.GetDamaged();
            }
        }
    }

    public void StopAttack()
    {
        // _hitbox.enabled = false;
        _isAttacking = false;
    }
    #endregion
}
