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
        HandleWriting();
        HandleMoving();
    }

    protected virtual void GatherInput()
    {
        MousePosition = (Vector2)FrameInput.MousePosition;
        FrameInput = _input.FrameInput;

        if (FrameInput.WriteDown) _writingToConsume = true;
        if (FrameInput.DashDown) _dashToConsume = true;
        if (FrameInput.AttackDown) _attackToConsume = true;
        if (FrameInput.ShootDown) _shootToConsume = true;
    }
    #endregion

    #region Moving

    private bool _canMove = true;

    private Vector2 currentPlayerDirecton = new Vector3(0f, 0f, 0f);
    private Vector2 cachedPlayerDirection = new Vector2(0f, 0f);

    private void HandleMoving()
    {
        if (_isAttacking || _isWriting) return;

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
    }

    #endregion

    #region Dahsing

    private bool _dashToConsume = false;
    private bool _canDash = true;

    private float playerSpeed = 5f;
    private float dashConstant = 20f;
    private float dashTime = 0.75f;
    private float dashCoolTime = 2f;

    private void HandleDashing()
    {
        if (!_dashToConsume) return;

        if (_dashToConsume && _canDash && _canMove)
        {
            if (_GameStateManager.dashTemp)
            {
                _GameStateManager.dashTemp = false;
            }
            else
            {
                IsDashing = true;
                _canDash = false;
                DashingChanged?.Invoke(true, cachedPlayerDirection);
                StartCoroutine(Dash());
            }
        }

        _dashToConsume = false;
    }

    private void ResetDash()
    {
        _canDash = true;
    }

    private IEnumerator Dash()
    {
        Vector3 dashDirection = new Vector3(cachedPlayerDirection.x, cachedPlayerDirection.y, 0f);
        float elapsedTime = 0f;
        while (elapsedTime < dashTime)
        {
            transform.position += dashDirection * dashConstant * Time.deltaTime * (dashTime - elapsedTime) * (dashTime - elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        IsDashing = false;
        yield return new WaitForSeconds(dashCoolTime - dashTime);
        ResetDash();
    }

    #endregion

    #region Attacking

    private bool _attackToConsume = false;
    private bool _isAttacking = false;
    private bool _shootToConsume = false;
    private bool _isShooting = false;

    private bool _canAttack = true;
    private float _attackDelay = 0.66f;

    private void HandleAttacking()
    {
        if (!_attackToConsume) return;
        if (_isAttacking || _isWriting || !_isAlive) return;

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
        _canMove = false;
        _isAttacking = true;

        yield return new WaitForSeconds(_attackDelay);
        
        _canAttack = true;
        _canMove = true;
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
            _canMove = true;
            GameManager.Instance.QuestManager.UnlockBook = true;
        }
        writingSlider.gameObject.SetActive(false);
        _isWriting = false;
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Debug.Log("Hi");
        if (col.gameObject.layer == LayerMask.NameToLayer("DeathHitBox"))
        {
            // GetDamaged();
        }
    }

    public void GetDamaged()
    {
        _isAlive = false;
        _canMove = false;
        GameManager.Instance.GameStateManager.Lose();
    }
}

public interface IPlayerController
{
    public event Action<bool, Vector2> DashingChanged;
    public event Action<Vector2> Attacked;
    public event Action AttackEnd;
    public event Action<Vector2, bool> Shotted;

    public Vector2 MousePosition { get; set; }
    public Vector2 PlayerInput { get; }
    public Vector2 PlayerDirection { get; }

    public bool IsDashing { get; }
}