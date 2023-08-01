using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour, IPlayerController
{
    #region Internal
    private float playerSpeed = 5f;
    private float dashConstant = 80f;
    private float dashTime = 0.5f;
    private float rollingCoolTime = 3f;

    private Vector2 currentPlayerDirecton = new Vector3(0f, 1f);
    private Vector2 cachedPlayerDirection = new Vector2(0f, 1f);

    private Vector2 _PlayerInput;
    private GameStateManager _GameStateManager;
    #endregion

    // [HideInInspector] public bool IsDash = false;
    [HideInInspector] public bool CanDash = true;
    [HideInInspector] public bool CanMove = true;

    // private Animator animator;

    #region External
    public event Action<bool, Vector2> DashingChanged;

    public Vector2 PlayerInput => _PlayerInput;
    public Vector2 PlayerDirection => cachedPlayerDirection;
    public bool IsDashing { get; private set; }
    #endregion


    private void Start()
    {
        _GameStateManager = GameManager.Instance.GameStateManager;
        // animator = GetComponentInChildren<Animator>();
    }
    
    private void Update()
    {
        _PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (_PlayerInput.x != 0 || _PlayerInput.y != 0)
        {
            currentPlayerDirecton = new Vector3(_PlayerInput.x, _PlayerInput.y).normalized;
            cachedPlayerDirection = currentPlayerDirecton;
        }
        else
            currentPlayerDirecton = new Vector3(0f, 0f, 0f);

        if (Input.GetKeyDown(KeyCode.Space) && CanDash && CanMove)
        {
            if (_GameStateManager.dashTemp)
            {
                _GameStateManager.dashTemp = false;
            }
            else
            {
                IsDashing = true;
                CanDash = false;
                DashingChanged?.Invoke(true, cachedPlayerDirection);
                StartCoroutine(Dash());
            }
        }
        else if (!IsDashing && CanMove)
        {
            int speedLvl = BookManager.Instance.CheckBookEquipped("Tenacity");

            float effect1 = float.Parse(BookManager.Instance.bookDB.GetData().Find(
                e => e[BookManager.Instance.bookDB.GetHeaderIndex("title")].Equals("Tenacity") &&
                int.Parse(e[BookManager.Instance.bookDB.GetHeaderIndex("level")]) == speedLvl)[BookManager.Instance.bookDB.GetHeaderIndex("effect1")]);

            var posX = transform.position.x + (playerSpeed * effect1 / 100f) * _PlayerInput.x * Time.deltaTime;
            var posY = transform.position.y + (playerSpeed * effect1 / 100f) * _PlayerInput.y * Time.deltaTime;

            transform.position = new Vector3(posX, posY, 0f);
        }
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
        yield return new WaitForSeconds(rollingCoolTime - dashTime);
        CanDash = true;
    }
}

public interface IPlayerController
{
    public event Action<bool, Vector2> DashingChanged;

    public Vector2 PlayerInput { get; }
    public Vector2 PlayerDirection { get; }
    
    public bool IsDashing { get; }
}