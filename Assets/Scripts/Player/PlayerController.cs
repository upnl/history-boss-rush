using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    #region Internal
    private Rigidbody2D _rb;
    private BoxCollider2D _col;
    protected PlayerInput _input;

    private FrameInput FrameInput;
    private float _fixedTime;

    private bool _hasControl = true;

    private GameStateManager _GameStateManager;
    #endregion

    #region External

    public event Action<bool, Vector2> DashingChanged;
    public event Action<Vector2> Attacked;
    public event Action<Vector2, bool> Shotted;

    public Vector2 PlayerInput => FrameInput.Move;
    public bool IsDashing { get; private set; }

    public void TakeAwayControl(bool resetVelocity = true)
    {
        if (resetVelocity) _rb.velocity = Vector2.zero;
        _hasControl = false;
    }

    public void ReturnControl()
    {
        _hasControl = true;
    }

    #endregion

    private void Awake()
    {
        _GameStateManager = GameManager.Instance.GameStateManager;
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponentInChildren<BoxCollider2D>();
    }

    #region Update

    private void Update()
    {
        GatherInput();
    }

    protected virtual void GatherInput()
    {
        FrameInput = _input.FrameInput;

        if (FrameInput.DashDown) _dashToConsume = true;
        if (FrameInput.AttackDown) _attackToConsume = true;
        if (FrameInput.ShootDown) _shootToConsume = true;
    }
    #endregion

    #region Moving

    private bool _canMove = true;

    private Vector2 currentPlayerDirecton = new Vector3(0f, 1f);
    private Vector2 cachedPlayerDirection = new Vector2(0f, 1f);

    private void HandleMoving()
    {
        if (PlayerInput.x != 0 || PlayerInput.y != 0)
        {
            currentPlayerDirecton = new Vector3(PlayerInput.x, PlayerInput.y).normalized;
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

            var posX = transform.position.x + (playerSpeed * effect1 / 100f) * PlayerInput.x * Time.deltaTime;
            var posY = transform.position.y + (playerSpeed * effect1 / 100f) * PlayerInput.y * Time.deltaTime;

            transform.position = new Vector3(posX, posY, 0f);
        }
    }

    #endregion

    #region Dahsing

    private bool _dashToConsume;
    private bool _canDash;

    private float playerSpeed = 5f;
    private float dashConstant = 80f;
    private float dashTime = 0.5f;
    private float dashCoolTime = 3f;

    private void HandleDashing()
    {
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

    protected virtual void ResetDash()
    {
        _canDash = true;
    }

    private IEnumerator Dash()
    {
        Vector3 dashDirection = new Vector3(currentPlayerDirecton.x, currentPlayerDirecton.y, 0f);
        float elapsedTime = 0f;
        while (elapsedTime < dashTime)
        {
            transform.position += dashDirection * dashConstant * Time.deltaTime * (dashTime - elapsedTime) * (dashTime - elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        IsDashing = false;
        yield return new WaitForSeconds(dashCoolTime - dashTime);
        _canDash = true;
    }

    #endregion

    #region Attacking

    private bool _attackToConsume = false;
    private bool _shootToConsume = false;

    #endregion
}

public interface IPlayerController
{
    public event Action<bool, Vector2> DashingChanged;

    public Vector2 PlayerInput { get; }
    public Vector2 PlayerDirection { get; }

    public bool IsDashing { get; }
}