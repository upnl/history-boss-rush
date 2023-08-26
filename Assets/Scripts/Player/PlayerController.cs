using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IPlayerController
{
    #region Internal
    private Rigidbody2D _rb;
    private BoxCollider2D _col;
    protected PlayerInput _input;

    private FrameInput FrameInput;
    private float _fixedTime;

    private bool _isAlive = true;
    private bool _hasControl = true;

    private GameStateManager _GameStateManager;
    private QuestManager _QuestManager;
    #endregion

    #region External

    public event Action<bool, Vector2> DashingChanged;
    public event Action<Vector2> Attacked;
    public event Action AttackEnd;
    public event Action<bool> ShootStanceChanged;
    public event Action<Vector2, bool> Shotted;

    public Vector2 MousePosition { get; set; }
    public Vector2 PlayerInput => currentPlayerDirecton;
    public Vector2 PlayerDirection => cachedPlayerDirection;
    public bool IsDashing { get; private set; }

    public bool CanDash => _canDash;

    public void TakeAwayControl()
    {
        _canDash = _canAttack = _canMove = false;
        _hasControl = false;
    }

    public void ReturnControl(bool canDashOverride = false)
    {
        _canDash = canDashOverride;
        _canMove = true;
        _hasControl = true;
    }

    #endregion

    private void Start()
    {
        _GameStateManager = GameManager.Instance.GameStateManager;
        _QuestManager = GameManager.Instance.QuestManager;

        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponentInChildren<BoxCollider2D>();
    }

    #region Update

    private void Update()
    {
        _fixedTime += Time.fixedDeltaTime;
        GatherInput();

        HandleDashing();
        HandleAttacking();
        HandleShooting();
        HandleWriting();
        HandleMoving();
    }

    private float ShootDownElapsedTime = 0f;
    private float ShootTriggerTime = 0.75f;
    protected virtual void GatherInput()
    {
        MousePosition = (Vector2)FrameInput.MousePosition;
        FrameInput = _input.FrameInput;

        if (FrameInput.WriteDown && !_isWriting) _writingToConsume = true;
        if (FrameInput.DashDown && !IsDashing) _dashToConsume = true;
        if (FrameInput.AttackDown && !_isAttacking) _attackToConsume = true;
        if (FrameInput.ShootDown && !_isShooting)
        {
            ShootStanceChanged?.Invoke(true);
            _isShootingStance = true;
            _canDash = false;
            _canMove = false;
            ShootDownElapsedTime = 0f;
        }
        if (FrameInput.ShootHeld && !_isShooting)
        {
            if(!_isShootingStance)
            {
                _isShootingStance = true;
                ShootStanceChanged?.Invoke(true);
            }
            _canDash = false;
            _canMove = false;
            ShootDownElapsedTime += Time.deltaTime;
        }
        if (FrameInput.ShootUp && !_isShooting)
        {
            if (ShootDownElapsedTime > ShootTriggerTime)
            {
                _shootToConsume = true;
                ShootDownElapsedTime = 0f;
            }
            else
            {
                _isShootingStance = false;
                ShootStanceChanged?.Invoke(false);
                _canDash = true;
                _canMove = true;
            }
        }
    }
    #endregion

    #region Moving

    private float playerSpeed = 4f;
    private bool _canMove = true;
    private float dashMovementModifier = 1.5f;

    private Vector2 currentPlayerDirecton = new Vector3(0f, 0f, 0f);
    private Vector2 cachedPlayerDirection = new Vector2(0f, 0f);

    private void HandleMoving()
    {
        if (_isWriting) return;

        if (FrameInput.Move.x != 0 || FrameInput.Move.y != 0)
        {
            currentPlayerDirecton = new Vector3(FrameInput.Move.x, FrameInput.Move.y).normalized;
            cachedPlayerDirection = currentPlayerDirecton;
        }
        else
            currentPlayerDirecton = new Vector3(0f, 0f, 0f);

        
        if (!IsDashing && _canMove)
        {
            int speedLvl = BookManager.Instance.CheckBookEquipped("Tenacity");

            float effect1 = float.Parse(BookManager.Instance.bookDB.GetData().Find(
                e => e[BookManager.Instance.bookDB.GetHeaderIndex("title")].Equals("Tenacity") &&
                int.Parse(e[BookManager.Instance.bookDB.GetHeaderIndex("level")]) == speedLvl)[BookManager.Instance.bookDB.GetHeaderIndex("effect1")]);

            var posX = transform.position.x + (playerSpeed * effect1 / 100f) * currentPlayerDirecton.x * Time.deltaTime;
            var posY = transform.position.y + (playerSpeed * effect1 / 100f) * currentPlayerDirecton.y * Time.deltaTime;

            transform.position = new Vector3(posX, posY, 0f);
        }
        else if(IsDashing && _canMove)
        {
            var posX = transform.position.x + (playerSpeed / dashMovementModifier) * currentPlayerDirecton.x * Time.deltaTime;
            var posY = transform.position.y + (playerSpeed / dashMovementModifier) * currentPlayerDirecton.y * Time.deltaTime;

            transform.position = new Vector3(posX, posY, 0f);
        }
    }

    #endregion

    #region Dahsing

    private bool _dashToConsume = false;
    private bool _canDash = true;
    private bool _dashCoolTime = true;

    private float dashConstant = 5f;
    private float dashTime = 0.5f;
    private float dashCoolTime = 0.75f;

    [SerializeField] private GameObject dashDummy;
    private float dashStartUpTime = 0.05f;
    private float dashInvulnTime = 0.3f;


    private void HandleDashing()
    {
        if (!_dashToConsume) return;

        if (_dashToConsume && _canDash && _canMove && _dashCoolTime)
        {
            if (_GameStateManager.dashTemp)
            {
                _GameStateManager.dashTemp = false;
            }
            else
            {
                IsDashing = true;
                _dashCoolTime = false;
                DashingChanged?.Invoke(true, cachedPlayerDirection);
                StartCoroutine(Dash());
            }
        }

        _dashToConsume = false;
    }

    private void ResetDash()
    {
        IsDashSuccess = false;
        _dashCoolTime = true;
    }

    private float elapsedTime = 0f;
    private IEnumerator Dash()
    {
        GameObject dashDummyObject = Instantiate(dashDummy, transform.position, Quaternion.identity);
        JustDashDummy dashDummyComponent = dashDummyObject.GetComponent<JustDashDummy>();
        dashDummyComponent.StartUp(dashStartUpTime, dashInvulnTime, this);
        Vector3 dashDirection = new Vector3(cachedPlayerDirection.x, cachedPlayerDirection.y, 0f);
        elapsedTime = 0f;
        while (elapsedTime < dashTime)
        {
            // transform.position += dashDirection * dashConstant * Time.deltaTime * (dashTime - elapsedTime) * (dashTime - elapsedTime);
            float dashMovement = Mathf.Lerp(dashConstant, dashConstant * 0.75f, elapsedTime / dashTime);
            transform.position += dashDirection * dashMovement * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        IsDashing = false;
        yield return new WaitForSeconds(dashCoolTime - dashTime);
        ResetDash();
    }

    private bool IsDashSuccess = false;
    public void OnDashSuccess()
    {
        // To Prevent one dash succeding twice
        if(!IsDashSuccess)
        {
            IsDashSuccess = true;
            Debug.Log("DashSuccess");
            // book related things
        }
    }

    #endregion

    #region Attacking

    private bool _attackToConsume = false;
    private bool _isAttacking = false;

    private bool _canAttack = true;
    private float _attackDelay = 0.5f;

    private void HandleAttacking()
    {
        if (!_attackToConsume) return;
        if (_isAttacking || _isWriting || !_isAlive || IsDashing || _isShootingStance || _isShooting) return;

        if (_canAttack)
        {
            Debug.Log("Attacking");
            var attackDirection = new Vector2(FrameInput.MousePosition.x - transform.position.x, FrameInput.MousePosition.y - transform.position.y).normalized;

            Attacked?.Invoke(attackDirection);
            StartCoroutine(AttackDelay());
        }

        _attackToConsume = false;
    }

    private IEnumerator AttackDelay()
    {
        _canAttack = false;
        _canDash = false;
        // _canMove = false;
        _isAttacking = true;

        yield return new WaitForSeconds(_attackDelay);

        _canDash = true;
        _canAttack = true;
        // _canMove = true;
        _isAttacking = false;

        AttackEnd?.Invoke();
    }

    //private void SwordAttack()
    //{
    //    // float z = Quaternion.FromToRotation(Vector3.up, (targetPos - transform.position).normalized).eulerAngles.z;
    //    // AudioManager.Instance.PlaySfx(0);
    //    // GameObject slash = Instantiate<GameObject>(slashPrefab, transform.position, Quaternion.identity);
    //    // slash.transform.localRotation = Quaternion.Euler(slash.transform.localRotation.x, slash.transform.localRotation.y, z + 90f);
    //    // StartCoroutine(AttackDelay());
    //    // Destroy(slash, 0.4f);
    //}

    #endregion

    #region Shooting
    private bool _shootToConsume = false;
    private bool _isShooting = false;
    private bool _isShootingStance = false;

    private bool _canShoot = true;
    private float _shootDelay = 0.15f;

    private void HandleShooting()
    {
        if (!_isShootingStance) return;
        if (_isShooting || _isWriting || !_isAlive || _isAttacking || IsDashing) return;

        if (_canShoot)
        {
            if(_shootToConsume)
            {
                Debug.Log("Shooting");
                var attackDirection = new Vector2(FrameInput.MousePosition.x - transform.position.x, FrameInput.MousePosition.y - transform.position.y).normalized;

                Shotted?.Invoke(attackDirection, true);
                StartCoroutine(ShootDelay());
            }

            _shootToConsume = false;
        }

        
    }

    private IEnumerator ShootDelay()
    {
        _canShoot = false;
        _isShooting = true;

        yield return new WaitForSeconds(_shootDelay);

        _canShoot = true;

        _canMove = true;
        _canDash = true;
        _isShootingStance = false;
        ShootStanceChanged?.Invoke(false);

        _isShooting = false;
    }

    #endregion

    #region Writing
    [SerializeField] private Slider writingSlider;

    private bool _writingToConsume = false;
    private bool _isWriting = false;
    private float _writingTime = 1f;

    private void HandleWriting()
    {
        if (!_writingToConsume) return;
        if (_isWriting || _isAttacking) return;

        if (_writingToConsume && _QuestManager.HasQuest)
        {
            StartCoroutine(WriteBook());
        }

        _writingToConsume = false;
    }

    private IEnumerator WriteBook()
    {
        writingSlider.gameObject.SetActive(true);
        writingSlider.value = 1f;

        _isWriting = true;
        _canMove = false;
        _canDash = false;

        GameManager.Instance.QuestManager.StopTimer = true;
        
        float elapsedTime = 0f;
        while (elapsedTime < _writingTime)
        {
            elapsedTime += Time.deltaTime;
            writingSlider.value = 1f - elapsedTime / _writingTime;
            yield return null;
        }

        if (_isAlive)
        {
            _canDash = true;
            _canMove = true;
            GameManager.Instance.QuestManager.UnlockBook = true;
        }
        writingSlider.gameObject.SetActive(false);
        _isWriting = false;
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("DeathHitBox"))
        {
            Debug.Log("Death");
            GetDamaged();

            //if(IsDashing && elapsedTime > dashStartUpTime && elapsedTime < dashStartUpTime + dashInvulnTime)
            //{
            //    OnDashSuccess();
            //}
            //else
            //{
            //    Debug.Log("Death");
            //    GetDamaged();
            //}
        }
    }

    public void GetDamaged()
    {
        _isAlive = false;
        _canMove = false;
        _canDash = false;
        GameManager.Instance.GameStateManager.Lose();
    }
}

public interface IPlayerController
{
    public event Action<bool, Vector2> DashingChanged;
    public event Action<Vector2> Attacked;
    public event Action AttackEnd;
    public event Action<bool> ShootStanceChanged;
    public event Action<Vector2, bool> Shotted;

    public void OnDashSuccess();
    public Vector2 MousePosition { get; set; }
    public Vector2 PlayerInput { get; }
    public Vector2 PlayerDirection { get; }

    public bool IsDashing { get; }
}